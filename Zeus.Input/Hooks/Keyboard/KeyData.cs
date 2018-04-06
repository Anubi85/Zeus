using Zeus.Helpers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Zeus.Input.Hooks.Keyboard
{
    /// <summary>
    /// Data associated to key event.
    /// </summary>
    public class KeyData
    {
        #region Properties

        /// <summary>
        /// Key is not pressed.
        /// </summary>
        public bool IsUp { get; private set; }
        /// <summary>
        /// ALT modifier is pressed.
        /// </summary>
        public bool IsAltDown { get; private set; }
        /// <summary>
        /// Key is toggled.
        /// </summary>
        public bool IsToggled { get; private set; }
        /// <summary>
        /// Gets virtual key code.
        /// </summary>
        public int KeyCode { get; private set; }
        /// <summary>
        /// Gets keyboard message type.
        /// </summary>
        public User32Helper.WM MessageType { get; private set; }
        /// <summary>
        /// Counts how many key down events has been generated for the same key.
        /// </summary>
        public int DownCounter
        {
            get
            {
                if (!m_downCounter.ContainsKey(KeyCode))
                {
                    m_downCounter[KeyCode] = k_InitialCounter;
                }
                return m_downCounter[KeyCode];
            }
        }

        #endregion

        #region Fields

        /// <summary>
        /// wParam from <see cref="User32Helper.LowLevelKeyboardProc"/> delegate.
        /// </summary>
        private User32Helper.WM m_wParam;
        /// <summary>
        /// lParam from <see cref="User32Helper.LowLevelKeyboardProc"/> delegate.
        /// </summary>
        private User32Helper.KBDLLHOOKSTRUCT m_lParam;
        /// <summary>
        /// Status of the key
        /// </summary>
        private short m_keyStatus;
        /// <summary>
        /// Dictionary that stores the down counter for each key that have generated at least one event.
        /// </summary>
        private static Dictionary<int, int> m_downCounter = new Dictionary<int, int>();

        #endregion

        #region Constants

        /// <summary>
        /// Default down counter value.
        /// </summary>
        private const int k_InitialCounter = -1;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of <see cref="KeyData"/> and initialize its internal structures.
        /// </summary>
        /// <param name="wParam">wParam from <see cref="User32Helper.LowLevelKeyboardProc"/> delegate.</param>
        /// <param name="lParam">lParam from <see cref="User32Helper.LowLevelKeyboardProc"/> delegate.</param>
        public KeyData(User32Helper.WM wParam, IntPtr lParam)
        {
            m_wParam = wParam;
            m_lParam = (User32Helper.KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(User32Helper.KBDLLHOOKSTRUCT));
            m_keyStatus = User32Helper.GetKeyState(m_lParam.vkCode);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Populate class properties formatting the <see cref="m_wParam"/> and <see cref="m_lParam"/> class fields.
        /// </summary>
        internal void FormatData()
        {
            IsUp = (m_lParam.flags & User32Helper.LLKHF.UP) != 0;
            IsAltDown = (m_lParam.flags & User32Helper.LLKHF.ALTDOWN) != 0;
            IsToggled = (m_keyStatus & 0x1) != 0;
            KeyCode = m_lParam.vkCode;
            MessageType = m_wParam;
            if (IsUp)
            {
                m_downCounter[KeyCode] = k_InitialCounter;
            }
            else
            {
                if (!m_downCounter.ContainsKey(KeyCode))
                {
                    m_downCounter[KeyCode] = k_InitialCounter;
                }
                m_downCounter[KeyCode]++;
            }
        }

        #endregion
    }
}
