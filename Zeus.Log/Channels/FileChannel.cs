using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Zeus.Exceptions;

namespace Zeus.Log.Channels
{
    /// <summary>
    /// Logs message to file.
    /// </summary>
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
            foreach (string key in settings.GetKeys())
            {
                switch(key)
                {
                    case c_FileNameKey:
                        m_FileNameTemplate = settings.GetValue<string>(c_FileNameKey);
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
        }

        #endregion

        #region IDisposable interface

        /// <summary>
        /// Releases unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            m_LogFile?.Flush();
            m_LogFile?.Close();
            m_LogFile?.Dispose();
        }

        #endregion

        #region Constants

        /// <summary>
        /// Match pattern used by <see cref="Regex.Replace(string, string, string)"/> to format the file name.
        /// </summary>
        private const string c_MatchPattern = "(?<={{)(?i){0}(?-i)";
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
            string parsedFormat = m_FileNameTemplate;
            parsedFormat = Regex.Replace(parsedFormat, string.Format(c_MatchPattern, "Date"), "0");
            parsedFormat = Regex.Replace(parsedFormat, string.Format(c_MatchPattern, "Time"), "0");
            parsedFormat = Regex.Replace(parsedFormat, string.Format(c_MatchPattern, "Id"), "1");
            string fileName = string.Format(parsedFormat, DateTime.UtcNow, m_FileCounter++);
            return new StreamWriter(File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.Read)) { AutoFlush = true };
        }

        #endregion
    }
}
