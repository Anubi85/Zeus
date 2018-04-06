using Zeus.Helpers;
using Zeus.UI.Enums;
using Zeus.UI.Extensions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Window tray icon control.
    /// </summary>
    public class ZeusTrayIcon : UIElement, IDisposable
    {
        #region IDisposable interface

        /// <summary>
        /// Release managed resources of the <see cref="ZeusTrayIcon"/> instance.
        /// </summary>
        public void Dispose()
        {
            if (!m_IsDisposed)
            {
                Application.Current.Exit -= OnApplicationExit;

                m_MessageWindow.RemoveHook(WndProc);
                m_MessageWindow.Dispose();
                m_Icon?.Dispose();
                Shell32Helper.Shell_NotifyIcon(Shell32Helper.NIM.DELETE, ref m_IconData);

                m_IsDisposed = true;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of <see cref="ZeusTrayIcon"/> and initialize its internal fields.
        /// </summary>
        public ZeusTrayIcon()
        {
            m_Id = s_NextId++;

            m_MessageWindow = new HwndSource(0, 0, 0, 0, 0, string.Empty, IntPtr.Zero);
            m_MessageWindow.AddHook(WndProc);

            CreateTrayIcon();

            Application.Current.Exit += OnApplicationExit;
        }

        #endregion

        #region Constants

        /// <summary>
        /// ID for the custom message sent by tray icon.
        /// </summary>
        private const uint c_msgID = 0x401;

        #endregion

        #region Fields

        /// <summary>
        /// Flag that track Dispose method calling.
        /// </summary>
        private bool m_IsDisposed;
        /// <summary>
        /// Tray icon data.
        /// </summary>
        private Shell32Helper.NOTIFYICONDATA m_IconData;
        /// <summary>
        /// Id of the current tray icon.
        /// </summary>
        private uint m_Id;
        /// <summary>
        /// Native window that handle tray icon messages.
        /// </summary>
        private HwndSource m_MessageWindow;
        /// <summary>
        /// <see cref="Icon"/> object created by <see cref="Image"/> property.
        /// </summary>
        private Icon m_Icon;
        /// <summary>
        /// Taskbar created message ID.
        /// </summary>
        private uint m_TaskbarCreatedMsgID = User32Helper.RegisterWindowMessage("TaskbarCreated");
        /// <summary>
        /// ID of the next tray icon that will be created.
        /// </summary>
        private static uint s_NextId = 0;

        #endregion

        #region Event Handlers

        /// <summary>
        /// If object is not disposed release managed resources of the <see cref="ZeusTrayIcon"/> instance when the application exits.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            Dispose();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="ZeusTrayIcon"/> image.
        /// </summary>
        public ImageSource Image
        {
            get { return GetValue(ImageProperty) as ImageSource; }
            set
            {
                SetValue(ImageProperty, value);
                m_Icon?.Dispose();
                if (value != null)
                {
                    m_Icon = value.ToIcon();
                    m_IconData.hIcon = m_Icon.Handle;
                    m_IconData.uFlags |= Shell32Helper.NIF.ICON;
                }
                else
                {
                    m_Icon = null;
                    m_IconData.hIcon = IntPtr.Zero;
                }
                UpdateIconData();
            }
        }
        /// <summary>
        /// Gets or sets <see cref="ZeusTrayIcon"/> tooltip.
        /// </summary>
        public string Tooltip
        {
            get { return GetValue(TooltipProperty) as string; }
            set
            {
                m_IconData.szTip = value;
                m_IconData.uFlags |= Shell32Helper.NIF.TIP | Shell32Helper.NIF.SHOWTIP;
                UpdateIconData();
            }
        }
        /// <summary>
        /// Gets or sets icon context menu.
        /// </summary>
        public ContextMenu ContextMenu
        {
            get { return GetValue(ContextMenuProperty) as ContextMenu; }
            set { SetValue(ContextMenuProperty, value); }
        }
        /// <summary>
        /// Gets or sets the automatic display mode of the icon <see cref="ContextMenu"/>.
        /// </summary>
        public ShowContexMenu ShowContextMenu
        {
            get { return (ShowContexMenu)GetValue(ShowContextMenuProperty); }
            set { SetValue(ShowContextMenuProperty, value); }
        }

        #endregion

        #region Dependency Properties

        /// <summary>
        /// <see cref="DependencyProperty"/> for icon image.
        /// </summary>
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(ZeusTrayIcon), new PropertyMetadata(null, ImagePropertyChanged));
        /// <summary>
        /// <see cref="DependencyProperty"/> for icon tooltip.
        /// </summary>
        public static readonly DependencyProperty TooltipProperty = DependencyProperty.Register("Tooltip", typeof(string), typeof(ZeusTrayIcon), new PropertyMetadata(null, TooltipPropertyChanged));
        /// <summary>
        /// <see cref="DependencyProperty"/> for icon context menu.
        /// </summary>
        public static readonly DependencyProperty ContextMenuProperty = DependencyProperty.Register("ContextMenu", typeof(ContextMenu), typeof(ZeusTrayIcon));
        /// <summary>
        /// <see cref="DependencyProperty"/> that control the automatic display of the <see cref="ContextMenu"/>.
        /// </summary>
        public static readonly DependencyProperty ShowContextMenuProperty = DependencyProperty.Register("ShowContextMenu", typeof(ShowContexMenu), typeof(ZeusTrayIcon));

        #endregion

        #region Dependency Properties Events

        /// <summary>
        /// Callback method executed when the <see cref="DependencyProperty"/> <see cref="ImageProperty"/> changes.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> that generate the event.</param>
        /// <param name="e">Provides information about the updated property.</param>
        private static void ImagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ImageSource src = e.NewValue as ImageSource;
            src?.Freeze();
            (d as ZeusTrayIcon).Image = src;
        }

        /// <summary>
        /// Callback method executed when the <see cref="DependencyProperty"/> <see cref="TooltipProperty"/> changes.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void TooltipPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ZeusTrayIcon).Tooltip = e.NewValue as string;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a new tray icon.
        /// </summary>
        private void CreateTrayIcon()
        {
            //check if icon data has already been initialized
            if (m_IconData.cbSize == 0)
            {
                m_IconData = new Shell32Helper.NOTIFYICONDATA();
                m_IconData.cbSize = Marshal.SizeOf(m_IconData);
                m_IconData.hWnd = m_MessageWindow.Handle;
                m_IconData.uCallbackMessage = c_msgID;
                m_IconData.uID = m_Id;
                m_IconData.uVersion = 0x4;

                m_IconData.uFlags = Shell32Helper.NIF.MESSAGE;
            }

            if (GetValue(DesignerProperties.IsInDesignModeProperty).Equals(false))
            {
                Shell32Helper.Shell_NotifyIcon(Shell32Helper.NIM.ADD, ref m_IconData);
                Shell32Helper.Shell_NotifyIcon(Shell32Helper.NIM.SETVERSION, ref m_IconData);
            }
        }

        /// <summary>
        /// Update an existing tray icon.
        /// </summary>
        private void UpdateIconData()
        {
            Shell32Helper.Shell_NotifyIcon(Shell32Helper.NIM.MODIFY, ref m_IconData);
        }

        /// <summary>
        /// Open the <see cref="ContextMenu"/> if it is defined and the condition is valid.
        /// </summary>
        /// <param name="condition">The condition that has to be valid in order to open the <see cref="ContextMenu"/>.</param>
        private void OpenContextMenu(ShowContexMenu condition)
        {
            ContextMenu.IsOpen = ContextMenu != null && ShowContextMenu.HasFlag(condition);
            User32Helper.SetForegroundWindow(m_MessageWindow.Handle);
        }

        /// <summary>
        /// Handle messages posted by tray icon.
        /// </summary>
        /// <param name="hwnd">Handle of the window that generate the message.</param>
        /// <param name="msg">Message ID.</param>
        /// <param name="wParam">Message wParam value.</param>
        /// <param name="lParam">Message lParam value.</param>
        /// <param name="handled">A value that indicates if the message has been handled.</param>
        /// <returns>A value that depends from the processed message.</returns>
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == c_msgID)
            {
                switch ((User32Helper.WM)lParam.ToInt32())
                {
                    case User32Helper.WM.LBUTTONDOWN:
                        OpenContextMenu(ShowContexMenu.OnLeftClick);
                        break;
                    case User32Helper.WM.RBUTTONDOWN:
                        OpenContextMenu(ShowContexMenu.OnRightClick);
                        break;
                    case User32Helper.WM.MBUTTONDOWN:
                        OpenContextMenu(ShowContexMenu.OnMiddleClick);
                        break;
                    default:
                        break;
                }
            }
            else if (msg == m_TaskbarCreatedMsgID)
            {
                CreateTrayIcon();
            }
            else
            {
                return User32Helper.DefWindowProc(hwnd, (uint)msg, wParam, lParam);
            }
            return IntPtr.Zero;
        }

        #endregion
    }
}