using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Zeus.Config.Sources;
using Zeus.Data;

namespace Zeus.Config
{
    /// <summary>
    /// Contains the settings for the configuration manager.
    /// </summary>
    internal class ConfigSettings : IXmlSerializable
    {
        #region Fields

        /// <summary>
        /// The list of the avaiavle sources data.
        /// </summary>
        private List<KeyValuePair<Type, DataStore>> m_Sources;
        /// <summary>
        /// A dictionary that associate avaialble source types to its names.
        /// </summary>
        private static Dictionary<string, Type> s_AvaialbleSources;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize class fields.
        /// </summary>
        public ConfigSettings()
        {
            m_Sources = new List<KeyValuePair<Type, DataStore>>();
        }
        /// <summary>
        /// Initialize class static fields.
        /// </summary>
        static ConfigSettings()
        {
            Type configSourceType = typeof(IConfigSource);
            s_AvaialbleSources = configSourceType.Assembly.GetTypes()
                .Where(t => !t.IsInterface && configSourceType.IsAssignableFrom(t))
                .ToDictionary(t => t.Name, t => t);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the configured sources data.
        /// </summary>
        public List<KeyValuePair<Type, DataStore>> Sources { get { return m_Sources; } }

        #endregion

        #region IXmlSerializable interface

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            //clear the sources list
            m_Sources.Clear();
            //read start element of the xml configuration
            reader.ReadStartElement();
            //read all the nodes until the end element node is reached
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                //deserialize channel settings if channel type exists
                if (s_AvaialbleSources.ContainsKey(reader.Name))
                {
                    KeyValuePair<Type, DataStore> srcInfo = new KeyValuePair<Type, DataStore>(s_AvaialbleSources[reader.Name], new DataStore());
                    while(reader.MoveToNextAttribute())
                    {
                        srcInfo.Value.Create<string>(reader.Name, reader.Value);
                    }
                    reader.MoveToElement();
                    string elemName = reader.Name;
                    reader.ReadStartElement();
                    if (reader.NodeType == XmlNodeType.EndElement && reader.Name == elemName)
                    {
                        reader.ReadEndElement();
                    }
                    m_Sources.Add(srcInfo);
                }
            }
            //read end element of the xml configuration
            reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
        }

        #endregion
    }
}
