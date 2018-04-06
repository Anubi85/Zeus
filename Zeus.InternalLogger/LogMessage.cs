using System;
using System.Runtime.Serialization;

namespace Zeus.InternalLogger
{
    /// <summary>
    /// Stores he information of a log message.
    /// </summary>
    [DataContract]
    internal class LogMessage
    {
        #region Properties

        /// <summary>
        /// Gets the message id.
        /// </summary>
        [DataMember]
        public ulong Id { get; private set; }
        /// <summary>
        /// Gets the date of the log message.
        /// </summary>
        [DataMember]
        public DateTime Date { get; private set; }
        /// <summary>
        /// Gets the name of the method that generates the log message.
        /// </summary>
        [DataMember]
        public string Caller { get; private set; }
        /// <summary>
        /// Gets the log message.
        /// </summary>
        [DataMember]
        public string Message { get; private set; }

        #endregion

        #region Fields

        /// <summary>
        /// A counter for generated message.
        /// </summary>
        private static ulong s_MsgCounter = 0;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new <see cref="LogMessage"/>.
        /// </summary>
        /// <param name="methodName">The name of the method that generates the message.</param>
        /// <param name="msg">The message text.</param>        
        public LogMessage(string methodName, string msg)
        {
            Id = ++s_MsgCounter;
            //ignore milliseconds fraction
            DateTime tmp = DateTime.UtcNow;
            Date = new DateTime(
                tmp.Year,
                tmp.Month,
                tmp.Day,
                tmp.Hour,
                tmp.Minute,
                tmp.Second,
                tmp.Millisecond,
                DateTimeKind.Utc);
            Caller = methodName;
            Message = msg;
        }

        #endregion
    }
}
