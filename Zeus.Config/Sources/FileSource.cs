using Zeus.Data;
using System;
using System.IO;
using System.Linq;
using Zeus.Exceptions;

namespace Zeus.Config.Sources
{
    /// <summary>
    /// Gives access to a .odin configuration file.
    /// </summary>
    public sealed class FileSource : IConfigSource
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

        #endregion

        #region IConfigSource interface

        /// <summary>
        /// Initialize the configuration source.
        /// </summary>
        /// <param name="settings">Config source settings.</param>
        /// <param name="name">Config source name.</param>
        public void Initialize(DataStore settings, string name)
        {
            Name = name;
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
        /// Gets the name of the confg source.
        /// </summary>
        public string Name { get; private set; }

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
