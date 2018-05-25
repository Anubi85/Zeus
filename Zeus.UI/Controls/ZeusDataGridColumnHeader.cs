using System.Windows;
using System.Windows.Controls.Primitives;

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
        }

        #endregion
    }
}
