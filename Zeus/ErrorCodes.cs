using System.Collections.Generic;
using System.Linq;

namespace Zeus
{
    /// <summary>
    /// Defines the Zeus framework error codes.
    /// </summary>
    public sealed class ErrorCodes
    {
        #region Constructor

        /// <summary>
        /// Initialize class static fields.
        /// </summary>
        static ErrorCodes()
        {
            s_InstanceCounter = 0;
            s_Values = new List<ErrorCodes>();
        }

        /// <summary>
        /// Creates a new instance of <see cref="ErrorCodes"/> with the specified message.
        /// </summary>
        /// <param name="message"></param>
        private ErrorCodes(string message)
        {
            m_Code = s_InstanceCounter++;
            m_Message = message;
            s_Values.Add(this);
        }

        #endregion

        #region Fields

        /// <summary>
        /// Static fields that count class instances.
        /// </summary>
        private static int s_InstanceCounter;
        /// <summary>
        /// List that contains all the avaialble class instances.
        /// </summary>
        private static List<ErrorCodes> s_Values;
        /// <summary>
        /// The numeric code associated with the error.
        /// </summary>
        private int m_Code;
        /// <summary>
        /// The message associated with the error code.
        /// </summary>
        private string m_Message;

        #endregion

        #region Operators

        /// <summary>
        /// Cast operator that automatically convert an <see cref="ErrorCodes"/> instacne into an int.
        /// </summary>
        /// <param name="code">The <see cref="ErrorCodes"/> instance that has to be converted.</param>
        public static implicit operator int(ErrorCodes code)
        {
            if (code == null)
            {
                return 0;
            }
            return code.m_Code;
        }
        /// <summary>
        /// Cast operator that automatically convert an integer into the corresponding <see cref="ErrorCodes"/> instance.
        /// </summary>
        /// <param name="code">The integer value that has to be converted.</param>
        public static implicit operator ErrorCodes(int code)
        {
            return s_Values.FirstOrDefault(ec => ec.m_Code == code) ?? Undefined;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the default message associated with the <see cref="ErrorCodes"/> instance.
        /// </summary>
        public string Message { get { return m_Message; } }

        #endregion

        #region Methods

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> that iterate through all the avaialble <see cref="ErrorCodes"/> instances.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> object that allow to iterato through all the avaialbel <see cref="ErrorCodes"/> instances.</returns>
        public static IEnumerable<ErrorCodes> GetValues()
        {
            return s_Values;
        }

        #endregion

        #region Values

        /// <summary>
        /// Undefined error.
        /// </summary>
        public static readonly ErrorCodes Undefined = new ErrorCodes("General error");
        /// <summary>
        /// Tag not valid error.
        /// </summary>
        public static readonly ErrorCodes InvalidTag = new ErrorCodes("Requested tag is not valid or not present.");
        /// <summary>
        /// Index not valid error.
        /// </summary>
        public static readonly ErrorCodes InvalidIndex = new ErrorCodes("Requested index is not valid or out of range.");
        /// <summary>
        /// Type not matching or not compatible error.
        /// </summary>
        public static readonly ErrorCodes TypeMismatch = new ErrorCodes("The conversion between the requested types is not possible.");
        /// <summary>
        /// Tag already exists error.
        /// </summary>
        public static readonly ErrorCodes DuplicatedTag = new ErrorCodes("Requested tag already exists.");
        /// <summary>
        /// Configuration manager initialization failed error.
        /// </summary>
        public static readonly ErrorCodes ConfigManagerInitFailed = new ErrorCodes("Error occurred while initializing configuration editor.");
        /// <summary>
        /// Requested key missing error.
        /// </summary>
        public static readonly ErrorCodes MissingSettingsKey = new ErrorCodes("The requested key is missing.");

        #endregion
    }
}
