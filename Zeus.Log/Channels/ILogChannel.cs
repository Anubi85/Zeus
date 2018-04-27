using System;
using Zeus.Data;

namespace Zeus.Log.Channels
{
    /// <summary>
    /// Represent a generic log channel.
    /// </summary>
    public interface ILogChannel : IDisposable
    {
        #region Methods

        /// <summary>
        /// Initializes the log channel.
        /// </summary>
        /// <param name="settings">The object that contains channel settings.</param>
        void Initialize(DataStore settings);

        /// <summary>
        /// Writes a new message on the log channel.
        /// </summary>
        /// <param name="msg">The message that has to be processed.</param>
        void WriteMessage(LogMessage msg);

        #endregion

        #region Properties

        /// <summary>
        /// Gets the minimum log level for the log channel.
        /// </summary>
        LogLevels MinimumLogLevel { get; }

        /// <summary>
        /// Gets the maximum log level for the log channel.
        /// </summary>
        LogLevels MaximumLogLevel { get; }

        #endregion
    }
}

