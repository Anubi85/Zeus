using System.Windows;
using System.Windows.Controls;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="DataGrid"/>.
    /// </summary>
    public class ZeusDataGrid : DataGrid
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusDataGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusDataGrid), new FrameworkPropertyMetadata(typeof(ZeusDataGrid)));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates or identifies the element used to display the specified item.
        /// </summary>
        /// <returns>A <see cref="ZeusDataGridRow"/>.</returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ZeusDataGridRow();
        }

        #endregion
    }
}
