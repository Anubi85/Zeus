using System.Windows;
using System.Windows.Controls;
using Zeus.UI.Themes.Enums;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="Label"/>.
    /// </summary>
    public class ZeusLabel : Label
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusLabel), new FrameworkPropertyMetadata(typeof(ZeusLabel)));
            ColorProperty = ZeusWindowBase.ColorProperty.AddOwner(typeof(ZeusLabel), new FrameworkPropertyMetadata(ZeusColorStyles.Blue, FrameworkPropertyMetadataOptions.Inherits));
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusLabel"/> color styles.
        /// </summary>
        public static readonly DependencyProperty ColorProperty;
        /// <summary>
        /// <see cref="DependencyProperty"/> that handle border color.
        /// </summary>
        public static readonly DependencyProperty ShowColoredBorderProperty = DependencyProperty.Register("ShowColoredBorder", typeof(bool), typeof(ZeusLabel), new PropertyMetadata(true));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="ZeusLabel"/> color style.
        /// </summary>
        public ZeusColorStyles Color
        {
            get { return (ZeusColorStyles)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        /// <summary>
        /// Flag rhat handle border color.
        /// </summary>
        public bool ShowColoredBorder
        {
            get { return (bool)GetValue(ShowColoredBorderProperty); }
            set { SetValue(ShowColoredBorderProperty, value); }
        }

        #endregion
    }
}
