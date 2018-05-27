using System.Windows;
using System.Windows.Controls.Primitives;
using Zeus.UI.Themes.Enums;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="DataGridRowHeader"/>.
    /// </summary>
    public class ZeusDataGridRowHeader : DataGridRowHeader
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusDataGridRowHeader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusDataGridRowHeader), new FrameworkPropertyMetadata(typeof(ZeusDataGridRowHeader)));
            ColorProperty = ZeusWindowBase.ColorProperty.AddOwner(typeof(ZeusDataGridRowHeader), new FrameworkPropertyMetadata(ZeusColorStyles.Blue, FrameworkPropertyMetadataOptions.Inherits));
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusDataGridRowHeader"/> color styles.
        /// </summary>
        public static readonly DependencyProperty ColorProperty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="ZeusDataGridRowHeader"/> color style.
        /// </summary>
        public ZeusColorStyles Color
        {
            get { return (ZeusColorStyles)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        #endregion
    }
}
