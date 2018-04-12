using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Zeus.Log.Channels
{
    /// <summary>
    /// Logs message to file.
    /// </summary>
    [AllowMultipleInstances]
    public class FileChannel : ILogChannel
    {
        #region ILogChannel interface

        /// <summary>
        /// Initializes the log channel.
        /// </summary>
        /// <param name="settings">The object that contains channel settings.</param>
        public void Initialize(CustomLogChannelSettings settings)
        {
            //loop over all defined keys
            foreach (string key in settings.GetKeys().OrderBy(k => c_KeyPriority[k]))
            {
                switch (key)
                {
                    case c_FileNameKey:
                        m_FileNameTemplate = settings.GetValue<string>(c_FileNameKey);
                        break;
                    case c_MaxFileSizeKey:
                        string maxFileSize = settings.GetValue<string>(c_MaxFileSizeKey);
                        switch (maxFileSize.Last())
                        {
                            case 'm':
                            case 'M':
                                m_MaxFileSize = int.Parse(maxFileSize.Substring(0, maxFileSize.Length - 1)) * (int)1E6;
                                break;
                            case 'k':
                            case 'K':
                                m_MaxFileSize = int.Parse(maxFileSize.Substring(0, maxFileSize.Length - 1)) * (int)1E3;
                                break;
                            case 'g':
                            case 'G':
                                m_MaxFileSize = int.Parse(maxFileSize.Substring(0, maxFileSize.Length - 1)) * (int)1E9;
                                break;
                            case 'l':
                            case 'L':
                                m_MaxFileSize = -int.Parse(maxFileSize.Substring(0, maxFileSize.Length - 1)); //negative numbers mean line numbers, not file size.
                                break;
                            default:
                                if (char.IsDigit(maxFileSize.Last()))
                                {
                                    m_MaxFileSize = int.Parse(maxFileSize);
                                }
                                break;
                        }
                        break;
                    case c_CreateAnywayKey:
                        if (settings.GetValue<bool>(c_CreateAnywayKey))
                        {
                            m_LogFile = GetLogFile();
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Writes a new message on the console.
        /// </summary>
        /// <param name="msg">The message that has to be processed.</param>
        /// <param name="format">The message format string.</param>
        public void WriteMessage(LogMessage msg, string format)
        {
            //check for file existance
            if (m_LogFile == null)
            {
                //create the file
                m_LogFile = GetLogFile();
            }
            //log to file
            m_LogFile.WriteLine(msg.ApplyFormat(format ?? c_MsgFormat));
            //update message counter
            m_MsgCounter++;
            if (m_MaxFileSize != 0)
            {
                if (((m_MaxFileSize > 0) && (m_MaxFileSize < m_LogFile.BaseStream.Length)) || ((m_MaxFileSize < 0) && (m_MaxFileSize + m_MsgCounter == 0)))
                {
                    CloseLogFile();
                }
            }
        }

        #endregion

        #region IDisposable interface

        /// <summary>
        /// Releases unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            CloseLogFile();
        }

        #endregion

        #region Constants

        /// <summary>
        /// Format string for messages logged by this channel.
        /// </summary>
        private const string c_MsgFormat = "{date:yyyy-MM-dd};{time:HH:mm:ss};{level};{processname};{methodname};{text}";
        /// <summary>
        /// The settings key for the Create Anyway option.
        /// </summary>
        private const string c_CreateAnywayKey = "CreateAnyway";
        /// <summary>
        /// The settings key for the file name.
        /// </summary>
        private const string c_FileNameKey = "FileName";
        /// <summary>
        /// The settings key for the maximum file size.
        /// </summary>
        private const string c_MaxFileSizeKey = "MaxFileSize";
        /// <summary>
        /// The dictionary that contains format keys mapping.
        /// </summary>
        private static readonly Dictionary<string, string> c_FormatKeys = new Dictionary<string, string>() {
            { "Date", "0" },
            { "Time", "0" },
            { "Id", "1" }
        };
        /// <summary>
        /// The dictionary that maps each possible settings key to its parsing priority.
        /// </summary>
        private static readonly Dictionary<string, int> c_KeyPriority = new Dictionary<string, int>() {
            { c_FileNameKey, 0 },
            { c_MaxFileSizeKey, 1 },
            { c_CreateAnywayKey, 2 }
        };


        #endregion

        #region Fields

        /// <summary>
        /// The log file stream.
        /// </summary>
        private StreamWriter m_LogFile;
        /// <summary>
        /// The log file name template.
        /// </summary>
        private string m_FileNameTemplate;
        /// <summary>
        /// A counter that keep trace about how much log files has been created.
        /// </summary>
        private int m_FileCounter;
        /// <summary>
        /// A counter that keep trace about how much message has been logged.
        /// </summary>
        private int m_MsgCounter;
        /// <summary>
        /// The maximum log file size in bytes.
        /// </summary>
        private int m_MaxFileSize;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize class internal fields.
        /// </summary>
        public FileChannel()
        {
            m_FileNameTemplate = "Log_{id}.csv";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the log file writer.
        /// </summary>
        /// <returns>The <see cref="StreamWriter"/> that allows to wite logs to file.</returns>
        private StreamWriter GetLogFile()
        {
            FileStream stream = null;
            bool stop = false;
            while (!stop)
            {
                string parsedFormat = FormatParser.Parse(m_FileNameTemplate, c_FormatKeys);
                string fileName = string.Format(parsedFormat, DateTime.UtcNow, m_FileCounter++);
                stream?.Flush();
                stream?.Close();
                stream?.Dispose();
                stream = File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.Read);
                //check if a restriction about file size exists
                if (m_MaxFileSize != 0)
                {
                    if (m_MaxFileSize > 0)
                    {
                        stop = stream.Length < m_MaxFileSize;
                    }
                    else
                    {
                        m_MsgCounter = 0;
                        using (StreamReader reader = new StreamReader(File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                        {
                            while (reader.ReadLine() != null)
                            {
                                m_MsgCounter++;
                            }
                        }
                        stop = m_MsgCounter + m_MaxFileSize < 0; //m_MaxFileSize is negative!
                    }
                }
                else
                {
                    stop = true;
                }
            }
            return new StreamWriter(stream) { AutoFlush = true };
        }

        /// <summary>
        /// Closes the log file.
        /// </summary>
        private void CloseLogFile()
        {
            m_LogFile?.Flush();
            m_LogFile?.Close();
            m_LogFile?.Dispose();
            m_LogFile = null;
            m_MsgCounter = 0;
        }

        #endregion
    }
}
