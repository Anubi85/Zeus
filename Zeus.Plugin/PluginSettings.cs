using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Zeus.Config;
using Zeus.Data;
using Zeus.Exceptions;
using Zeus.Plugin.Repositories;

namespace Zeus.Plugin
{
    /// <summary>
    /// Contains the settings of the application plugin loader.
    /// The name of the Xml configration file node associated with this class is "plugin".
    /// </summary>
    [SectionName("plugin")]
    public sealed class PluginSettings : IXmlSerializable
    {
        #region Fields

        /// <summary>
        /// List of objects that contains configuration information for plugin repositories.
        /// </summary>
        private List<KeyValuePair<string, DataStore>> m_RepositorySettings;
        /// <summary>
        /// A dictionary that associate avaialble repository types to its names.
        /// </summary>
        private static Dictionary<string, Type> s_AvaialbleRepositories;

        #endregion

        #region Construcotr

        /// <summary>
        /// Initializes internal class fields.
        /// </summary>
        public PluginSettings()
        {
            m_RepositorySettings = new List<KeyValuePair<string, DataStore>>();
        }
        /// <summary>
        /// Initialize class static fields.
        /// </summary>
        static PluginSettings()
        {
            Type repositoryType = typeof(RepositoryBase);
            s_AvaialbleRepositories = repositoryType.Assembly.GetTypes()
                .Where(t => !t.IsInterface && repositoryType.IsAssignableFrom(t))
                .ToDictionary(t => t.Name, t => t);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a list of objects that contains channels configuration information.
        /// </summary>
        public IList<KeyValuePair<string, DataStore>> RepositorySettings { get { return m_RepositorySettings; } }

        #endregion

        #region IXmlSerializable interface

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            //clear the repository settings list
            m_RepositorySettings.Clear();
            //read start element of the xml configuration
            reader.ReadStartElement();
            //read all the nodes until the end element node is reached
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                //check if the repository type is valid
                if (s_AvaialbleRepositories.ContainsKey(reader.Name))
                {
                    //prepare the item for the repository list
                    KeyValuePair<string, DataStore> repoInfo = new KeyValuePair<string, DataStore>(reader.Name, new DataStore());
                    while (reader.MoveToNextAttribute())
                    {
                        //read the attributes into the data store
                        repoInfo.Value.Create<string>(reader.Name, reader.Value);
                    }
                    //read the element
                    reader.MoveToElement();
                    string elemName = reader.Name;
                    reader.ReadStartElement();
                    if (reader.NodeType == XmlNodeType.EndElement && reader.Name == elemName)
                    {
                        reader.ReadEndElement();
                    }
                    //add new data into repository settings list
                    m_RepositorySettings.Add(repoInfo);
                }
                else
                {
                    throw new ZeusException(ErrorCodes.RepositoryClassNotFound, string.Format("Repository {0} class not found.", reader.Name));
                }
            }
            //read end element of the xml configuration
            reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            //loop over all configured repositories
            foreach (KeyValuePair<string, DataStore> chInfo in m_RepositorySettings)
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
