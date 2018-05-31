using System.Windows;
using System.Windows.Controls;
using Zeus.UI.Themes.Enums;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="ListBox"/>.
    /// </summary>
    public class ZeusListBox : ListBox
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusListBox), new FrameworkPropertyMetadata(typeof(ZeusListBox)));
            ColorProperty = ZeusWindowBase.ColorProperty.AddOwner(typeof(ZeusListBox), new FrameworkPropertyMetadata(ZeusColorStyles.Blue, FrameworkPropertyMetadataOptions.Inherits));
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusListBox"/> color styles.
        /// </summary>
        public static readonly DependencyProperty ColorProperty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="ZeusListBox"/> color style.
        /// </summary>
        public ZeusColorStyles Color
        {
            get { return (ZeusColorStyles)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates or identifies the element used to display the specified item.
        /// </summary>
        /// <returns>A <see cref="ZeusDataGridRow"/>.</returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ZeusListBoxItem();
        }

        #endregion
    }
}
