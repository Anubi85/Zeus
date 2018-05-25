using System.Windows;
using System.Windows.Controls.Primitives;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="DataGridColumnHeadersPresenter"/>.
    /// </summary>
    public class ZeusDataGridColumnHeadersPresenter : DataGridColumnHeadersPresenter
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusDataGridColumnHeadersPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusDataGridColumnHeadersPresenter), new FrameworkPropertyMetadata(typeof(ZeusDataGridColumnHeadersPresenter)));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates or identifies the element used to display the specified item.
        /// </summary>
        /// <returns>A <see cref="ZeusDataGridColumnHeader"/>.</returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ZeusDataGridColumnHeader();
        }

        #endregion
    }
}
