using System;

namespace Zeus.Log.Channels
{
    /// <summary>
    /// Logs a message to the console.
    /// </summary>
    public class ConsoleChannel : ILogChannel
    {
        #region ILogChannel interface

        /// <summary>
        /// Initializes the log channel.
        /// </summary>
        /// <param name="settings">The object that contains channel settings.</param>
        public void Initialize(CustomLogChannelSettings settings)
        {
        }

        /// <summary>
        /// Writes a new message on the console.
        /// </summary>
        /// <param name="msg">The message that has to be processed.</param>
        /// <param name="format">The message format string.</param>
        public void WriteMessage(LogMessage msg, string format)
        {
            ConsoleColor original = Console.ForegroundColor;
            switch (msg.Level)
            {
                case LogLevels.Trace:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogLevels.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case LogLevels.Info:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogLevels.Success:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogLevels.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevels.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevels.Fatal:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
            }
            Console.Write(string.Format("[{0}]:", msg.Level).PadRight(12));
            Console.ForegroundColor = original;
            Console.WriteLine(msg.ApplyFormat(format ?? c_MsgFormat));
        }

        #endregion

        #region IDisposable interface

        /// <summary>
        /// Releases unmanaged resources.
        /// </summary>
        public void Dispose()
        {

        }

        #endregion

        #region Constants

        /// <summary>
        /// Format string for messages logged by this channel.
        /// </summary>
        private const string c_MsgFormat = "{time:yyyy-MM-dd HH:mm:ss} {methodname}::{text}";

        #endregion
    }
}
