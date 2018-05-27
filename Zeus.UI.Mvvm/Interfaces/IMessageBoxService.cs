namespace Zeus.UI.Mvvm.Interfaces
{
    /// <summary>
    /// Provides the methods that a message box service must implement.
    /// </summary>
    public interface IMessageBoxService
    {
        /// <summary>
        /// Shows an information dialog.
        /// </summary>
        /// <param name="text">The information text that will be shown.</param>
        void ShowInfo(string text);
        /// <summary>
        /// Shows a warning dialog.
        /// </summary>
        /// <param name="text">The warning text that will be shown.</param>
        void ShowWarning(string text);
        /// <summary>
        /// Shows an error dialog.
        /// </summary>
        /// <param name="text">The error text that will be shown.</param>
        void ShowError(string text);
        /// <summary>
        /// Shows a confirmation dialog.
        /// </summary>
        /// <param name="text">The confirmation text that will be shown.</param>
        bool ShowConfirmation(string text);
    }
}
