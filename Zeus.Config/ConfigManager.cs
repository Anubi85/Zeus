using Zeus.Config.Sources;
using Zeus.Data;
using Zeus.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace Zeus.Config
{
    /// <summary>
    /// Manages the application coniguration.
    /// </summary>
    public static class ConfigManager
    {
        #region Constructor

        /// <summary>
        /// Allocate and initialize class resources.
        /// </summary>
        static ConfigManager()
        {
            s_CfgMutex = new Mutex(false);
            s_ReaderSettings = new XmlReaderSettings();
            s_ReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
            s_ReaderSettings.IgnoreWhitespace = true;
            s_WriterSettings = new XmlWriterSettings();
            s_WriterSettings.ConformanceLevel = ConformanceLevel.Fragment;
            s_WriterSettings.Indent = true;
            s_WriterSettings.OmitXmlDeclaration = true;
            s_Sources = new List<IConfigSource>();
            //add default local source
            DataStore settings = new DataStore();
            try
            {
                AddSource<FileSource>(settings, "Local zeus file");
            }
            catch (Exception ex)
            {
                throw new ZeusException(ErrorCodes.ConfigManagerInitFailed, ex.Message);
            }
        }

        #endregion

        #region Fields

        /// <summary>
        /// The mutex used to syncronize parallel access to the settings file.
        /// </summary>
        private static Mutex s_CfgMutex;
        /// <summary>
        /// Settings for the <see cref="XmlReader"/> objects.
        /// </summary>
        private static XmlReaderSettings s_ReaderSettings;
        /// <summary>
        /// Settings for the <see cref="XmlWriter"/> objects.
        /// </summary>
        private static XmlWriterSettings s_WriterSettings;
        /// <summary>
        /// List of the avaialble sources for configuration data.
        /// </summary>
        private static List<IConfigSource> s_Sources;

        #endregion

        #region Methods

        /// <summary>
        /// Gets the name of the section associated with the <typeparamref name="T"/> class.
        /// The section name is the class name, or the name defined by the <see cref="XmlRootAttribute"/>.
        /// </summary>
        /// <typeparam name="T">Type of the class for wich the section name must be retrieved.</typeparam>
        /// <returns>The name of the section associated with the requested class.</returns>
        private static string GetSectionName<T>()
        {
            Type SectionType = typeof(T);
            return SectionType.GetCustomAttribute<SectionNameAttribute>()?.ElementName ?? SectionType.Name;
        }
        /// <summary>
        /// Load requested data from configuration file.
        /// </summary>
        /// <typeparam name="T">The type of the data that has to be load.</typeparam>
        /// <returns>A new instance of the <typeparamref name="T"/> class populated with the data from the configuration file if succeed, null otherwise.</returns>
        public static T LoadSection<T>() where T : class
        {
            T data = null;
            s_CfgMutex.WaitOne();
            foreach (IConfigSource source in s_Sources)
            {
                Stream dataStream = source.Open();
                using (XmlReader xr = XmlReader.Create(dataStream, s_ReaderSettings))
                {
                    //move to root node
                    xr.MoveToContent();
                    //search for requested section
                    if (xr.IsStartElement(GetSectionName<T>()) || xr.ReadToFollowing(GetSectionName<T>()))
                    {
                        //check if section has been found
                        if (xr.NodeType != XmlNodeType.None)
                        {
                            //deserialize section
                            XmlSerializer xs = new XmlSerializer(typeof(T));
                            //get section data
                            data = (T)xs.Deserialize(xr.ReadSubtree());
                            //data found, stop searching
                            break;
                        }
                    }
                }
                source.Close();
            }
            s_CfgMutex.ReleaseMutex();
            return data;
        }
        /// <summary>
        /// Save given data to configuration file.
        /// </summary>
        /// <typeparam name="T">The type of the data that has to be saved.</typeparam>
        /// <param name="sectionData">The data that has to be saved in the configuration file.</param>
        public static void SaveSection<T>(T sectionData) where T : class
        {
            s_CfgMutex.WaitOne();
            XmlSerializer xs = new XmlSerializer(typeof(T));
            //use a memory stream to holt temporary output in order to be able to modify sections in the middle of the file.
            using (MemoryStream ms = new MemoryStream())
            {
                //save only to local source
                IConfigSource source = s_Sources.First();
                Stream dataStream = source.Open();
                using (XmlReader xr = XmlReader.Create(dataStream, s_ReaderSettings))
                {
                    using (XmlWriter xw = XmlWriter.Create(ms, s_WriterSettings))
                    {
                        //states need to be maually changed otherwise XmlSerializer try to write start document node
                        //that for Fragment xml throw an exception.
                        xw.FakeWrite();
                        //flag to indicate if the section already exists
                        bool dataWritten = false;
                        xr.MoveToContent();
                        //check if file is empty
                        if (xr.NodeType != XmlNodeType.None)
                        {
                            //loop over all available sections
                            while (xr.NodeType == XmlNodeType.Element)
                            {
                                if (xr.Name != GetSectionName<T>())
                                {
                                    //if not requested section copy it to output
                                    xw.WriteNode(xr.ReadSubtree(), false);
                                }
                                else
                                {
                                    xs.Serialize(xw, sectionData, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
                                    dataWritten = true;
                                }
                                //move to next section
                                xr.Skip();
                            }
                        }
                        if (!dataWritten)
                        {
                            //section has not been found, create it
                            xs.Serialize(xw, sectionData, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
                        }
                    }
                }
                //update the settings file with the data stored in the temporary memory stream
                dataStream.Seek(0, SeekOrigin.Begin);
                dataStream.SetLength(0);
                dataStream.Write(ms.GetBuffer(), 0, (int)ms.Length);
                source.Close();
            }
            s_CfgMutex.ReleaseMutex();
        }
        /// <summary>
        /// Add a new source of the specified type to the <see cref="ConfigManager"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the source that has to be added.</typeparam>
        /// <param name="settings">The new source settings.</param>
        /// <param name="name">The new source name.</param>
        public static void AddSource<T>(DataStore settings, string name) where T : IConfigSource
        {
            IConfigSource newSource = (IConfigSource)Activator.CreateInstance(typeof(T), true);
            newSource.Initialize(settings, name);
            s_Sources.Add(newSource);
        }

        #endregion
    }
}
