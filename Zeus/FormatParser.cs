using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Zeus
{
    /// <summary>
    /// Parses a Zeus format string.
    /// </summary>
    public static class FormatParser
    {
        #region Constants

        /// <summary>
        /// Match pattern used by <see cref="Regex.Replace(string, string, string)"/> to format the file name.
        /// </summary>
        private const string c_MatchPattern = "(?<={{)(?i){0}(?-i)";

        #endregion

        #region Methods

        /// <summary>
        /// Prepare the given format string to be used in a <see cref="string.Format(string, object[])"/> call.
        /// </summary>
        /// <param name="format">The format string to be parsed.</param>
        /// <param name="keys">The keys that has to be replaced.</param>
        /// <returns>The parsed format string.</returns>
        public static string Parse(string format, Dictionary<string, string> keys)
        {
            string parsedFormat = format;
            foreach (KeyValuePair<string, string> kvp in keys)
            {
                parsedFormat = Regex.Replace(parsedFormat, string.Format(c_MatchPattern, kvp.Key), kvp.Value);
            }
            return parsedFormat;
        }

        #endregion
    }
}
