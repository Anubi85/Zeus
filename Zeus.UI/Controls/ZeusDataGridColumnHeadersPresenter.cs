using System.Windows;
using System.Windows.Controls.Primitives;
using Zeus.UI.Themes.Enums;

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
            ColorProperty = ZeusWindowBase.ColorProperty.AddOwner(typeof(ZeusDataGridColumnHeadersPresenter), new FrameworkPropertyMetadata(ZeusColorStyles.Blue, FrameworkPropertyMetadataOptions.Inherits));
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

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusDataGridColumnHeadersPresenter"/> color styles.
        /// </summary>
        public static readonly DependencyProperty ColorProperty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="ZeusDataGridColumnHeadersPresenter"/> color style.
        /// </summary>
        public ZeusColorStyles Color
        {
            get { return (ZeusColorStyles)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        #endregion
    }
}
