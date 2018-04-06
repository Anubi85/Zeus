using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Log
{
    /// <summary>
    /// This enum define all the available log levels.
    /// </summary>
    public enum LogLevels
    {
        /// <summary>
        /// Trace level.
        /// </summary>
        Trace,
        /// <summary>
        /// Debug level.
        /// </summary>
        Debug,
        /// <summary>
        /// Information level.
        /// </summary>
        Info,
        /// <summary>
        /// Success level.
        /// </summary>
        Success,
        /// <summary>
        /// Warning level.
        /// </summary>
        Warning,
        /// <summary>
        /// Non fatal error level.
        /// </summary>
        Error,
        /// <summary>
        /// Fatal error level.
        /// </summary>
        Fatal,

    }
}