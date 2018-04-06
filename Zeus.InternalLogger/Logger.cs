using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace Zeus.InternalLogger
{
    /// <summary>
    /// Log internal messages.
    /// </summary>
    internal static class Logger
    {
        #region Constructor

        /// <summary>
        /// Performs class initialization.
        /// </summary>
        static Logger()
        {
            s_Lock = new object();
            s_MsgQueue = new Queue<LogMessage>(c_MsgQueueSize);
            s_MsgAvaialble = new AutoResetEvent(false);
            s_Factory = new ChannelFactory<IWcfLogger>(IWcfLoggerUtility.Endpoint);
            Thread worker = new Thread(ProcessMessages);
            worker.IsBackground = true;
            worker.Start();
        }

        #endregion

        #region Constrants

        /// <summary>
        /// The size of the message queue.
        /// </summary>
        private const int c_MsgQueueSize = 128;

        #endregion

        #region Fields

        /// <summary>
        /// Lock object used to syncronize the access to the message queue.
        /// </summary>
        private static object s_Lock;
        /// <summary>
        /// The message queue.
        /// </summary>
        private static Queue<LogMessage> s_MsgQueue;
        /// <summary>
        /// Object used to notify the message avaialbility.
        /// </summary>
        private static AutoResetEvent s_MsgAvaialble;
        /// <summary>
        /// The factory object used to generate <see cref="IWcfLogger"/> channels.
        /// </summary>
        private static ChannelFactory<IWcfLogger> s_Factory;

        #endregion

        #region Methods

        /// <summary>
        /// Logs a message into the internal logger.
        /// </summary>
        /// <param name="msg">The message to log.</param>
        /// <param name="caller">The name of the method that generate the message.</param>
        public static void Log(string msg, [CallerMemberName] string caller = null)
        {
            LogMessage toLog = new LogMessage(caller, msg);
            lock(s_Lock)
            {
                if (s_MsgQueue.Count == c_MsgQueueSize)
                {
                    //queue is full, on message must be removed first
                    s_MsgQueue.Dequeue();
                }
                s_MsgQueue.Enqueue(toLog);
            }
            //notify that messages are available
            s_MsgAvaialble.Set();
        }
        /// <summary>
        /// Process the logged messages sending them to the server.
        /// </summary>
        private static void ProcessMessages()
        {
            HMACSHA256 hmacCalculator = null;
            //run indefinetly
            while(true)
            {
                //try to connect
                IWcfLogger channel = s_Factory.CreateChannel();
                TimeSpan timeout = new TimeSpan(0, 1, 0);
                while(channel.Cast<ICommunicationObject>().State != CommunicationState.Opened)
                {
                    try
                    {
                        channel.Cast<ICommunicationObject>().Open(timeout);
                        //perform handshake
                        BigInteger mod = IWcfLoggerUtility.GenerateRandomInt();
                        BigInteger prKey = IWcfLoggerUtility.GenerateRandomInt();
                        LogInReply reply = channel.LogIn(mod, IWcfLoggerUtility.GetPublicKey(2, mod, prKey), "admin");
                        if (reply.Status)
                        {
                            BigInteger sharedKey = IWcfLoggerUtility.GetSharedKey(mod, prKey, reply.PublicKey);
                            //check the password
                            SHA256 hasher = SHA256.Create();
                            byte[] tmp = hasher.ComputeHash(Encoding.UTF8.GetBytes("password").Concat(mod.ToByteArray()).ToArray());
                            tmp = hasher.ComputeHash(tmp.Concat(sharedKey.ToByteArray()).ToArray());
                            if (tmp.SequenceEqual(reply.PasswordToken.ToByteArray()))
                            {
                                hmacCalculator = new HMACSHA256(sharedKey.ToByteArray());
                                //exit the loop
                                break;
                            }
                        }
                        channel.Cast<ICommunicationObject>().Abort();
                    }
                    catch
                    {
                        //re-create the channel and try again
                        channel.Cast<ICommunicationObject>().Abort();
                        channel = s_Factory.CreateChannel();
                    }
                }
                //now channel is open, start sending messages
                while (channel.Cast<ICommunicationObject>().State == CommunicationState.Opened)
                {
                    List<LogMessage> toSend = new List<LogMessage>();
                    //check for messages
                    s_MsgAvaialble.WaitOne();
                    lock (s_Lock)
                    {
                        toSend = s_MsgQueue.ToList();
                        s_MsgQueue.Clear();
                    }
                    foreach(LogMessage msg in toSend)
                    {
                        //get byte array associated with the message
                        byte[] data = BitConverter.GetBytes(msg.Id)
                            .Concat(BitConverter.GetBytes(msg.Date.ToBinary()))
                            .Concat(Encoding.UTF8.GetBytes(msg.Caller))
                            .Concat(Encoding.UTF8.GetBytes(msg.Message))
                            .ToArray();
                        //compute HMAC and send message
                        channel.SendMessage(msg, new BigInteger(hmacCalculator.ComputeHash(data)));
                    }
                }
            }
        }

        #endregion
    }
}
