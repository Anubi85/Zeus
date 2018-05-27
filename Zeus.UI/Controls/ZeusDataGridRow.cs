using System.Windows;
using System.Windows.Controls;
using Zeus.UI.Themes.Enums;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="DataGridRow"/>.
    /// </summary>
    public class ZeusDataGridRow : DataGridRow
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusDataGridRow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusDataGridRow), new FrameworkPropertyMetadata(typeof(ZeusDataGridRow)));
            ColorProperty = ZeusWindowBase.ColorProperty.AddOwner(typeof(ZeusDataGridRow), new FrameworkPropertyMetadata(ZeusColorStyles.Blue, FrameworkPropertyMetadataOptions.Inherits));
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusDataGridRow"/> color styles.
        /// </summary>
        public static readonly DependencyProperty ColorProperty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="ZeusDataGridRow"/> color style.
        /// </summary>
        public ZeusColorStyles Color
        {
            get { return (ZeusColorStyles)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        #endregion
    }
}
