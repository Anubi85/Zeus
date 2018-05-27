using System.Windows;
using System.Windows.Controls;
using Zeus.UI.Themes.Enums;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="DataGridCell"/>.
    /// </summary>
    public class ZeusDataGridCell : DataGridCell
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusDataGridCell()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusDataGridCell), new FrameworkPropertyMetadata(typeof(ZeusDataGridCell)));
            ColorProperty = ZeusWindowBase.ColorProperty.AddOwner(typeof(ZeusDataGridCell), new FrameworkPropertyMetadata(ZeusColorStyles.Blue, FrameworkPropertyMetadataOptions.Inherits));
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusDataGridCell"/> color styles.
        /// </summary>
        public static readonly DependencyProperty ColorProperty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="ZeusDataGridCell"/> color style.
        /// </summary>
        public ZeusColorStyles Color
        {
            get { return (ZeusColorStyles)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        #endregion
    }
}
