using System;

namespace Zeus.Plugin
{
    /// <summary>
    /// This attribute allows to the <see cref="PluginLoader"/> to recognize the class as a plugin.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class ExportPluginAttribute : Attribute
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
