using System.Numerics;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Zeus.InternalLogger
{
    /// <summary>
    /// Contract for the internal logging service.
    /// </summary>
    [ServiceContract]
    internal interface IWcfLogger
    {
        /// <summary>
        /// Try to login into the server.
        /// </summary>
        /// <param name="salt">The number used as module for the generation of the serer public key.</param>
        /// <param name="pbKey">The client public key.</param>
        /// <param name="user">The user to wich login is requested.</param>
        /// <returns>Returns a <see cref="LogInReply"/> object that contains informations about the operation results.</returns>
        [OperationContract]
        [WebInvoke(
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "LogIn"
            )]
        LogInReply LogIn(BigInteger salt, BigInteger pbKey, string user);
        /// <summary>
        /// Sends the specified message to the WCF service.
        /// </summary>
        /// <param name="msg">The message to be send.</param>
        /// <param name="hmac">The HMAC signature of the message.</param>
        [OperationContract]
        [WebInvoke(
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "SendMessage"
            )]
        void SendMessage(LogMessage msg, BigInteger hmac);
    }
}
