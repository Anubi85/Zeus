using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Zeus.UI.Converters
{
    /// <summary>
    /// Handle convertions that invert the thikness sign.
    /// </summary>
    public class InvertThicknessConverter : IValueConverter
    {
        #region IvalueConverter interface

        /// <summary>
        /// Invert thickness sign.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Return a <see cref="Visibility"/> value according with the given <paramref name="value"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Thickness tk = (Thickness)value;
                return new Thickness(-tk.Left, -tk.Top, -tk.Right, -tk.Bottom);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }
        /// <summary>
        /// Invert thickness sign.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Return a <see cref="Visibility"/> value according with the given <paramref name="value"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }

        #endregion
    }
}
