using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Zeus.Data;

namespace Zeus.Plugin.Repositories
{

    /// <summary>
    /// defines methods and properties that a repository inspector must implements.
    /// </summary>
    internal abstract class RepositoryBase
    {
        #region Fields

        /// <summary>
        /// The list of the possible plugins found in the repository.
        /// </summary>
        protected List<RepositoryRecord> m_Records;

        #endregion

        #region Methods

        /// <summary>
        /// Initialize the repository object.
        /// </summary>
        /// <param name="settings">The data needed to initialize the repository.</param>
        public abstract void Initialize(DataStore settings);
        /// <summary>
        /// Inspects the repository and retrieve information about avaialble plugins.
        /// </summary>
        public abstract void Inspect();
        /// <summary>
        /// Gets all the avaialble plugin <see cref="PluginFactory{T}"/> for the requested plugin type.
        /// </summary>
        /// <typeparam name="T">The type of the requested plugin.</typeparam>
        /// <returns>A collection of all the <see cref="PluginFactory{T}"/> capable to instantiate a plugin of the requested type.</returns>
        public IEnumerable<PluginFactory<T>> GetFactories<T>() where T : class
        {
            return m_Records.Where(r => r.ExportedType == typeof(T)).Select(r => new PluginFactory<T>(r.AssemblyName, r.AssemblyPath, r.TypeName));
        }
        /// <summary>
        /// Gets all the avaialble plugin <see cref="PluginFactory{T}"/> for the requsted plugin type and that satisfy the filtering function.
        /// </summary>
        /// <typeparam name="T">The type of the requestd plugin.</typeparam>
        /// <typeparam name="TMetaData">The type of the plugin metadata.</typeparam>
        /// <param name="filter">The filtering funtion used to filter the available plugins.</param>
        /// <returns>A collection of all the <see cref="PluginFactory{T}"/> capable to instantiate a plgin of the request type and which metadata satisfy the filtering function.</returns>
        public IEnumerable<PluginFactory<T>> GetFactories<T, TMetaData>(Func<TMetaData, bool> filter) where T : class where TMetaData : class
        {
            return m_Records.Where(r => r.ExportedType == typeof(T))
                .Where(r => r.MetaData.Count > 0)
                .Where(r => filter(GetMetaData<TMetaData>(r.MetaData)))
                .Select(r => new PluginFactory<T>(r.AssemblyName, r.AssemblyPath, r.TypeName));
        }
        /// <summary>
        /// Inspect the given assembly to discovery all avaialble plugins.
        /// </summary>
        /// <param name="asm">The <see cref="Assembly"/> to inspect.</param>
        /// <returns>The list of the information about the founded plugins.</returns>
        protected static List<RepositoryRecord> InspectAssembly(Assembly asm)
        {
            List<RepositoryRecord> records = new List<RepositoryRecord>();
            //loop over all the public types of the assembly
            foreach (Type t in asm.GetExportedTypes())
            {
                //check if at least one type attrbute is an instance of ExportPluginAttribute or at least inherit from it
                foreach (ExportPluginAttribute epa in t.GetCustomAttributes<ExportPluginAttribute>())
                {
                    //check for metadata
                    Dictionary<string, object> metadata = new Dictionary<string, object>();
                    foreach (ExportPluginMetadataAttribute epma in t.GetCustomAttributes<ExportPluginMetadataAttribute>())
                    {
                        metadata.Add(epma.Name, epma.Value);
                    }
                    //add a new record to the results list
                    records.Add(new RepositoryRecord(asm.FullName, asm.Location, t.FullName, epa.PluginType, metadata));
                }
            }
            return records;
        }
        /// <summary>
        /// Gets the metadata object instance. Eventually create a runtime dummy type to store metadata if <typeparamref name="TMetaData"/> <see cref="Type"/> id an interface.
        /// </summary>
        /// <typeparam name="TMetaData">The type of the metadata.</typeparam>
        /// <param name="metadata">The metadata information.</param>
        /// <returns>The object of type <typeparamref name="TMetaData"/> that contains the metadata information.</returns>
        private static TMetaData GetMetaData<TMetaData>(Dictionary<string, object> metadata) where TMetaData : class
        {
            TMetaData res;
            Type metadataType = typeof(TMetaData);
            //check metadata type
            if (metadataType.IsInterface)
            {
                res = (TMetaData)Activator.CreateInstance(CreateDummyType(metadataType));

            }
            else
            {
                res = Activator.CreateInstance<TMetaData>();
            }
            //fill metadata object
            Type returnObjType = res.GetType();
            foreach (KeyValuePair<string, object> metadataKVP in metadata)
            {
                PropertyInfo pi = returnObjType.GetProperty(metadataKVP.Key);
                pi?.SetValue(res, metadataKVP.Value);
            }
            return res;
        }
        /// <summary>
        /// Create a dummy type for store metadata information. USed when metadata type is an interface.
        /// </summary>
        /// <param name="metadataType">The metadata interface <see cref="Type"/>.</param>
        /// <returns>A new type that implement the provided interface.</returns>
        private static Type CreateDummyType(Type metadataType)
        {
            //create a type builder object
            TypeBuilder tb = AppDomain.CurrentDomain
                .DefineDynamicAssembly(new AssemblyName("PluginRepository"), AssemblyBuilderAccess.Run)
                .DefineDynamicModule("PluginRepository")
                .DefineType("Dummy" + metadataType.Name);
            //implement the interface
            tb.AddInterfaceImplementation(metadataType);
            //implement interface methods
            foreach (MethodInfo mi in metadataType.GetMethods())
            {
                if (!mi.IsSpecialName)
                {
                    MethodBuilder mb = tb.DefineMethod(mi.Name, MethodAttributes.Public | MethodAttributes.Virtual, mi.ReturnType, mi.GetParameters().Select(pi => pi.ParameterType).ToArray());
                    if (mi.IsGenericMethod)
                    {
                        mb.DefineGenericParameters(mi.GetGenericArguments().Select(t => t.Name).ToArray());
                    }
                    ILGenerator gen = mb.GetILGenerator();
                    gen.ThrowException(typeof(NotImplementedException));
                }
            }
            //implement interface properties
            foreach (PropertyInfo pi in metadataType.GetProperties())
            {
                FieldBuilder fb = tb.DefineField("m_" + pi.Name, pi.PropertyType, FieldAttributes.Private);
                PropertyBuilder pb = tb.DefineProperty(pi.Name, PropertyAttributes.None, pi.PropertyType, null);
                MethodBuilder mbGet = tb.DefineMethod("get_" + pi.Name, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual, pi.PropertyType, null);
                ILGenerator genGet = mbGet.GetILGenerator();
                genGet.Emit(OpCodes.Ldarg_0);
                genGet.Emit(OpCodes.Ldfld, fb);
                genGet.Emit(OpCodes.Ret);
                MethodBuilder mbSet = tb.DefineMethod("set_" + pi.Name, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual, null, new[] { pi.PropertyType });
                ILGenerator genSet = mbSet.GetILGenerator();
                genSet.Emit(OpCodes.Ldarg_0);
                genSet.Emit(OpCodes.Ldarg_1);
                genSet.Emit(OpCodes.Stfld, fb);
                genSet.Emit(OpCodes.Ret);
                pb.SetGetMethod(mbGet);
                pb.SetSetMethod(mbSet);
            }
            //create the dummy type
            return tb.CreateType();
        }
        /// <summary>
        /// Compare the current repository object with the settings provided to check if it is the same.
        /// </summary>
        /// <param name="settings">the repository settings that has to be checked.</param>
        /// <returns>Returns true if the repository are the same, false otherwise.</returns>
        public abstract bool EqualsTo(DataStore settings);

        #endregion
    }
}
