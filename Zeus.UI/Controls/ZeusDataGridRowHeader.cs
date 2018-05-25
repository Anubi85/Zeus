using System.Windows;
using System.Windows.Controls.Primitives;

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
        }

        #endregion
    }
}
