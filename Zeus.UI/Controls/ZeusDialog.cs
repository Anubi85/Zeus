using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Animation;
using Zeus.Helpers;

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
            m_BlinkStart = false;
            m_BlinkStoryboard = new Storyboard();
            m_BlinkStoryboard.Children.Add(new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.1)));
            m_BlinkStoryboard.RepeatBehavior = new RepeatBehavior(5);
            m_BlinkStoryboard.AutoReverse = true;
            Storyboard.SetTargetProperty(m_BlinkStoryboard, new PropertyPath("BorderBrush.Opacity"));
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle window flashing for modal dialog when the paren is clicked.
        /// </summary>
        public static readonly DependencyProperty IsModalFlashingEnabledProperty = DependencyProperty.Register("IsModalFlashingEnabled", typeof(bool), typeof(ZeusDialog), new PropertyMetadata(true));

        #endregion

        #region Properties

        /// <summary>
        /// Flag that handle window window flashing for modal dialog when the paren is clicked.
        /// </summary>
        public bool IsModalFlashingEnabled
        {
            get { return (bool)GetValue(IsModalFlashingEnabledProperty); }
            set { SetValue(IsModalFlashingEnabledProperty, value); }
        }

        #endregion

        #region Fields

        /// <summary>
        /// A flag that indicates if the blink animation should start.
        /// false mean ready for new animation.
        /// true mean ready for animation.
        /// </summary>
        private bool m_BlinkStart;
        /// <summary>
        /// Storyboard used for blink animation when a modal dialog parent is click.
        /// </summary>
        private Storyboard m_BlinkStoryboard;

        #endregion

        #region Methods

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
        protected override IntPtr WndProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                if (IsModalFlashingEnabled)
                {
                    User32Helper.CWPSTRUCT msg = Marshal.PtrToStructure<User32Helper.CWPSTRUCT>(lParam);
                    switch ((User32Helper.WM)msg.message)
                    {
                        case User32Helper.WM.NCACTIVATE:
                            if (m_BlinkStart && msg.wParam == IntPtr.Zero && msg.lParam == IntPtr.Zero)
                            {
                                m_BlinkStart = false;
                                m_BlinkStoryboard.Begin(this);
                            }
                            break;
                        case User32Helper.WM.WINDOWPOSCHANGING:
                            m_BlinkStart = true;
                            break;
                        default:
                            m_BlinkStart = false;
                            break;
                    }
                }
            }
            return base.WndProc(nCode, wParam, lParam);
        }

        #endregion
    }
}
