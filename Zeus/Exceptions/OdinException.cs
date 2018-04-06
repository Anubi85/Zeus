using System;
using System.Runtime.Serialization;
using System.Security;

namespace Zeus.Exceptions
{
    /// <summary>
    /// Generic exception class for Zeus framework.
    /// </summary>
    public class ZeusException : Exception
    {
        #region Properties

        /// <summary>
        /// The error code associated to the current instance.
        /// </summary>
        public int ErrorCode { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialize a new instance of the class with the given error code.
        /// </summary>
        public ZeusException() : this(null, ErrorCodes.Undefined, ErrorCodes.Undefined.Message)
        {
        }

        /// <summary>
        /// Initialize a new instance of the class with the given error code.
        /// </summary>
        /// <param name="code">The error code associated with the occurred error.</param>
        public ZeusException(ErrorCodes code) : this(null, code, code.Message)
        {
        }

        /// <summary>
        /// Initialize a new instance of the class with the given error code.
        /// </summary>
        /// <param name="code">The error code associated with the occurred error.</param>
        /// <param name="message">The message that describes the occurred error.</param>
        public ZeusException(ErrorCodes code, string message) : this(null, code, message)
        {
        }

        /// <summary>
        /// Initialize a new instance of the class with the specified message, inner exception and error code.
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="code">The code associated with the occurred error.</param>
        public ZeusException(Exception innerException, ErrorCodes code) : this(innerException, code, code.Message)
        {
        }

        /// <summary>
        /// Initialize a new instance of the class with the specified message, inner exception and error code.
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="code">The code associated with the occurred error.</param>
        /// <param name="message">The message that describes the occurred error.</param>
        public ZeusException(Exception innerException, ErrorCodes code, string message) : base(message, innerException)
        {
            ErrorCode = code;
        }

        /// <summary>
        /// Initialize a new instance of the class with the specified serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        [SecuritySafeCritical]
        protected ZeusException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

        #endregion
    }
}
