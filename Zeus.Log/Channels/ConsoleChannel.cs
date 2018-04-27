using System;
using Zeus.Data;
using Zeus.Plugin;

namespace Zeus.Log.Channels
{
    /// <summary>
    /// Logs a message to the console.
    /// </summary>
    [ExportPlugin(typeof(ILogChannel))]
    [ExportPluginMetadata("Name", "ConsoleChannel")]
    public class ConsoleChannel : LogChannelBase
    {
        #region ILogChannel interface

        /// <summary>
        /// Writes a new message on the console.
        /// </summary>
        /// <param name="msg">The message that has to be processed.</param>
        public override void WriteMessage(LogMessage msg)
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
            Console.WriteLine(msg.ApplyFormat(MessageFormat ?? c_MsgFormat));
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
