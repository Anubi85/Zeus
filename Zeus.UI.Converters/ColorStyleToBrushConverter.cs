using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Zeus.UI.Themes.Enums;

namespace Zeus.UI.Converters
{
    /// <summary>
    /// Handle conversion between <see cref="ZeusColorStyles"/> and <see cref="SolidColorBrush"/> types.
    /// </summary>
    public class ColorStyleToBrushConverter : IValueConverter
    {
        #region IValueConverter interface

        /// <summary>
        /// Converts from a <see cref="ZeusColorStyles"/> value to a <see cref="SolidColorBrush"/> value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Return a <see cref="SolidColorBrush"/> value according with the given <paramref name="value"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Opacity = (double)(parameter ?? 1.0);
            switch((ZeusColorStyles)value)
            {
                case ZeusColorStyles.Blue:
                    brush.Color = (Color)Application.Current.FindResource("BlueColor");
                    break;
                case ZeusColorStyles.Green:
                    brush.Color = (Color)Application.Current.FindResource("GreenColor");
                    break;
                case ZeusColorStyles.Red:
                    brush.Color = (Color)Application.Current.FindResource("RedColor");
                    break;
                case ZeusColorStyles.Yellow:
                    brush.Color = (Color)Application.Current.FindResource("YellowColor");
                    break;
                default:
                    return DependencyProperty.UnsetValue;
            }
            return brush;
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
            throw new NotImplementedException();
        }

        #endregion
    }
}
