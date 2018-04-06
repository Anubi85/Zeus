﻿using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Zeus.Config;

namespace Zeus.Log
{
    /// <summary>
    /// Containsthe settings of the application logger.
    /// The name of the Xml configration file node associated with this class is "message".
    /// </summary>
    [SectionName("logs")]
    public class LogSettings : IXmlSerializable
    {
        #region Constants

        /// <summary>
        /// The name of the attribute that contains information about the custom channels repository path.
        /// </summary>
        private const string c_ChannelsRepositoryAttributeName = "ChannelsRepository";

        #endregion

        #region Fields

        /// <summary>
        /// List of <see cref="ChannelSettings"/> objects that contains configuration information for log channels.
        /// </summary>
        private List<LogChannelSettings> m_ChannelSettings;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes internal class fields.
        /// </summary>
        public LogSettings()
        {
            m_ChannelSettings = new List<LogChannelSettings>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a list of <see cref="ChannelSettings"/> objects that contains channels configuration information.
        /// </summary>
        public List<LogChannelSettings> ChannelSettings { get { return m_ChannelSettings; } }

        /// <summary>
        /// Gets or sets the channels modules repository path.
        /// Used to load custom log channels through the plugin system.
        /// </summary>
        public string ChannelsRepository { get; set; }

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
            //read repository path if present
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case c_ChannelsRepositoryAttributeName:
                        ChannelsRepository = reader.Value;
                        break;
                    default:
                        break;
                }
                reader.MoveToElement();
            }
            //read start element of the xml configuration
            reader.ReadStartElement();
            //create an instance of XmlSerializer class used to deseialize channel settings
            XmlSerializer xs = new XmlSerializer(typeof(LogChannelSettings));
            //read all the nodes until the end element node is reached
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                //deserialize channel settings
                m_ChannelSettings.Add((LogChannelSettings)xs.Deserialize(reader));
            }
            //read end element of the xml configuration
            reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            //write channel repository path if present
            if (!string.IsNullOrEmpty(ChannelsRepository))
            {
                writer.WriteStartAttribute(c_ChannelsRepositoryAttributeName);
                writer.WriteValue(ChannelsRepository);
                writer.WriteEndAttribute();
            }
            //create an instance of XmlXerializer class
            XmlSerializer xs = new XmlSerializer(typeof(LogChannelSettings));
            //loop over all channels settings
            foreach (LogChannelSettings cs in m_ChannelSettings)
            {
                //serialize all channel settings
                xs.Serialize(writer, cs);
            }
        }

        #endregion
    }
}
