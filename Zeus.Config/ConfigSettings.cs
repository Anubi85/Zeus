using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    class ConfigSettings : IXmlSerializable
    {
        #region Fields

        /// <summary>
        /// The list of the avaiavle sources data.
        /// </summary>
        private List<Tuple<Type, DataStore>> m_Sources;
        /// <summary>
        /// A disctionary that associate avaialble source types to its names.
        /// </summary>
        private static Dictionary<string, Type> s_AvaialbleSources;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize class fields.
        /// </summary>
        public ConfigSettings()
        {
            m_Sources = new List<Tuple<Type, DataStore>>();
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
        public List<Tuple<Type, DataStore>> Sources { get { return m_Sources; } }

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
                    IXmlSettingsParser parser = Activator.CreateInstance(s_AvaialbleSources[reader.Name], true) as IXmlSettingsParser;
                    if (parser != null)
                    {
                        m_Sources.Add(new Tuple<Type, DataStore> (s_AvaialbleSources[reader.Name], parser.ParseXmlData(reader)));
                    }
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
