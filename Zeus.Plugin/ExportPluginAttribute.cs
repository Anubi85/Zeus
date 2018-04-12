using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Plugin
{
    /// <summary>
    /// This attribute allows to class marked with it to be recognized by the <see cref="PluginLoader"/> class.
    /// This attribute does not allow multiple definition and can be applied only to clases and interfaces.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
    public class ExportPluginAttribute : Attribute
    {
        #region Constructor

        /// <summary>
        /// Store the type of the exported class.
        /// </summary>
        /// <param name="pluginType">Type of the exported plugin.</param>
        public ExportPluginAttribute(Type pluginType)
        {
            PluginType = pluginType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of the exported class.
        /// </summary>
        public Type PluginType { get; private set; }

        #endregion
    }
}
