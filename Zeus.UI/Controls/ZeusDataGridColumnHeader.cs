using System.Windows;
using System.Windows.Controls.Primitives;
using Zeus.UI.Themes.Enums;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="DataGridColumnHeader"/>.
    /// </summary>
    public class ZeusDataGridColumnHeader : DataGridColumnHeader
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusDataGridColumnHeader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusDataGridColumnHeader), new FrameworkPropertyMetadata(typeof(ZeusDataGridColumnHeader)));
            ColorProperty = ZeusWindowBase.ColorProperty.AddOwner(typeof(ZeusDataGridColumnHeader), new FrameworkPropertyMetadata(ZeusColorStyles.Blue, FrameworkPropertyMetadataOptions.Inherits));
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusDataGridColumnHeader"/> color styles.
        /// </summary>
        public static readonly DependencyProperty ColorProperty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="ZeusDataGridColumnHeader"/> color style.
        /// </summary>
        public ZeusColorStyles Color
        {
            get { return (ZeusColorStyles)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        #endregion
    }
}
