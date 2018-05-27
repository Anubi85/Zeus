using System;
using System.Windows;
using
    System.Windows.Input;
using Zeus.UI.Themes.Enums;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Base Zeus styled <see cref="Window"/>.
    /// </summary>
    public abstract class ZeusWindowBase : Window
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusWindowBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusWindowBase), new FrameworkPropertyMetadata(typeof(ZeusWindowBase)));
        }

        /// <summary>
        /// Create a new instance of <see cref="ZeusWindowBase"/> and initialize its internal structures.
        /// </summary>
        public ZeusWindowBase()
        {
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, (sender, e) => SystemCommands.CloseWindow(this)));
            Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("/Zeus.UI;component/Styles/Zeus.xaml", UriKind.Relative) });
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle window close button visibility.
        /// </summary>
        public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register("ShowCloseButton", typeof(bool), typeof(ZeusWindowBase));
        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusWindow"/> color styles.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.RegisterAttached("Color", typeof(ZeusColorStyles), typeof(ZeusWindowBase), new FrameworkPropertyMetadata(ZeusColorStyles.Blue, FrameworkPropertyMetadataOptions.Inherits));

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
