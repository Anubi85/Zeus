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
        /// <param name="canExecuteCallback">A function that specify when the <paramref name="callback"/> action shall be executed.</param>
        void ShowDialog(ViewModelBase viewModel, Action<ViewModelBase> callback, Func<DialogResult, bool> canExecuteCallback);
        /// <summary>
        /// Shows a new non modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>        
        /// <param name="owner">The owner window of the dialog.</param>
        /// <param name="callback">Callback action that will be execute after the dialog as been colosed.</param>
        void ShowDialog(ViewModelBase viewModel, Window owner, Action<ViewModelBase> callback);
        /// <summary>
        /// Shows a new non modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>        
        /// <param name="owner">The owner window of the dialog.</param>
        /// <param name="callback">Callback action that will be execute after the dialog as been colosed.</param>
        /// <param name="canExecuteCallback">A function that specify when the <paramref name="callback"/> action shall be executed.</param>
        void ShowDialog(ViewModelBase viewModel, Window owner, Action<ViewModelBase> callback, Func<DialogResult, bool> canExecuteCallback);
        /// <summary>
        /// Shows a new modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <returns>A <see cref="DialogResult"/> value that specified whether the activity was accepted (true) or cancelled(false).</returns>
        DialogResult ShowModalDialog(ViewModelBase viewModel);
        /// <summary>
        /// Shows a new modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="callback">Callback action that will be execute after the dialog as been colosed.</param>
        /// <returns>A <see cref="DialogResult"/> value that specified whether the activity was accepted (true) or cancelled(false).</returns>
        DialogResult ShowModalDialog(ViewModelBase viewModel, Action<ViewModelBase> callback);
        /// <summary>
        /// Shows a new modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="callback">Callback action that will be execute after the dialog as been colosed.</param>
        /// <param name="canExecuteCallback">A function that specify when the <paramref name="callback"/> action shall be executed.</param>
        /// <returns>A <see cref="DialogResult"/> value that specified whether the activity was accepted (true) or cancelled(false).</returns>
        DialogResult ShowModalDialog(ViewModelBase viewModel, Action<ViewModelBase> callback, Func<DialogResult, bool> canExecuteCallback);
        /// <summary>
        /// Shows a new modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="owner">The owner window of the dialog.</param>
        /// <returns>A <see cref="DialogResult"/> value that specified whether the activity was accepted (true) or cancelled(false).</returns>
        DialogResult ShowModalDialog(ViewModelBase viewModel, Window owner);
        /// <summary>
        /// Shows a new modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="owner">The owner window of the dialog.</param>
        /// <param name="callback">Callback action that will be execute after the dialog as been colosed.</param>
        /// <returns>A <see cref="DialogResult"/> value that specified whether the activity was accepted (true) or cancelled(false).</returns>
        DialogResult ShowModalDialog(ViewModelBase viewModel, Window owner, Action<ViewModelBase> callback);
        /// <summary>
        /// Shows a new modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="owner">The owner window of the dialog.</param>
        /// <param name="callback">Callback action that will be execute after the dialog as been colosed.</param>
        /// <param name="canExecuteCallback">A function that specify when the <paramref name="callback"/> action shall be executed.</param>
        /// <returns>A <see cref="DialogResult"/> value that specified whether the activity was accepted (true) or cancelled(false).</returns>
        DialogResult ShowModalDialog(ViewModelBase viewModel, Window owner, Action<ViewModelBase> callback, Func<DialogResult, bool> canExecuteCallback);
        /// <summary>
        /// Closes the dialog associated with the given view model.
        /// </summary>
        /// <param name="viewModel">The view model associated with the dialog that has to be closed.</param>
        void CloseDialog(ViewModelBase viewModel);
        /// <summary>
        /// Closes the dialog associated with the given view model.
        /// </summary>
        /// <param name="viewModel">The view model associated with the dialog that has to be closed.</param>
        /// <param name="executeCallback">A flag that indicates if the callback action shall be executed.</param>
        void CloseDialog(ViewModelBase viewModel, bool executeCallback);
        /// <summary>
        /// Closes the dialog associated with the given view model.
        /// </summary>
        /// <param name="viewModel">The view model associated with the dialog that has to be closed.</param>
        /// <param name="dialogResult">The result that has to be associated with the dialog.</param>
        void CloseDialog(ViewModelBase viewModel, DialogResult dialogResult);
        /// <summary>
        /// Closes the dialog associated with the given view model.
        /// </summary>
        /// <param name="viewModel">The view model associated with the dialog that has to be closed.</param>
        /// <param name="dialogResult">The result that has to be associated with the dialog.</param>
        /// <param name="executeCallback">A flag that indicates if the callback action shall be executed.</param>
        void CloseDialog(ViewModelBase viewModel, DialogResult dialogResult, bool executeCallback);
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
