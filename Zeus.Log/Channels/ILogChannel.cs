using System;

namespace Zeus.Log.Channels
{
    /// <summary>
    /// Represent a generic log channel.
    /// </summary>
    public interface ILogChannel : IDisposable
    {
        /// <summary>
        /// Initializes the log channel.
        /// </summary>
        /// <param name="settings">The object that contains channel settings.</param>
        void Initialize(CustomLogChannelSettings settings);

        /// <summary>
        /// Writes a new message on the log channel.
        /// </summary>
        /// <param name="msg">The message that has to be processed.</param>
        /// /// <param name="format">The message format string.</param>
        void WriteMessage(LogMessage msg, string format);
    }
}

