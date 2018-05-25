using System.Windows;
using System.Windows.Controls;

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
        }

        #endregion
    }
}
