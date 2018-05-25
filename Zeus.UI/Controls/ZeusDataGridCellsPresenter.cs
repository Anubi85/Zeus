using System.Windows;
using System.Windows.Controls.Primitives;

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
    }
}
