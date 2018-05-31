using System.Windows;
using System.Windows.Controls;
using Zeus.UI.Themes.Enums;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="ZeusListBoxItem"/>.
    /// </summary>
    public class ZeusListBoxItem : ListBoxItem
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusListBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusListBoxItem), new FrameworkPropertyMetadata(typeof(ZeusListBoxItem)));
            ColorProperty = ZeusWindowBase.ColorProperty.AddOwner(typeof(ZeusListBoxItem), new FrameworkPropertyMetadata(ZeusColorStyles.Blue, FrameworkPropertyMetadataOptions.Inherits));
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusListBoxItem"/> color styles.
        /// </summary>
        public static readonly DependencyProperty ColorProperty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="ZeusListBoxItem"/> color style.
        /// </summary>
        public ZeusColorStyles Color
        {
            get { return (ZeusColorStyles)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        #endregion
    }
}
