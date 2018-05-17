using Zeus.UI.Enums;
using System.Windows;
using System.Windows.Controls;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="Button"/>.
    /// </summary>
    public class ZeusButton : Button
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusButton), new FrameworkPropertyMetadata(typeof(ZeusButton)));
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusButton"/> color styles.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(ZeusColorStyles), typeof(ZeusButton));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="ZeusButton"/> color style.
        /// </summary>
        public ZeusColorStyles Color
        {
            get { return (ZeusColorStyles)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        #endregion
    }
}
