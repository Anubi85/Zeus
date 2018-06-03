using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;

namespace Zeus.UI.MarkupExtensions
{
    /// <summary>
    /// Implements a markup extension that provides enum values starting from enum type.
    /// </summary>
    [MarkupExtensionReturnType(typeof(IEnumerable<object>))]
    public class EnumValues : MarkupExtension
    {
        #region MarkupExtension implementation

        /// <summary>
        /// Retrieve the anum values starting from the enum type.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>Retuns the list of the enum values.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (m_EnumType.IsEnum)
            {
                return Enum.GetValues(m_EnumType).Cast<object>().Select(ev => new { Value = (int)ev, Name = ev.ToString() });
            }
            else
            {
                throw new ArgumentException("Provided type is not an enum");
            }
        }

        #endregion

        #region Fields

        /// <summary>
        /// The type of the enum wich values has to be retrieved.
        /// </summary>
        private Type m_EnumType;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize class internal fields.
        /// </summary>
        /// <param name="enumType">The enum type wich values has to be retrieved.</param>
        public EnumValues(Type enumType)
        {
            m_EnumType = enumType;
        }

        #endregion
    }
}
