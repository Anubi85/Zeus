using System.Windows;
using System.Windows.Controls;
using Zeus.UI.Themes.Enums;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="ComboBoxItem"/>.
    /// </summary>
    public class ZeusComboBoxItem : ComboBoxItem
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusComboBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusComboBoxItem), new FrameworkPropertyMetadata(typeof(ZeusComboBoxItem)));
            ColorProperty = ZeusWindowBase.ColorProperty.AddOwner(typeof(ZeusComboBoxItem), new FrameworkPropertyMetadata(ZeusColorStyles.Blue, FrameworkPropertyMetadataOptions.Inherits));
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusComboBoxItem"/> color styles.
        /// </summary>
        public static readonly DependencyProperty ColorProperty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="ZeusComboBoxItem"/> color style.
        /// </summary>
        public ZeusColorStyles Color
        {
            get { return (ZeusColorStyles)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        #endregion
    }
}
