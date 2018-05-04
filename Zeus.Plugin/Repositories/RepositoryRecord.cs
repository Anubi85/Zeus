using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Zeus.Plugin.Repositories
{
    /// <summary>
    /// Contains all the information relative to a repository record.
    /// </summary>
    [Serializable]
    internal class RepositoryRecord
    {
        #region Fields

        /// <summary>
        /// A cache to store the dummy types created for the metadata interfaces.
        /// </summary>
        private static Dictionary<Type, Type> s_MetaDataTypesCache;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize class properties.
        /// </summary>
        /// <param name="assemblyName">Name of the <see cref="Assembly"/>.</param>
        /// <param name="typeName">Name of the exported plugin <see cref="Type"/>.</param>
        /// <param name="exportedType">The plugin exported <see cref="Type"/>.</param>
        /// <param name="metadata">Metadata of the exported plugin.</param>
        public RepositoryRecord(string assemblyName, string typeName, Type exportedType, Dictionary<string, object> metadata)
        {
            AssemblyName = assemblyName;
            TypeName = typeName;
            ExportedType = exportedType;
            MetaData = metadata;
        }
        /// <summary>
        /// Initialize class static fields.
        /// </summary>
        static RepositoryRecord()
        {
            s_MetaDataTypesCache = new Dictionary<Type, Type>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="System.Reflection.Assembly"/> name.
        /// </summary>
        public string AssemblyName { get; private set; }

        /// <summary>
        /// Gets the <see cref="Type"/> name of the exported type
        /// </summary>
        public string TypeName { get; private set; }

        /// <summary>
        /// Gets the plugin exported type.
        /// </summary>
        public Type ExportedType { get; private set; }

        /// <summary>
        /// Gets the plugin metadata.
        /// </summary>
        public Dictionary<string, object> MetaData { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Builds at runtime a dummy type that implement the metadata interface and exposes the exported plugin metadata.
        /// </summary>
        /// <param name="metadataType">The <see cref="Type"/> of the exported plugin metadata. Must be an interface.</param>
        /// <param name="metadata">The metadata of the exported plugin.</param>
        /// <returns></returns>
        private object BuildDummyMetaData(Type metadataType, Dictionary<string, object> metadata)
        {
            //ensure that the provided type is an interface
            if (!metadataType.IsInterface)
            {
                throw new ArgumentException("Provided type is not an interface", "metadataType");
            }
            Type dummyMetaDataType;
            //check for metadata type into the cache
            if (s_MetaDataTypesCache.ContainsKey(metadataType))
            {
                dummyMetaDataType = s_MetaDataTypesCache[metadataType];
            }
            else
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
                dummyMetaDataType = tb.CreateType();
                //add it to the cahce
                s_MetaDataTypesCache.Add(metadataType, dummyMetaDataType);
            }
            //assign metadata to dummy type instance
            object metadataObj = Activator.CreateInstance(dummyMetaDataType);
            foreach(KeyValuePair<string, object> metadataKVP in metadata)
            {
                PropertyInfo pi = dummyMetaDataType.GetProperty(metadataKVP.Key);
                pi?.SetValue(metadataObj, metadataKVP.Value);
            }
            //return metadata
            return metadataObj;
        }

        #endregion
    }
}
