using Zeus.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Zeus.Input.Hooks.Keyboard
{
    /// <summary>
    /// Hook for keyboard messages.
    /// </summary>
    public class KeyboardHook : IDisposable
    {
        #region IDisposable interface

        /// <summary>
        /// Stops the worker thread and remove the keyboard messages hook.
        /// </summary>
        public void Dispose()
        {
            m_HookQueue.Add(null);
            User32Helper.UnhookWindowsHookEx(m_HookID);
        }

        #endregion

        #region Fields

        /// <summary>
        /// Identifiers of the installed keyboard messages hook.
        /// </summary>
        private IntPtr m_HookID;
        /// <summary>
        /// Queue that contains the key messages to be processed.
        /// </summary>
        private BlockingCollection<KeyData> m_HookQueue;
        /// <summary>
        /// The thread that actually process the keyboard messages formatting the data and firing events.
        /// </summary>
        private Thread m_WorkerThread;
        /// <summary>
        /// Reference to the <see cref="User32Helper.LowLevelKeyboardProc"/> callback.
        /// </summary>
        private User32Helper.LowLevelKeyboardProc m_LowLevelKeyboardCallback;

        #endregion

        #region Methods

        /// <summary>
        /// This method runs in a separateed thread and takec care to process the generated keyboard messages firing
        /// <see cref="KeyStatusChanged"/> event according with defined <see cref="Filters"/>.
        /// </summary>
        private void Worker()
        {
            while (true)
            {
                KeyData data = m_HookQueue.Take();
                if (data == null)
                {
                    return;
                }
                data.FormatData();
                if ((Filters.Count(f => f.CanCombine) == 0 ||
                    Filters.Where(f => f.CanCombine).Any(f => f.Apply(data))) &&
                    Filters.Where(f => !f.CanCombine).All(f => f.Apply(data)))
                {
                    KeyStatusChanged?.Invoke(data);
                }
            }
        }

        /// <summary>
        /// Callback function called by the operating system when a keyboard message is processed.
        /// This function adds data relative to the keyboard message to the <see cref="m_HookQueue"/> queue.
        /// </summary>
        /// <param name="nCode">A code the hook procedure uses to determine how to process the message.
        /// If nCode is less than zero, the hook procedure must pass the message to the <see cref="CallNextHookEx(IntPtr, int, int, IntPtr)"/>
        /// function without further processing and should return the value returned by <see cref="CallNextHookEx(IntPtr, int, int, IntPtr)"/>.</param>
        /// <param name="wParam">The identifier of the keyboard message.</param>
        /// <param name="lParam">A pointer to a <see cref="KBDLLHOOKSTRUCT"/> structure.</param>
        /// <returns>If returns <see cref="IntPtr.Zero"/> other message hooks in the queue are not processed,
        /// otherwise processing continues normally.</returns>
        private IntPtr HookCallBack(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                KeyData data = new KeyData((User32Helper.WM)wParam.ToInt32(), lParam);
                m_HookQueue.Add(data);
            }
            return User32Helper.CallNextHookEx(m_HookID, nCode, wParam, lParam);
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of the <see cref="KeyboardHook"/> and initialize ith internal structures.
        /// </summary>
        public KeyboardHook()
        {
            Filters = new List<IKeyboardHookFilter>();
            m_HookQueue = new BlockingCollection<KeyData>();
            m_WorkerThread = new Thread(Worker);
            m_WorkerThread.Start();
            using (Process curProc = Process.GetCurrentProcess())
            {
                using (ProcessModule curModule = curProc.MainModule)
                {
                    m_LowLevelKeyboardCallback = new User32Helper.LowLevelKeyboardProc(HookCallBack);
                    m_HookID = User32Helper.SetWindowsHookEx(User32Helper.WH.KEYBOARD_LL, m_LowLevelKeyboardCallback, Kernel32Helper.GetModuleHandle(curModule.ModuleName), 0);
                }
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Events generated when keyboard messages matches the conditions defined by <see cref="Filters"/>.
        /// </summary>
        public event KeyboardHookEventHandler KeyStatusChanged;

        #endregion

        #region Delegates

        /// <summary>
        /// Delegate that handle the <see cref="KeyStatusChanged"/> event.
        /// </summary>
        /// <param name="e"><see cref="KeyData"/> associated to the key that generates the event.</param>
        public delegate void KeyboardHookEventHandler(KeyData e);

        #endregion

        #region Properties

        /// <summary>
        /// Gets a list of all the installed filters. If the list is empty all the keyboard mesaages are processed.
        /// </summary>
        public List<IKeyboardHookFilter> Filters { get; private set; }

        #endregion
    }
}

