using System.Windows;
using System.Windows.Controls;

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
        }

        #endregion
    }
}
