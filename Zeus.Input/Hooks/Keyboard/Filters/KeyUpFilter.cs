﻿namespace Zeus.Input.Hooks.Keyboard.Filters
{
    /// <summary>
    /// Makes <see cref="KeyboardHook"/> to fire <see cref="KeyboardHook.KeyStatusChanged"/> events only if key is not pressed.
    /// </summary>
    public sealed class KeyUpFilter : IKeyboardHookFilter
    {
        #region IKeyboardHookFilter interface

        /// <summary>
        /// Apply the filter logic the the given <see cref="KeyData"/>.
        /// </summary>
        /// <param name="data">The data to wich the filter logic must be applied.</param>
        /// <returns>True if the <paramref name="data"/> satisfy the filter logic, false otherwise.</returns>
        public bool Apply(KeyData data)
        {
            return data.IsUp;
        }

        /// <summary>
        /// Gers a flag that indicates if the filter can be combined with other filters.
        /// </summary>
        public bool CanCombine
        {
            get { return true; }
        }

        #endregion
    }
}
