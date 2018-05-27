using System.Windows;
using System.Windows.Controls.Primitives;
using Zeus.UI.Themes.Enums;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="StatusBarItem"/>.
    /// </summary>
    public class ZeusStatusBarItem : StatusBarItem
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusStatusBarItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusStatusBarItem), new FrameworkPropertyMetadata(typeof(ZeusStatusBarItem)));
            ColorProperty = ZeusWindowBase.ColorProperty.AddOwner(typeof(ZeusStatusBarItem), new FrameworkPropertyMetadata(ZeusColorStyles.Blue, FrameworkPropertyMetadataOptions.Inherits));
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusStatusBarItem"/> color styles.
        /// </summary>
        public static readonly DependencyProperty ColorProperty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="ZeusStatusBarItem"/> color style.
        /// </summary>
        public ZeusColorStyles Color
        {
            get { return (ZeusColorStyles)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        #endregion
    }
}
