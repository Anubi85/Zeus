using System.Windows;
using System.Windows.Controls.Primitives;
using Zeus.UI.Themes.Enums;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="ScrollBar"/>.
    /// </summary>
    public class ZeusScrollBar : ScrollBar
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusScrollBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusScrollBar), new FrameworkPropertyMetadata(typeof(ZeusScrollBar)));
            ColorProperty = ZeusWindowBase.ColorProperty.AddOwner(typeof(ZeusScrollBar), new FrameworkPropertyMetadata(ZeusColorStyles.Blue, FrameworkPropertyMetadataOptions.Inherits));
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusScrollBar"/> color styles.
        /// </summary>
        public static readonly DependencyProperty ColorProperty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="ZeusScrollBar"/> color style.
        /// </summary>
        public ZeusColorStyles Color
        {
            get { return (ZeusColorStyles)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        #endregion
    }
}
