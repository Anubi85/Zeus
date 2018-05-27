using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Zeus.UI.Converters
{
    /// <summary>
    /// Handle conversion between <see cref="bool"/> and <see cref="Visibility"/> types.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        #region IValueConverter interface

        /// <summary>
        /// Converts from a <see cref="bool"/> value to a <see cref="Visibility"/> value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Return a <see cref="Visibility"/> value according with the given <paramref name="value"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Converts from a <see cref="Visibility"/> value to a <see cref="bool"/> value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Return a <see cref="bool"/> value according with the given <paramref name="value"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible;
        }

        #endregion
    }
}
