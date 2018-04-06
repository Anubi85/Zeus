using System.Collections.Generic;

namespace Zeus.Input.Hooks.Keyboard.Filters
{
    /// <summary>
    /// Makes <see cref="KeyboardHook"/> to fire <see cref="KeyboardHook.KeyStatusChanged"/> events only 
    /// if there is a math within the sequence of pressed key and the key sequence in <see cref="KeySequenceFilter.KeyCodesSequence"/>.
    /// </summary>
    public sealed class KeySequenceFilter : IKeyboardHookFilter
    {
        #region IKeyboardHookFilter interface        

        /// <summary>
        /// Apply the filter logic the the given <see cref="KeyData"/>.
        /// </summary>
        /// <param name="data">The data to wich the filter logic must be applied.</param>
        /// <returns>True if the <paramref name="data"/> satisfy the filter logic, false otherwise.</returns>
        public bool Apply(KeyData data)
        {
            if ((UseKeyDown ^ data.IsUp) && (UseKeyDown ? data.DownCounter == 0 : true))
            {
                if (KeyCodesSequence[m_KeyCodeIndex++] == data.KeyCode)
                {
                    if (m_KeyCodeIndex == KeyCodesSequence.Count)
                    {
                        m_KeyCodeIndex = 0;
                        return true;
                    }
                    return false;
                }
                m_KeyCodeIndex = KeyCodesSequence[0] == data.KeyCode ? 1 : 0;
            }
            return false;
        }

        /// <summary>
        /// Gers a flag that indicates if the filter can be combined with other filters.
        /// </summary>
        public bool CanCombine
        {
            get { return true; }
        }

        #endregion

        #region Fields

        /// <summary>
        /// Index of the neext key in the sequence.
        /// </summary>
        private int m_KeyCodeIndex;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of <see cref="KeySequenceFilter"/> and initialize its internal fields.
        /// </summary>
        public KeySequenceFilter()
        {
            KeyCodesSequence = new List<int>();
            m_KeyCodeIndex = 0;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list with the sequence of key that must be matched.
        /// </summary>
        public List<int> KeyCodesSequence { get; private set; }

        /// <summary>
        /// Flag that indicates if perfomr the computation on key down or on key up events.
        /// </summary>
        public bool UseKeyDown { get; set; }

        #endregion
    }
}
