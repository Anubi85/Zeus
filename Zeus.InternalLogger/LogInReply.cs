using System.Numerics;
using System.Runtime.Serialization;

namespace Zeus.InternalLogger
{
    /// <summary>
    /// Contains the information about the result of a <see cref="IWcfLogger.LogIn(BigInteger, BigInteger, string)"/> operation.
    /// </summary>
    [DataContract]
    internal class LogInReply
    {
        #region Properties

        /// <summary>
        /// Gets the status of the operation. True if succeed, false otherwise.
        /// </summary>
        [DataMember]
        public bool Status { get; set; }
        /// <summary>
        /// If the operation succeed gets the server public key.
        /// </summary>
        [DataMember]
        public BigInteger PasswordToken { get; set; }
        /// <summary>
        /// If the operation succeed gets the password token computed by the server.
        /// </summary>
        [DataMember]
        public BigInteger PublicKey { get; set; }

        #endregion
    }
}
