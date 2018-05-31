using System.Windows;
using System.Windows.Input;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="Window"/>.
    /// </summary>
    public class ZeusWindow : ZeusWindowBase
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusWindow), new FrameworkPropertyMetadata(typeof(ZeusWindow)));
        }

        /// <summary>
        /// Create a new instance of <see cref="ZeusWindow"/> and initialize its internal structures.
        /// </summary>
        public ZeusWindow()
        {
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, (sender, e) => SystemCommands.MaximizeWindow(this)));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, (sender, e) => SystemCommands.RestoreWindow(this)));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, (sender, e) => SystemCommands.MinimizeWindow(this)));
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle window minimize button visibility.
        /// </summary>
        public static readonly DependencyProperty ShowMinimizeButtonProperty = DependencyProperty.Register("ShowMinimizeButton", typeof(bool), typeof(ZeusWindow), new PropertyMetadata(true));
        /// <summary>
        /// <see cref="DependencyProperty"/> that handle window maximize button visibility.
        /// </summary>
        public static readonly DependencyProperty ShowMaximizeButtonProperty = DependencyProperty.Register("ShowMaximizeButton", typeof(bool), typeof(ZeusWindow), new PropertyMetadata(true));

        #endregion

        #region Properties

        /// <summary>
        /// Flag that handle window minimize button visibility.
        /// </summary>
        public bool ShowMinimizeButton
        {
            get { return (bool)GetValue(ShowMinimizeButtonProperty); }
            set { SetValue(ShowMinimizeButtonProperty, value); }
        }
        /// <summary>
        /// Flag that handle window maximize button visibility.
        /// </summary>
        public bool ShowMaximizeButton
        {
            get { return (bool)GetValue(ShowMaximizeButtonProperty); }
            set { SetValue(ShowMaximizeButtonProperty, value); }
        }

        #endregion
    }
}
