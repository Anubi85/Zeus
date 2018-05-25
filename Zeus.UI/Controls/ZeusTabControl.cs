using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="TabControl"/>.
    /// </summary>
    public class ZeusTabControl : TabControl
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusTabControl), new FrameworkPropertyMetadata(typeof(ZeusTabControl)));
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle tab closing.
        /// </summary>
        public static readonly DependencyProperty CloseTabCommandProperty = DependencyProperty.Register("CloseTabCommand", typeof(ICommand), typeof(ZeusTabControl));

        #endregion

        #region Properties

        /// <summary>
        /// Get/sets the command that executes when a <see cref="ZeusTabItem"/> close button is clicked.
        /// </summary>
        public ICommand CloseTabCommand
        {
            get { return (ICommand)GetValue(CloseTabCommandProperty); }
            set { SetValue(CloseTabCommandProperty, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates or identifies the element used to display the specified item.
        /// </summary>
        /// <returns>A <see cref="ZeusTabItem"/>.</returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ZeusTabItem();
        }

        #endregion
    }
}
