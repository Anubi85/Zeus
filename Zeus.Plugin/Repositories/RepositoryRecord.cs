using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Plugin.Repositories
{
    /// <summary>
    /// Contains all the information relative to a repository record.
    /// </summary>
    sealed class RepositoryRecord
    {
        #region Constructor

        /// <summary>
        /// Initializes <see cref="RepositoryRecord"/> properties.
        /// </summary>
        /// <param name="assemblyName">Name of the <see cref="System.Reflection.Assembly"/>.</param>
        /// <param name="typeName">Name of the exported <see cref="Type"/>.</param>
        /// <param name="attribute"><see cref="ExportPluginAttribute"/> instance.</param>
        /// <param name="attributeType"><see cref="Type"/> of the <see cref="ExportPluginAttribute"/>. May be a type hat inherits from it.</param>
        /// <param name="exportedType">The <see cref="Type"/> with wich the class is exported.</param>
        public RepositoryRecord(string assemblyName, string typeName, ExportPluginAttribute attribute, Type attributeType, Type exportedType)
        {
            AssemblyName = assemblyName;
            TypeName = typeName;
            AttributeInstance = attribute;
            AttributeType = attributeType;
            ExportedType = exportedType;
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
        /// Gets the instance of <see cref="ExportPluginAttribute"/>.
        /// </summary>
        public ExportPluginAttribute AttributeInstance { get; private set; }

        /// <summary>
        /// Gets the real type of the <see cref="DiscoveryRecord.AttributeInstance"/>.
        /// </summary>
        public Type AttributeType { get; private set; }

        /// <summary>
        /// Gets <see cref="Type"/> with wich the class is exported.
        /// </summary>
        public Type ExportedType { get; private set; }

        #endregion
    }
}
