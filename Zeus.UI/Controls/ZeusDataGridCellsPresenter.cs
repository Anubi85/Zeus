using System.Windows;
using System.Windows.Controls.Primitives;
using Zeus.UI.Themes.Enums;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="DataGridCellsPresenter"/>.
    /// </summary>
    public class ZeusDataGridCellsPresenter : DataGridCellsPresenter
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusDataGridCellsPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusDataGridCellsPresenter), new FrameworkPropertyMetadata(typeof(ZeusDataGridCellsPresenter)));
            ColorProperty = ZeusWindowBase.ColorProperty.AddOwner(typeof(ZeusDataGridCellsPresenter), new FrameworkPropertyMetadata(ZeusColorStyles.Blue, FrameworkPropertyMetadataOptions.Inherits));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates or identifies the element used to display the specified item.
        /// </summary>
        /// <returns>A <see cref="ZeusDataGridCell"/>.</returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ZeusDataGridCell();
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusDataGridCellsPresenter"/> color styles.
        /// </summary>
        public static readonly DependencyProperty ColorProperty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="ZeusDataGridCellsPresenter"/> color style.
        /// </summary>
        public ZeusColorStyles Color
        {
            get { return (ZeusColorStyles)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        #endregion
    }
}
