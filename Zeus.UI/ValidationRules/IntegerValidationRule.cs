using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Zeus.UI.ValidationRules
{
    /// <summary>
    /// Provides logic to validate a text as an integer number within specified limits.
    /// </summary>
    public class IntegerValidationRule : ValidationRule
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
                int intValue;
                if (int.TryParse((value as string), out intValue))
                {
                    if (MinValue <= intValue && intValue <= MaxValue)
                    {
                        return new ValidationResult(true, null);
                    }
                    else
                    {
                        return new ValidationResult(false, string.Format("Number out of valid range [{0} : {1}]", MinValue, MaxValue));
                    }
                }
                else
                {
                    return new ValidationResult(false, "Invalid integer number format");
                }
            }
            else
            {
                return new ValidationResult(false, "Wrong value type, expected string");
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the maximum valid value for the integer number.
        /// </summary>
        public int MaxValue { get; set; }
        /// <summary>
        /// Gets or sets the minimum valid value for the integer number.
        /// </summary>
        public int MinValue { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize class fields to the default values.
        /// </summary>
        public IntegerValidationRule()
        {
            MaxValue = int.MaxValue;
            MinValue = int.MinValue;
        }

        #endregion
    }
}
