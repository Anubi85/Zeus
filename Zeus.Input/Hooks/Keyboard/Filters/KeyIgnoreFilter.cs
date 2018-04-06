using System.Collections.Generic;

namespace Zeus.Input.Hooks.Keyboard.Filters
{
    /// <summary>
    /// Makes <see cref="KeyboardHook"/> to not fire <see cref="KeyboardHook.KeyStatusChanged"/> events
    /// only for keys defined in <see cref="KeyCodes"/>.
    /// </summary>
    public sealed class KeyIgnoreFilter : IKeyboardHookFilter
    {
        #region IKeyboardHookFilter interface

        /// <summary>
        /// Apply the filter logic the the given <see cref="KeyData"/>.
        /// </summary>
        /// <param name="data">The data to wich the filter logic must be applied.</param>
        /// <returns>True if the <paramref name="data"/> satisfy the filter logic, false otherwise.</returns>
        public bool Apply(KeyData data)
        {
            return !KeyCodes.Contains(data.KeyCode);
        }

        /// <summary>
        /// Gers a flag that indicates if the filter can be combined with other filters.
        /// </summary>
        public bool CanCombine
        {
            get { return false; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of <see cref="KeyIgnoreFilter"/> and initialize its internal structures.
        /// </summary>
        public KeyIgnoreFilter()
        {
            KeyCodes = new List<int>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of the keys for wich <see cref="KeyboardHook"/>
        /// must not generate the <see cref="KeyboardHook.KeyStatusChanged"/> event.
        /// </summary>
        public List<int> KeyCodes { get; private set; }

        #endregion
    }
}
