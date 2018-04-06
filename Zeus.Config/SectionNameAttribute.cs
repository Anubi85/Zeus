using System;
using System.Xml.Serialization;

namespace Zeus.Config
{
    /// <summary>
    /// Attribute that define the config file section name.
    /// Can only be used on classes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SectionNameAttribute : XmlRootAttribute
    {
        #region Constructor

        /// <summary>
        /// Create  new instance of the attribute.
        /// </summary>
        /// <param name="name">The name of the config section that contains data associated with the class in wich this attribute is used.</param>
        public SectionNameAttribute(string name)
        {
            ElementName = name;
        }

        #endregion
    }
}
