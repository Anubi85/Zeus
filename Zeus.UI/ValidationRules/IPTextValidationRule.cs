using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Zeus.UI.ValidationRules
{
    /// <summary>
    /// Provides logic to validate a text as an IP address.
    /// </summary>
    public class IPTextValidationRule : ValidationRule
    {
        #region ValidationRule oveeride

        /// <summary>
        /// Perform validation checks.
        /// </summary>
        /// <param name="value">The value from the binding target to check.</param>
        /// <param name="cultureInfo">The culture to use in this rule.</param>
        /// <returns>The <see cref="ValidationResult"/> object that contains the result of the validation checks.</returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string)
            {
                if (ValidateIPv4Address(value as string) || ValidateIPv6Address(value as string))
                {
                    return new ValidationResult(true, null);
                }
                else
                {
                    return new ValidationResult(false, "Invalid IP address format");
                }
            }
            else
            {
                return new ValidationResult(false, "Wrong value type, expected string");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks if the given string is a valid IPv4 address.
        /// </summary>
        /// <param name="strAddress">The string to be checked.</param>
        /// <returns>true if the string is a valid address, false otherwise.</returns>
        private bool ValidateIPv4Address(string strAddress)
        {
            return Regex.IsMatch(strAddress, @"^((25[0-5]|2[0-4][0-9]|[0-1]?[0-9]?[0-9])\.){3}(25[0-5]|2[0-4][0-9]|[0-1]?[0-9]?[0-9])$");
        }
        /// <summary>
        /// Checks if the given string is a valid IPv6 address.
        /// </summary>
        /// <param name="strAddress">The string to be checked.</param>
        /// <returns>true if the string is a valid address, false otherwise.</returns>
        private bool ValidateIPv6Address(string strAddress)
        {
            return Regex.IsMatch(strAddress, @"^(([0-9a-fA-F]{1,4}:){7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))$");
        }

        #endregion
    }
}
