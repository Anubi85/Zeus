using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Plugin
{
    /// <summary>
    /// This attribute allows to <see cref="PluginLoader"/> to read metadata associated with a plugin.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class ExportPluginMetadataAttribute : Attribute
    {
        #region Constructor

        /// <summary>
        /// Store the metadata of the exported class.
        /// </summary>
        /// <param name="name">The name of the metadata.</param>
        /// <param name="value">The metadata value.</param>
        public ExportPluginMetadataAttribute(string name, object value)
        {
            Name = name;
            Value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the metadata name.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Gets the metadata value.
        /// </summary>
        public object Value { get; private set; }

        #endregion
    }
}
