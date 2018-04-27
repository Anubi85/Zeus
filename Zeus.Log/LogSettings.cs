using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Zeus.Config;
using Zeus.Data;
using Zeus.Log.Channels;
using Zeus.Plugin;
using Zeus.Plugin.Repositories;

namespace Zeus.Log
{
    /// <summary>
    /// Contains the settings of the application logger.
    /// The name of the Xml configration file node associated with this class is "log".
    /// </summary>
    [SectionName("log")]
    public sealed class LogSettings : IXmlSerializable
    {
        #region Fields

        /// <summary>
        /// List of objects that contains configuration information for log channels.
        /// </summary>
        private List<KeyValuePair<string, DataStore>> m_ChannelSettings;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes internal class fields.
        /// </summary>
        public LogSettings()
        {
            m_ChannelSettings = new List<KeyValuePair<string, DataStore>>();
            //configure the plugin loader
            PluginLoader.AddRepository(RepositoryTypes.Assembly, typeof(ILogChannel).Assembly.FullName);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a list of objects that contains channels configuration information.
        /// </summary>
        public IList<KeyValuePair<string, DataStore>> ChannelSettings { get { return m_ChannelSettings; } }

        #endregion

        #region IXmlSerializable interface

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            //clear the channels settings list
            m_ChannelSettings.Clear();
            //read start element of the xml configuration
            reader.ReadStartElement();
            //read all the nodes until the end element node is reached
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                //prepare the item for the channel list
                KeyValuePair<string, DataStore> chInfo = new KeyValuePair<string, DataStore>(reader.Name, new DataStore());
                while (reader.MoveToNextAttribute())
                {
                    //read the attributes into the data store
                    chInfo.Value.Create<string>(reader.Name, reader.Value);
                }
                //read the element
                reader.MoveToElement();
                string elemName = reader.Name;
                reader.ReadStartElement();
                if (reader.NodeType == XmlNodeType.EndElement && reader.Name == elemName)
                {
                    reader.ReadEndElement();
                }
                //add new data into channel settings list
                m_ChannelSettings.Add(chInfo);
            }
            //read end element of the xml configuration
            reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            //loop over all configured channels
            foreach (KeyValuePair<string, DataStore> chInfo in m_ChannelSettings)
            {
                writer.WriteStartElement(chInfo.Key);
                foreach (string key in chInfo.Value.GetTags())
                {
                    writer.WriteStartAttribute(key);
                    writer.WriteValue(chInfo.Value.Get<string>(key));
                    writer.WriteEndAttribute();
                }
                writer.WriteEndElement();
            }
        }

        #endregion
    }
}
