namespace Zeus.Input.Hooks.Keyboard
{
    /// <summary>
    /// Defines methods and properties that a KeyboardHookFilter must implement.
    /// </summary>
    public interface IKeyboardHookFilter
    {
        /// <summary>
        /// Apply the filter logic the the given <see cref="KeyData"/>.
        /// </summary>
        /// <param name="data">The data to wich the filter logic must be applied.</param>
        /// <returns>True if the <paramref name="data"/> satisfy the filter logic, false otherwise.</returns>
        bool Apply(KeyData data);
        /// <summary>
        /// Gers a flag that indicates if the filter can be combined with other filters.
        /// <see cref="KeyboardHook"/> raise the <see cref="KeyboardHook.KeyStatusChanged"/> events only if
        /// al least one of the filters logic with <see cref="CanCombine"/> flag set is satisfied and all the filters logic
        /// with the <see cref="CanCombine"/> flag not set are satisfied.
        /// </summary>
        bool CanCombine { get; }
    }
}
