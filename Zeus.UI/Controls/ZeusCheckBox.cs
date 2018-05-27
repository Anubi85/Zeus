using System.Windows;
using System.Windows.Controls;
using Zeus.UI.Themes.Enums;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="CheckBox"/>.
    /// </summary>
    public class ZeusCheckBox : CheckBox
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusCheckBox), new FrameworkPropertyMetadata(typeof(ZeusCheckBox)));
            ColorProperty = ZeusWindowBase.ColorProperty.AddOwner(typeof(ZeusCheckBox), new FrameworkPropertyMetadata(ZeusColorStyles.Blue, FrameworkPropertyMetadataOptions.Inherits));
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusCheckBox"/> color styles.
        /// </summary>
        public static readonly DependencyProperty ColorProperty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="ZeusCheckBox"/> color style.
        /// </summary>
        public ZeusColorStyles Color
        {
            get { return (ZeusColorStyles)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        #endregion
    }
}
