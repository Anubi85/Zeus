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
        /// Shows a new non modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="owner">The owner window of the dialog.</param>
        void ShowDialog(ViewModelBase viewModel, Window owner);
        /// <summary>
        /// Shows a new non modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="callback">Callback action that will be execute after the dialog as been colosed.</param>
        void ShowDialog(ViewModelBase viewModel, Action<ViewModelBase> callback);
        /// <summary>
        /// Shows a new non modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="callback">Callback action that will be execute after the dialog as been colosed.</param>
        /// <param name="owner">The owner window of the dialog.</param>
        void ShowDialog(ViewModelBase viewModel, Window owner, Action<ViewModelBase> callback);
        /// <summary>
        /// Shows a new modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <returns>A <see cref="Nullable"/> <see cref="bool"/> value hat specified whether the activity was accepted (true) or cancelled(false).</returns>
        bool? ShowModalDialog(ViewModelBase viewModel);
        /// <summary>
        /// Shows a new modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="owner">The owner window of the dialog.</param>
        /// <returns>A <see cref="Nullable"/> <see cref="bool"/> value hat specified whether the activity was accepted (true) or cancelled(false).</returns>
        bool? ShowModalDialog(ViewModelBase viewModel, Window owner);
        /// <summary>
        /// Closes the dialog associated with the given view model.
        /// </summary>
        /// <param name="viewModel">The view model associated with the dialog that has to be closed.</param>
        void CloseDialog(ViewModelBase viewModel);
        /// <summary>
        /// Closes the dialog associated with the given view model.
        /// </summary>
        /// <param name="viewModel">The view model associated with the dialog that has to be closed.</param>
        /// <param name="dialogResult">The result that has to be associated with the dialog.</param>
        void CloseDialog(ViewModelBase viewModel, bool? dialogResult);
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
