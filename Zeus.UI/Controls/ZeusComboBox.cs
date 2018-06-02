using System.Windows;
using System.Windows.Controls;
using Zeus.UI.Themes.Enums;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="ComboBox"/>.
    /// </summary>
    public class ZeusComboBox : ComboBox
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusComboBox), new FrameworkPropertyMetadata(typeof(ZeusComboBox)));
            ColorProperty = ZeusWindowBase.ColorProperty.AddOwner(typeof(ZeusComboBox), new FrameworkPropertyMetadata(ZeusColorStyles.Blue, FrameworkPropertyMetadataOptions.Inherits));
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusComboBox"/> color styles.
        /// </summary>
        public static readonly DependencyProperty ColorProperty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="ZeusComboBox"/> color style.
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
        /// <returns>A <see cref="ZeusComboBoxItem"/>.</returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ZeusComboBoxItem();
        }

        #endregion
    }
}
