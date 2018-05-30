using System;
using System.Windows;
using System.Windows.Input;
using Zeus.Helpers;
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
            m_WndProcCallback = new User32Helper.CallWndProc(WndProc);
            m_HookId = User32Helper.SetWindowsHookEx(User32Helper.WH.CALLWNDPROC, m_WndProcCallback, IntPtr.Zero, Kernel32Helper.GetCurrentThreadId());
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle window close button visibility.
        /// </summary>
        public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register("ShowCloseButton", typeof(bool), typeof(ZeusWindowBase), new PropertyMetadata(true));
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

        #region Fields

        /// <summary>
        /// The identifier of the hook used to intercept the window messages.
        /// </summary>
        private IntPtr m_HookId;
        /// <summary>
        /// Reference to the <see cref="User32Helper.CallWndProc"/> callback delegate.
        /// </summary>
        private User32Helper.CallWndProc m_WndProcCallback;

        #endregion

        #region Methods

        /// <summary>
        /// Perform managed object cleanup.
        /// </summary>
        /// <param name="e">Information about the closing event.</param>
        protected override void OnClosed(EventArgs e)
        {
            User32Helper.UnhookWindowsHookEx(m_HookId);
            base.OnClosed(e);
        }
        /// <summary>
        /// Delegate for CallWndProc hooks.
        /// </summary>
        /// <param name="nCode">A code the hook procedure uses to determine how to process the message.
        /// If nCode is less than zero, the hook procedure must pass the message to the <see cref="User32Helper.CallNextHookEx(IntPtr, int, int, IntPtr)"/>
        /// function without further processing and should return the value returned by <see cref="User32Helper.CallNextHookEx(IntPtr, int, int, IntPtr)"/>.</param>
        /// <param name="wParam">Specifies whether the message was sent by the current thread. If the message was sent by the current thread, it is nonzero; otherwise, it is zero.</param>
        /// <param name="lParam">A pointer to a <see cref="User32Helper.CWPSTRUCT"/> structure that contains details about the message..</param>
        /// <returns>If returns <see cref="IntPtr.Zero"/> other message hooks in the queue are not processed,
        /// otherwise processing continues normally.</returns>
        protected virtual IntPtr WndProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            return User32Helper.CallNextHookEx(m_HookId, nCode, wParam, lParam);
        }

        #endregion
    }
}
