using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeus.Patterns;

namespace Zeus.UI.Mvvm
{
    /// <summary>
    /// Enum that provides the available dialog results.
    /// </summary>
    public sealed class DialogResult : TypeSafeEnumBase<DialogResult>
    {
        #region Values

        /// <summary>
        /// The dialog operation has been completed sucessfully.
        /// </summary>
        public static readonly DialogResult Success = new DialogResult(true);
        /// <summary>
        /// The dialog operation has been cancelled by the user.
        /// </summary>
        public static readonly DialogResult Cancel = new DialogResult(false);
        /// <summary>
        /// The dialog operation has been aborted.
        /// </summary>
        public static readonly DialogResult Abort = new DialogResult(null);

        #endregion

        #region Fields

        /// <summary>
        /// The <see cref="Nullable{Boolean}"/> value assigned to the instance.
        /// </summary>
        private bool? m_Value;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize a new instance assigning the provided value.
        /// </summary>
        /// <param name="value"></param>
        private DialogResult(bool? value)
        {
            m_Value = value;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Converts the <see cref="DialogResult"/> value into a <see cref="Nullable{Boolean}"/> value that represent the current value.
        /// </summary>
        /// <param name="value">The <see cref="DialogResult"/> value to be converted.</param>
        public static implicit operator bool? (DialogResult value)
        {
            return value.m_Value;
        }
        /// <summary>
        /// Converts the <see cref="Nullable{Boolean}"/> value into the corresponding<see cref="DialogResult"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{Boolean}"/> value to be converted.</param>
        public static implicit operator DialogResult (bool? value)
        {
            return value.HasValue ? (value.Value ? Success : Cancel) : Abort;
        }

        #endregion
    }
}
