using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using static Zeus.Helpers.User32Helper;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled dialog <see cref="Window"/>.
    /// </summary>
    public class ZeusDialog : ZeusWindowBase
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusDialog), new FrameworkPropertyMetadata(typeof(ZeusDialog)));
        }
        /// <summary>
        /// Initialize a new instance of <see cref="ZeusDialog"/>.
        /// </summary>
        public ZeusDialog()
        {
            AddHandler(LoadedEvent, new RoutedEventHandler(OnLoad));
            m_BlinkStart = null;
            m_TitlebarBlinkStoryboard = new Storyboard();
            m_TitlebarBlinkStoryboard.Children.Add(new DoubleAnimation(1, 0, TimeSpan.FromTicks(750000)));
            m_TitlebarBlinkStoryboard.RepeatBehavior = new RepeatBehavior(5);
            m_TitlebarBlinkStoryboard.AutoReverse = true;
            Storyboard.SetTargetProperty(m_TitlebarBlinkStoryboard, new PropertyPath(Border.OpacityProperty));
            m_WindowBorderBlinkStoryboard = new Storyboard();
            m_WindowBorderBlinkStoryboard.Children.Add(new ThicknessAnimation(new Thickness(1), new Thickness(0), TimeSpan.FromTicks(750000)));
            m_WindowBorderBlinkStoryboard.RepeatBehavior = new RepeatBehavior(5);
            m_WindowBorderBlinkStoryboard.AutoReverse = true;
            Storyboard.SetTargetProperty(m_WindowBorderBlinkStoryboard, new PropertyPath(Border.BorderThicknessProperty));
        }

        #endregion

        #region Fields

        /// <summary>
        /// A flag that indicates if the blink animation should start.
        /// null mean ready for new animation.
        /// false mean ready for animation.
        /// true mean animation started.
        /// </summary>
        private bool? m_BlinkStart;
        /// <summary>
        /// Storyboard used for blink animation when a modal dialog parent is click.
        /// </summary>
        private Storyboard m_TitlebarBlinkStoryboard;
        /// <summary>
        /// Storyboard used for blink animation when a modal dialog parent is click.
        /// </summary>
        private Storyboard m_WindowBorderBlinkStoryboard;
        /// <summary>
        /// The titlebar object.
        /// </summary>
        private FrameworkElement m_Titlebar;
        /// <summary>
        /// The window border object.
        /// </summary>
        private FrameworkElement m_WindowBorder;

        #endregion

        #region Methods

        /// <summary>
        /// Handle the loaded event.
        /// </summary>
        /// <param name="sender">The object that trigger the event.</param>
        /// <param name="e">Information about the occurred event.</param>
        private void OnLoad(object sender, RoutedEventArgs e)
        {
            //add a hook to process windows messages
            HwndSource.FromHwnd(new WindowInteropHelper(this).Handle).AddHook(WndProc);
            //retrieve components
            m_Titlebar = GetTemplateChild("titlebar") as FrameworkElement;
            m_WindowBorder = GetTemplateChild("border") as FrameworkElement;
        }
        /// <summary>
        /// Handles Win32 messages.
        /// </summary>
        /// <param name="hwnd">The window handle.</param>
        /// <param name="msg">The message ID.</param>
        /// <param name="wParam">The message's wParam value.</param>
        /// <param name="lParam">The message's lParam value.</param>
        /// <param name="handled">A flag that indicates whether the message was handled.</param>
        /// <returns>The appropriate return value depends on the particular message. See the message documentation details for the Win32 message being handled.</returns>
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch((WM)msg)
            {
                case WM.NCACTIVATE:
                    if (m_BlinkStart != null &&  m_BlinkStart == false)
                    {
                        m_BlinkStart = true;
                        m_TitlebarBlinkStoryboard.Begin(m_Titlebar);
                        if (m_WindowBorder.Visibility == Visibility.Visible)
                        {
                            m_WindowBorderBlinkStoryboard.Begin(m_WindowBorder);
                        }
                    }
                    break;
                case WM.WINDOWPOSCHANGING:
                    m_BlinkStart = false;
                    break;
                default:
                    m_BlinkStart = null;
                    break;
            }
            return IntPtr.Zero;
        }

        #endregion
    }
}
