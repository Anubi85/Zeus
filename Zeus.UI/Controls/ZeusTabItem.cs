using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="TabControl"/>.
    /// </summary>
    public class ZeusTabItem : TabItem
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusTabItem), new FrameworkPropertyMetadata(typeof(ZeusTabItem)));
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle tab closing.
        /// </summary>
        public static readonly DependencyProperty CloseTabCommandProperty = DependencyProperty.Register("CloseTabCommand", typeof(ICommand), typeof(ZeusTabItem));

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
    }
}
