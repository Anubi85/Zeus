using System;
using System.Runtime.InteropServices;

namespace Zeus.Helpers
{
    /// <summary>
    /// Helper class for native methods from Shell32 dll.
    /// </summary>
    public static class Shell32Helper
    {
        #region Methods

        /// <summary>
        /// Sends a message to the taskbar's status area.
        /// </summary>
        /// <param name="dwMessage">A value that specifies the action to be taken by this function.</param>
        /// <param name="lpdata">A pointer to a <see cref="NOTIFYICONDATA"/> structure. The content of the structure depends on the value of <paramref name="dwMessage"/>.</param>
        /// <returns>Returns true if succeed, false otherwise.</returns>
        [DllImport("shell32.dll", SetLastError = true)]
        public static extern bool Shell_NotifyIcon(NIM dwMessage, [In] ref NOTIFYICONDATA lpdata);

        #endregion

        #region Structures

#pragma warning disable 0649
        /// <summary>
        /// Contains information that the system needs to display notifications in the notification area.
        /// </summary>
        public struct NOTIFYICONDATA
        {
            /// <summary>
            /// The size of this structure, in bytes.
            /// </summary>
            public int cbSize;
            /// <summary>
            /// A handle to the window that receives notifications associated with an icon in the notification area.
            /// </summary>
            public IntPtr hWnd;
            /// <summary>
            /// The application-defined identifier of the taskbar icon.
            /// </summary>
            public uint uID;
            /// <summary>
            /// Flags that either indicate which of the other members of the structure contain valid data
            /// or provide additional information to the tooltip as to how it should display.
            /// </summary>
            public NIF uFlags;
            /// <summary>
            /// An application-defined message identifier.
            /// </summary>
            public uint uCallbackMessage;
            /// <summary>
            /// A handle to the icon to be added, modified, or deleted. 
            /// </summary>
            public IntPtr hIcon;
            /// <summary>
            /// Standard tooltip text.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szTip;
            /// <summary>
            /// The state of the icon.
            /// </summary>
            public NIF dwState;
            /// <summary>
            /// A value that specifies which bits of the <see cref="dwState"/> member are retrieved or modified.
            /// </summary>
            public NIF dwStateMask;
            /// <summary>
            /// The text to be displayed in a balloon notification.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szInfo;
            /// <summary>
            /// Icon version.
            /// </summary>
            public uint uVersion;
            /// <summary>
            /// Title text for a balloon notification.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szInfoTitle;
            /// <summary>
            /// Flags that can be set to modify the behavior and appearance of a balloon notification.
            /// </summary>
            public NIIF dwInfoFlags;
            /// <summary>
            /// A registered <see cref="Guid"/> that identifies the icon.
            /// </summary>
            public Guid guidItem;
            /// <summary>
            /// The handle of a customized notification icon provided by the application
            /// that should be used independently of the notification area icon.
            /// </summary>
            public IntPtr hBalloonIcon;
        }
#pragma warning restore 0649

        #endregion

        #region Enums

        /// <summary>
        /// Valid memebrs of <see cref="NOTIFYICONDATA"/> structure.
        /// </summary>
        [Flags]
        public enum NIF
        {
            /// <summary>
            /// The <see cref="NOTIFYICONDATA.uCallbackMessage"/> member is valid.
            /// </summary>
            MESSAGE = 0x01,
            /// <summary>
            /// The <see cref="NOTIFYICONDATA.hIcon"/> member is valid.
            /// </summary>
            ICON = 0x02,
            /// <summary>
            /// The <see cref="NOTIFYICONDATA.szTip"/> member is valid.
            /// </summary>
            TIP = 0x04,
            /// <summary>
            /// Display a balloon notification. The <see cref="NOTIFYICONDATA.szInfo"/>, <see cref="NOTIFYICONDATA.szInfoTitle"/>,
            /// <see cref="NOTIFYICONDATA.dwInfoFlags"/>, and <see cref="NOTIFYICONDATA.uVersion"/> members are valid.
            /// </summary>
            INFO = 0x10,
            /// <summary>
            /// The <see cref="NOTIFYICONDATA.dwState"/> and <see cref="NOTIFYICONDATA.dwStateMask"/> members are valid.
            /// </summary>
            STATE = 0x08,
            /// <summary>
            /// The <see cref="NOTIFYICONDATA.guidItem"/> is valid. When used <see cref="NOTIFYICONDATA.uID"/> member is ignored.
            /// </summary>
            GUID = 0x20,
            /// <summary>
            /// If the balloon notification cannot be displayed immediately, discard it.
            /// </summary>
            REALTIME = 0x40,
            /// <summary>
            /// Use the standard tooltip.
            /// </summary>
            SHOWTIP = 0x80
        }

        /// <summary>
        /// <see cref="Shell_NotifyIcon(NIM, ref NOTIFYICONDATA)"/> actions.
        /// </summary>
        public enum NIM
        {
            /// <summary>
            /// Adds an icon to the status area.
            /// </summary>
            ADD = 0x0,
            /// <summary>
            /// Modifies an icon in the status area.
            /// </summary>
            MODIFY = 0x1,
            /// <summary>
            /// Deletes an icon from the status area.
            /// </summary>
            DELETE = 0x2,
            /// <summary>
            /// Returns focus to the taskbar notification area.
            /// </summary>
            SETFOCUS = 0x3,
            /// <summary>
            /// Instructs the notification area to behave according to the version number specified in the <see cref="NOTIFYICONDATA.uVersion"/> member of the structure pointed to by <paramref name="lpdata"/> in <see cref="Shell_NotifyIcon(NIM, ref NOTIFYICONDATA)"/>.
            /// </summary>
            SETVERSION = 0x4
        }

        /// <summary>
        /// Notify icon states.
        /// </summary>
        [Flags]
        public enum NIS
        {
            /// <summary>
            /// The icon is hidden.
            /// </summary>
            HIDDEN = 0x1,
            /// <summary>
            /// The icon resource is shared between multiple icons.
            /// </summary>
            SHAREDICON = 0x2,
        }

        /// <summary>
        /// Flags that can be set to modify the behavior and appearance of a balloon notification.
        /// </summary>
        public enum NIIF
        {
            /// <summary>
            /// No icon.
            /// </summary>
            NONE = 0x00,
            /// <summary>
            /// An information icon.
            /// </summary>
            INFO = 0x01,
            /// <summary>
            /// A warning icon.
            /// </summary>
            WARNING = 0x02,
            /// <summary>
            /// An error icon.
            /// </summary>
            ERROR = 0x03,
            /// <summary>
            /// Use the icon identified in <see cref="NOTIFYICONDATA.hBalloonIcon"/> as the notification balloon's title icon.
            /// </summary>
            USER = 0x04,
            /// <summary>
            /// Do not play the associated sound.
            /// </summary>
            NOSOUND = 0x10,
            /// <summary>
            /// The large version of the icon should be used as the notification icon.
            /// </summary>
            LARGE_ICON = 0x20,
            /// <summary>
            /// Do not display the balloon notification if the current user is in "quiet time".
            /// </summary>
            RESPECT_QUIET_TIME = 0x80,
        }

        #endregion
    }
}
