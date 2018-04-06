using Zeus.Data;
using System;
using System.IO;
using System.Linq;
using Zeus.InternalLogger;

namespace Zeus.Config.Sources
{
    /// <summary>
    /// Gives access to a .odin configuration file.
    /// </summary>
    public sealed class FileSource : IConfigSource
    {
        #region Constants

        private const string c_FileNameTag = "FileName";
        private const string c_CreateIfNotExistsTag = "CreateIfNotExists";

        #endregion

        #region IConfigSource interface

        /// <summary>
        /// Initialize the configuration source.
        /// </summary>
        /// <param name="settings">Config source settings.</param>
        /// <param name="name">Config source name.</param>
        /// <returns>Returns true if the initialization succeeds, false otherwise.</returns>
        public bool Initialize(DataStore settings, string name)
        {
            Name = name;
            m_FileName = settings.TryGet<string>(c_FileNameTag, Path.ChangeExtension(Path.GetFileName(Environment.GetCommandLineArgs().First()), "zeus"));
            if (string.IsNullOrEmpty(m_FileName))
            {
                Logger.Log("File name is not valid.");
                m_FileName = null;
                return false;
            }
            if (settings.TryGet<bool>(c_CreateIfNotExistsTag, true) && !File.Exists(m_FileName))
            {
                File.Open(m_FileName, FileMode.OpenOrCreate).Close();
            }
            return true;
        }
        /// <summary>
        /// Open the config source.
        /// </summary>
        /// <returns>A <see cref="Stream"/> with the configuration data.</returns>
        public Stream Open()
        {
            try
            {
                m_DataStream = File.Open(m_FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                return m_DataStream;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Closes the configuration source.
        /// </summary>
        public bool Close()
        {
            try
            {
                m_DataStream.Close();
                m_DataStream.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return false;
            }
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
