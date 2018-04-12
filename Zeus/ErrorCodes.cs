using System.Collections.Generic;
using System.Linq;

namespace Zeus
{
    /// <summary>
    /// Defines the Zeus framework error codes.
    /// </summary>
    public enum ErrorCodes
    {
        /// <summary>
        /// Undefined error.
        /// </summary>
        Undefined,
        /// <summary>
        /// Tag not valid error.
        /// </summary>
        InvalidTag,
        /// <summary>
        /// Index not valid error.
        /// </summary>
        InvalidIndex,
        /// <summary>
        /// Type not matching or not compatible error.
        /// </summary>
        TypeMismatch,
        /// <summary>
        /// Tag already exists error.
        /// </summary>
        DuplicatedTag,
        /// <summary>
        /// Configuration manager initialization failed error.
        /// </summary>
        ConfigManagerInitFailed,
        /// <summary>
        /// Requested key missing error.
        /// </summary>
        MissingSettingsKey,
        /// <summary>
        /// Invalid settings value error.
        /// </summary>
        InvalidSettings,
        /// <summary>
        /// Repository path does not exist error.
        /// </summary>
        RepositoryPathNotExist,
    }
}
