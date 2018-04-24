using System.Xml;
using Zeus.Data;

namespace Zeus.Config.Sources
{
    /// <summary>
    /// Interface that defines xml settings parser methods.
    /// </summary>
    internal interface IXmlSettingsParser
    {
        /// <summary>
        /// Parses the xml attributes of a config source node into a <see cref="DataStore"/>.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"> object from where read the data.</param>
        /// <returns>The <see cref="DataStore"/> object with the parsed data.</returns>
        DataStore ParseXmlData(XmlReader reader);
    }
}
