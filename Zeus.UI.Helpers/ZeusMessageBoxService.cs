using System;
using System.Windows;
using Zeus.UI.Mvvm.Interfaces;

namespace Zeus.UI.Mvvm
{
    /// <summary>
    /// Provides common message box functionalities.
    /// </summary>
    public class ZeusMessageBoxService : IMessageBoxService
    {
        #region IMessageBoxService interface

        /// <summary>
        /// Shows an information dialog.
        /// </summary>
        /// <param name="text">The information text that will be shown.</param>
        public void ShowInfo(string text)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Shows a warning dialog.
        /// </summary>
        /// <param name="text">The warning text that will be shown.</param>
        public void ShowWarning(string text)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Shows an error dialog.
        /// </summary>
        /// <param name="text">The error text that will be shown.</param>
        public void ShowError(string text)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Shows a confirmation dialog.
        /// </summary>
        /// <param name="text">The confirmation text that will be shown.</param>
        public bool ShowConfirmation(string text)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
