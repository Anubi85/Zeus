using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using Zeus.Config;
using Zeus.InternalLogger;
using Zeus.Log.Channels;

namespace Zeus.Log
{
    /// <summary>
    /// Manages the message logging of the application.
    /// </summary>
    public static class LogManager
    {
        #region Helper class

        /// <summary>
        /// This class represent a log channel and the information related to it.
        /// </summary>
        private class LogChannelRecord
        {
            #region Fields

            /// <summary>
            /// Conunts how many <see cref="LogChannelRecord"/> instances has been created.
            /// </summary>
            private static byte s_InstanceCounter = 0;

            /// <summary>
            /// The minimum log level that has to be processed by this channel.
            /// </summary>
            private LogLevels m_MinLogLevel;
            /// <summary>
            /// The maximum log level that has to be processed by this channel.
            /// </summary>
            private LogLevels m_MaxLogLevel;

            #endregion

            #region Constructor

            /// <summary>
            /// Initialize a new instance of <see cref="LogChannelRecord"/>.
            /// </summary>
            /// <param name="settings">The settings object that contains information about the channel that has to be initialized.</param>
            /// <param name="channel">The log channel object.</param>
            public LogChannelRecord(LogChannelSettings settings, ILogChannel channel)
            {
                s_InstanceCounter++;
                m_MinLogLevel = settings.MinimumLogLevel;
                m_MaxLogLevel = settings.MaximumLogLevel;                
                if (string.IsNullOrEmpty(settings.ChannelName))
                {
                    Name = string.Format("Channel {0}", s_InstanceCounter);
                }
                else
                {
                    Name = settings.ChannelName;
                }
                MessageFormat = settings.MessageFormat;
                Channel = channel;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets the channel name.
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// Gets the message format for the current channel.
            /// </summary>
            public string MessageFormat { get; private set; }

            /// <summary>
            /// Gets the cahnnel instance.
            /// </summary>
            public ILogChannel Channel { get; private set; }

            #endregion

            #region Methods

            /// <summary>
            /// Check if the channel can process the incoming message.
            /// </summary>
            /// <param name="level">The <see cref="LogLevels"/> of the incoming message.</param>
            /// <returns>True if the incoming message level is in the range that the channel should process, false otherwise.</returns>
            public bool CanProcess(LogLevels level)
            {
                return (m_MinLogLevel <= level) && (level <= m_MaxLogLevel);
            }

            #endregion
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Instantiate a new instance and allocate and initialize class resources.
        /// </summary>
        static LogManager()
        {
            CompleteMessageProcessing = true;
            s_LogChannels = new ConcurrentQueue<LogChannelRecord>();
            s_MessageQuque = new BlockingCollection<LogMessage>();
            s_ProcessName = Process.GetCurrentProcess().ProcessName;
            s_AddChannelMethod = typeof(LogManager).GetMethod("AddChannel", BindingFlags.Public | BindingFlags.Static);
            AppDomain.CurrentDomain.ProcessExit += OnExit;
            LogSettings settings = ConfigManager.LoadSection<LogSettings>();
            //configure the log channels
            if (settings != null)
            {
                //get avaialble log channels (local assembly)
                Type logChannelInterface = typeof(ILogChannel);
                Dictionary<string, Type> availableLogChannels = logChannelInterface.Assembly.GetTypes()
                    .Where(t => !t.IsInterface && logChannelInterface.IsAssignableFrom(t))
                    .ToDictionary(t => t.Name, t => t);
                foreach (LogChannelSettings chSetings in settings.ChannelSettings)
                {
                    //add a new channel
                    AddChannel(availableLogChannels[chSetings.ChannelType], chSetings);
                }
            }
            else
            {
                Logger.Log("No logger sonfiguration found");
            }
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Method called when the application exits. Clean up resources.
        /// </summary>
        /// <param name="sender">The object that generate the event.</param>
        /// <param name="e">The arguments that contains information about the event.</param>
        private static void OnExit(object sender, EventArgs e)
        {
            foreach (LogChannelRecord channelRecord in s_LogChannels)
            {
                channelRecord.Channel.Dispose();
            }
            s_MessageQuque?.Dispose();
        }

        #endregion

        #region Fields

        /// <summary>
        /// The list of channels used by the log manager to log messages.
        /// </summary>
        private static ConcurrentQueue<LogChannelRecord> s_LogChannels;
        /// <summary>
        /// The queue that contains the message to be processed by each configured channel.
        /// </summary>
        private static BlockingCollection<LogMessage> s_MessageQuque;
        /// <summary>
        /// The cancellation token used to stop worker thread.
        /// </summary>
        private static CancellationTokenSource s_Cancel;
        /// <summary>
        /// The <see cref="Thread"/> that actually process the log messages.
        /// </summary>
        private static Thread s_WorkerThread;
        /// <summary>
        /// The name of the process that instantiate the class.
        /// </summary>
        private static string s_ProcessName;
        /// <summary>
        /// The <see cref="LogManager.AddChannel{T}"/> method information retrieved throug reflection.
        /// </summary>
        private static MethodInfo s_AddChannelMethod;

        #endregion

        #region Constants

        /// <summary>
        /// The default value of the methodName parameters used in the Write*TYPE*Message methods.
        /// </summary>
        private const string c_DefaultMethodName = "No Data";

        #endregion

        #region Methods

        /// <summary>
        /// Apend a new message to the message queue.
        /// </summary>
        /// <param name="level">The type of the message.</param>
        /// <param name="methodName">The name of the method that generate the message.</param>
        /// <param name="text">The message text.</param>        
        private static void WriteMessage(LogLevels level, string methodName, string text)
        {
            s_MessageQuque.Add(new LogMessage(level, methodName, s_ProcessName, text));
        }

        /// <summary>
        /// Append a trace message to the message queue.
        /// </summary>
        /// <param name="text">The message text.</param>
        /// <param name="methodName">The name of the method that generate the message (automatically retrieved).</param>
        public static void WriteTraceMessage(string text, [CallerMemberName] string methodName = c_DefaultMethodName)
        {
            WriteMessage(LogLevels.Trace, methodName, text);
        }

        /// <summary>
        /// Append a debug message to the message queue.
        /// </summary>
        /// <param name="text">The message text.</param>
        /// <param name="methodName">The name of the method that generate the message (automatically retrieved).</param>
        public static void WriteDebugMessage(string text, [CallerMemberName] string methodName = c_DefaultMethodName)
        {
            WriteMessage(LogLevels.Debug, methodName, text);
        }

        /// <summary>
        /// Append an info message to the message queue.
        /// </summary>
        /// <param name="text">The message text.</param>
        /// <param name="methodName">The name of the method that generate the message (automatically retrieved).</param>
        public static void WriteInfoMessage(string text, [CallerMemberName] string methodName = c_DefaultMethodName)
        {
            WriteMessage(LogLevels.Info, methodName, text);
        }

        /// <summary>
        /// Append a success message to the message queue.
        /// </summary>
        /// <param name="text">The message text.</param>
        /// <param name="methodName">The name of the method that generate the message (automatically retrieved).</param>
        public static void WriteSuccessMessage(string text, [CallerMemberName] string methodName = c_DefaultMethodName)
        {
            WriteMessage(LogLevels.Success, methodName, text);
        }

        /// <summary>
        /// Append a warning message to the message queue.
        /// </summary>
        /// <param name="text">The message text.</param>
        /// <param name="methodName">The name of the method that generate the message (automatically retrieved).</param>
        public static void WriteWarningMessage(string text, [CallerMemberName] string methodName = c_DefaultMethodName)
        {
            WriteMessage(LogLevels.Warning, methodName, text);
        }

        /// <summary>
        /// Append an error message to the message queue.
        /// </summary>
        /// <param name="text">The message text.</param>
        /// <param name="methodName">The name of the method that generate the message (automatically retrieved).</param>
        public static void WriteErrorMessage(string text, [CallerMemberName] string methodName = c_DefaultMethodName)
        {
            WriteMessage(LogLevels.Error, methodName, text);
        }

        /// <summary>
        /// Append a fatal error message to the message queue.
        /// </summary>
        /// <param name="text">The message text.</param>
        /// <param name="methodName">The name of the method that generate the message (automatically retrieved).</param>
        public static void WriteFatalMessage(string text, [CallerMemberName] string methodName = c_DefaultMethodName)
        {
            WriteMessage(LogLevels.Fatal, methodName, text);
        }

        /// <summary>
        /// This method read a message from the message queue and pass it to all the configured channels in order to be processed.
        /// </summary>
        private static void DoWork()
        {
            LogMessage msg;
            while (true)
            {
                try
                {
                    msg = s_MessageQuque.Take(s_Cancel.Token);
                    if (msg == null)
                    {
                        return;
                    }
                    foreach (LogChannelRecord record in s_LogChannels)
                    {
                        if (record.CanProcess(msg.Level))
                        {
                            record.Channel.WriteMessage(msg, record.MessageFormat);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Enable the message processing of the logger.
        /// </summary>
        public static void Start()
        {
            if (s_WorkerThread == null)
            {
                //create a new cancellation token
                s_Cancel = new CancellationTokenSource();
                //create a new task that will process the new messages
                s_WorkerThread = new Thread(DoWork);
                s_WorkerThread.Start();
            }
        }

        /// <summary>
        /// Disable the message processing.
        /// </summary>
        public static void Stop()
        {
            if (!CompleteMessageProcessing)
            {
                s_Cancel.Cancel();
            }
            else
            {
                s_MessageQuque.Add(null);
            }
            s_WorkerThread.Join();
            s_WorkerThread = null;
            s_Cancel.Dispose();
        }

        /// <summary>
        /// Checks if the specified type allow multiple instances.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ILogChannel"/> that has to be tested for allowing multiple instances.</typeparam>
        /// <returns>True if multiple instances are allowed, otherwise false.</returns>
        private static bool IsSingleInstanceChannel<T>() where T : ILogChannel
        {
            return typeof(T).GetCustomAttribute<AllowMultipleInstancesAttribute>() == null;
        }

        /// <summary>
        /// Adds a new log channel to the <see cref="LogManager"/>.
        /// </summary>
        /// <typeparam name="T">The type of the channel that has to be added.</typeparam>
        /// <param name="settings">The channel settings.</param>
        /// <returns>True if succeed, false otherwise.</returns>
        public static bool AddChannel<T>(LogChannelSettings settings) where T : ILogChannel, new()
        {
            //check if multiple instances allowed
            if (IsSingleInstanceChannel<T>())
            {
                //check if an instance already exists
                if (s_LogChannels.OfType<T>().Count() != 0)
                {
                    //log the error and exit
                    Logger.Log(string.Format("Multiple instance of {0} channel are not allowed.", typeof(T).Name));
                    return false;
                }
            }
            ILogChannel channel = new T();
            try
            {
                channel.Initialize(settings.CustomSettings);
                s_LogChannels.Enqueue(new LogChannelRecord(settings, channel));
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("Channel {0} initialization fails with error: {1}", channel, ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Adds a log channel to the <see cref="LogManager"/>.
        /// </summary>
        /// <param name="channelType">The type of the channel that has to be added.</param>
        /// <param name="settings">The channel settings.</param>
        private static void AddChannel(Type channelType, LogChannelSettings settings)
        {
            s_AddChannelMethod.MakeGenericMethod(channelType).Invoke(null, new[] { settings });
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or set a flag that tell to the object if it has to complete the message processing or discard unprocessed messaged when it will be destroyed.
        /// </summary>
        public static bool CompleteMessageProcessing { get; set; }

        #endregion
    }
}
