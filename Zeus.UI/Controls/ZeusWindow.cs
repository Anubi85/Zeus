using System;
using System.Windows;
using System.Windows.Input;
using Zeus.UI.Themes.Enums;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="Window"/>.
    /// </summary>
    public class ZeusWindow : Window
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
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, (sender, e) => SystemCommands.CloseWindow(this)));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, (sender, e) => SystemCommands.MaximizeWindow(this)));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, (sender, e) => SystemCommands.RestoreWindow(this)));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, (sender, e) => SystemCommands.MinimizeWindow(this)));
            Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("/Zeus.UI;component/Styles/Zeus.xaml", UriKind.Relative) });
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle window close button visibility.
        /// </summary>
        public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register("ShowCloseButton", typeof(bool), typeof(ZeusWindow));
        /// <summary>
        /// <see cref="DependencyProperty"/> that handle window minimize button visibility.
        /// </summary>
        public static readonly DependencyProperty ShowMinimizeButtonProperty = DependencyProperty.Register("ShowMinimizeButton", typeof(bool), typeof(ZeusWindow));
        /// <summary>
        /// <see cref="DependencyProperty"/> that handle window maximize button visibility.
        /// </summary>
        public static readonly DependencyProperty ShowMaximizeButtonProperty = DependencyProperty.Register("ShowMaximizeButton", typeof(bool), typeof(ZeusWindow));
        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusWindow"/> color styles.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(ZeusColorStyles), typeof(ZeusWindow));

        #endregion

        #region Properties

        /// <summary>
        /// Flag that handle window close button visibility.
        /// </summary>
        public bool ShowCloseButton
        {
            get { return (bool)GetValue(ShowCloseButtonProperty); }
            set { SetValue(ShowCloseButtonProperty, value); }
        }
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
        /// <summary>
        /// Gets or sets <see cref="ZeusWindow"/> color style.
        /// </summary>
        public ZeusColorStyles Color
        {
            get { return (ZeusColorStyles)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        #endregion
    }
}
