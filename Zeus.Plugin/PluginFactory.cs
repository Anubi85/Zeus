using System;
using System.Linq;
using System.Reflection;

namespace Zeus.Plugin
{
    /// <summary>
    /// A factory that creates instance of the given type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The type of the objects instantieted by the factory.</typeparam>
    public sealed class PluginFactory<T> where T: class
    {
        #region Constructor

        /// <summary>
        /// Initialize class fields and load the assembly provided if needed.
        /// </summary>
        /// <param name="asmName">The name of the assembly that contains the requested type.</param>
        /// <param name="asmPath">The path of the assembly that contains the requested type.</param>
        /// <param name="typeName">The name of the type to load.</param>
        internal PluginFactory(string asmName, string asmPath, string typeName)
        {
            //check if an assembly with the same name has already been loaded
            Assembly asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == asmName) ?? Assembly.LoadFrom(asmPath);
            m_InstancesType = asm.GetType(typeName);
        }

        #endregion

        #region Fields

        /// <summary>
        /// The type of the instances created by this facotry.
        /// </summary>
        private Type m_InstancesType;

        #endregion

        #region Methods

        /// <summary>
        /// Create a new instance of the type assocated with the factory.
        /// </summary>
        /// <returns>The newly created instance.</returns>
        public T CreateInstance()
        {
            return (T)Activator.CreateInstance(m_InstancesType);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of the plugin.
        /// </summary>
        public Type PluginType { get { return m_InstancesType; } }

        #endregion
    }
}
