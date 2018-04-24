using Zeus.Data;
using System;
using System.IO;
using System.Linq;
using Zeus.Exceptions;
using System.Xml;

namespace Zeus.Config.Sources
{
    /// <summary>
    /// Gives access to a .odin configuration file.
    /// </summary>
    public sealed class FileSource : IConfigSource, IXmlSettingsParser
    {
        #region Constants

        /// <summary>
        /// The <see cref="DataStore"/> tag that contains information about the file name.
        /// </summary>
        private const string c_FileNameTag = "FileName";
        /// <summary>
        /// The <see cref="DataStore"/> tag that contains information about file creation option.
        /// </summary>
        private const string c_CreateIfNotExistsTag = "CreateIfNotExists";
        /// <summary>
        /// The <see cref="DataStore"/> tag that contains information about config source readonly flag.
        /// </summary>
        private const string c_IsReadOnlyTag = "ReadOnly";

        #endregion

        #region IConfigSource interface

        /// <summary>
        /// Initialize the configuration source.
        /// </summary>
        /// <param name="settings">Config source settings.</param>
        public void Initialize(DataStore settings)
        {
            m_FileName = settings.TryGet<string>(c_FileNameTag, Path.ChangeExtension(Path.GetFileName(Environment.GetCommandLineArgs().First()), "zeus"));
            if (string.IsNullOrEmpty(m_FileName))
            {
                m_FileName = null;
                throw new ZeusException(ErrorCodes.InvalidSettings, "Invalid file name settings");
            }
            if (settings.TryGet<bool>(c_CreateIfNotExistsTag, true) && !File.Exists(m_FileName))
            {
                File.Open(m_FileName, FileMode.OpenOrCreate).Close();
            }
            IsReadOnly = settings.TryGet<bool>(c_IsReadOnlyTag, true);
        }
        /// <summary>
        /// Open the config source.
        /// </summary>
        /// <returns>A <see cref="Stream"/> with the configuration data.</returns>
        public Stream Open()
        {
            m_DataStream = File.Open(m_FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            return m_DataStream;
        }
        /// <summary>
        /// Closes the configuration source.
        /// </summary>
        public void Close()
        {
            m_DataStream?.Close();
            m_DataStream?.Dispose();
        }
        /// <summary>
        /// Gets a flag that indicates if the source is readonly or not.
        /// </summary>
        public bool IsReadOnly { get; private set; }

        #endregion

        #region IXmlSettingsParser interface

        /// <summary>
        /// Parses the xml attributes of a config source node into a <see cref="DataStore"/>.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"> object from where read the data.</param>
        /// <returns>The <see cref="DataStore"/> object with the parsed data.</returns>
        DataStore IXmlSettingsParser.ParseXmlData(XmlReader reader)
        {
            DataStore res = new DataStore();
            while(reader.MoveToNextAttribute())
            {
                switch(reader.Name)
                {
                    case c_FileNameTag:
                        res.Create<string>(c_FileNameTag, reader.Value);
                        break;
                    case c_IsReadOnlyTag:
                        res.Create<bool>(c_IsReadOnlyTag, bool.Parse(reader.Value));
                        break;
                    case c_CreateIfNotExistsTag:
                        res.Create<bool>(c_CreateIfNotExistsTag, bool.Parse(reader.Value));
                        break;
                }
            }
            //move back to start element and read it
            reader.MoveToElement();
            reader.ReadStartElement();            
            return res;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Internal constructor used to avoid creation of class instance outside the current assembly.
        /// The constructor do nothing, all the action are performed in the <see cref="FileSource.Initialize(DataStore, string)"/> method.
        /// </summary>
        internal FileSource()
        {

        }

        #endregion

        #region Fields

        /// <summary>
        /// Name of the settings file.
        /// </summary>
        private string m_FileName;
        /// <summary>
        /// <see cref="Stream"/> linked to the configuration file.
        /// </summary>
        private Stream m_DataStream;

        #endregion
    }
}
