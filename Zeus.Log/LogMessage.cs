using System;
using System.Collections.Generic;

namespace Zeus.Log
{
    /// <summary>
    /// Contains the unformatted data of a log message.
    /// </summary>
    public sealed class LogMessage
    {
        #region Constructor

        /// <summary>
        /// Instantiate a new <see cref="LogMessage"/> object and initialize its properties.
        /// </summary>
        /// <param name="level">The type of the message.</param>
        /// <param name="methodName">The name of the method that originate the message.</param>
        /// <param name="processName">The name of the process that originate the message.</param>
        /// <param name="text">The text of the message.</param>
        public LogMessage(LogLevels level, string methodName, string processName, string text)
        {
            Time = DateTime.UtcNow;
            Level = level;
            MethodName = methodName;
            ProcessName = processName;
            Text = text;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the time at wich the <see cref="LogMessage"/> object has been created.
        /// </summary>
        public DateTime Time { get; private set; }

        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        public LogLevels Level { get; private set; }

        /// <summary>
        /// Gets the name of the method that generate the message.
        /// </summary>
        public string MethodName { get; private set; }

        /// <summary>
        /// Gets the name of the process that generate the message.
        /// </summary>
        public string ProcessName { get; private set; }

        /// <summary>
        /// Gets the message text.
        /// </summary>
        public string Text { get; private set; }

        #endregion

        #region Constants

        /// <summary>
        /// The dictionary that contains format keys mapping.
        /// </summary>
        private static readonly Dictionary<string, string> c_FormatKeys = new Dictionary<string, string>() {
            { "Date", "0" },
            { "Time", "0" },
            { "Level", "1" },
            { "ProcessName", "2" },
            { "MethodName", "3" },
            { "Text", "4" }
        };

        #endregion

        #region Methods

        /// <summary>
        /// Format the message with the given string format.
        /// </summary>
        /// <param name="format">String format for the message.</param>
        /// <returns>Message data formatted according wih given format string.</returns>
        public string ApplyFormat(string format)
        {
            //convert tags to indexes
            string parsedFormat = FormatParser.Parse(format, c_FormatKeys);
            try
            {
                return string.Format(parsedFormat, Time, Level, ProcessName, MethodName, Text);
            }
            catch (FormatException)
            {
                return "Bad format string sintax!";
            }
        }

        #endregion
    }
}
