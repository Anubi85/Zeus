using System;
using System.Windows;

namespace Zeus.UI.Mvvm.Interfaces
{
    /// <summary>
    /// Provides the methods that a sialog service must implement.
    /// </summary>
    public interface IDialogService
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
        /// <summary>
        /// Shows a new non modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        void ShowDialog(ViewModelBase viewModel);
        /// <summary>
        /// Shows a new modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <returns>A <see cref="Nullable"/> <see cref="bool"/> value hat specified whether the activity was accepted (true) or cancelled(false).</returns>
        bool? ShowModalDialog(ViewModelBase viewModel);
        /// <summary>
        /// Register a view for a specific view model type.
        /// </summary>
        /// <typeparam name="TView">The view type.</typeparam>
        /// <typeparam name="TViewModel">The view model type.</typeparam>
        void Register<TView, TViewModel>() 
            where TView: Window 
            where TViewModel : ViewModelBase;
        /// <summary>
        /// Update a registered view for a specific view model type.
        /// </summary>
        /// <typeparam name="TView">The view type.</typeparam>
        /// <typeparam name="TViewModel">The view model type.</typeparam>
        void Update<TView, TViewModel>() 
            where TView: Window
            where TViewModel : ViewModelBase;
        /// <summary>
        /// Register or update a view for a specific view model type.
        /// </summary>
        /// <typeparam name="TView">The view type.</typeparam>
        /// <typeparam name="TViewModel">The view model type.</typeparam>
        void RegisterOrUpdate<TView, TViewModel>()
            where TView: Window
            where TViewModel : ViewModelBase;
        /// <summary>
        /// Unregiser a view for a specific view model type.
        /// </summary>
        /// <typeparam name="TView">The view type.</typeparam>
        /// <typeparam name="TViewModel">The view model type.</typeparam>
        void Unregister<TView, TViewModel>()
            where TView: Window
            where TViewModel : ViewModelBase;
    }
}
