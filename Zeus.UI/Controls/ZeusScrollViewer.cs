using System.Windows;
using System.Windows.Controls;
using Zeus.UI.Themes.Enums;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="ScrollViewer"/>.
    /// </summary>
    public class ZeusScrollViewer : ScrollViewer
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusScrollViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusScrollViewer), new FrameworkPropertyMetadata(typeof(ZeusScrollViewer)));
            ColorProperty = ZeusWindowBase.ColorProperty.AddOwner(typeof(ZeusScrollViewer), new FrameworkPropertyMetadata(ZeusColorStyles.Blue, FrameworkPropertyMetadataOptions.Inherits));
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusScrollViewer"/> color styles.
        /// </summary>
        public static readonly DependencyProperty ColorProperty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="ZeusScrollViewer"/> color style.
        /// </summary>
        public ZeusColorStyles Color
        {
            get { return (ZeusColorStyles)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        #endregion
    }
}
