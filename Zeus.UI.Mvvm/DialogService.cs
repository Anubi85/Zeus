using System;
using System.Collections.Generic;
using System.Windows;
using Zeus.Exceptions;
using Zeus.UI.Mvvm.Interfaces;

namespace Zeus.UI.Mvvm
{
    /// <summary>
    /// Provides common dialog functionalities.
    /// </summary>
    public class DialogService : IDialogService
    {
        #region Helper struct

        /// <summary>
        /// Stores information about a window opened by the <see cref="DialogService"/>.
        /// </summary>
        private struct WindowRecord
        {
            #region Fields

            /// <summary>
            /// Gets or sets a flag that indicates if the window associated wit the record is modal.
            /// </summary>
            public bool IsModal;
            /// <summary>
            /// The <see cref="Window"/> object.
            /// </summary>
            public Window Dialog;

            #endregion

            #region Constructor

            /// <summary>
            /// Initialize a new <see cref="WindowRecord"/> with the given data.
            /// </summary>
            /// <param name="isModal">The is window modal flag.</param>
            /// <param name="dialog">The window instance.</param>
            public WindowRecord(bool isModal, Window dialog)
            {
                IsModal = isModal;
                Dialog = dialog;
            }

            #endregion
        }

        #endregion

        #region Fields

        /// <summary>
        /// The dictionary that stores the association between the view models and the views.
        /// </summary>
        private Dictionary<Type, Type> m_ViewStore;
        /// <summary>
        /// The dictionary that stores the open wiews.
        /// </summary>
        private Dictionary<ViewModelBase, WindowRecord> m_ViewInstanceStore;
        /// <summary>
        /// The dictionary that stores the result for an open view.
        /// </summary>
        private Dictionary<ViewModelBase, bool?> m_ViewInstaceExecuteCallbackStore;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize the class fields.
        /// </summary>
        public DialogService()
        {
            m_ViewStore = new Dictionary<Type, Type>();
            m_ViewInstanceStore = new Dictionary<ViewModelBase, WindowRecord>();
            m_ViewInstaceExecuteCallbackStore = new Dictionary<ViewModelBase, bool?>();
        }

        #endregion

        #region IDialogService interface

        /// <summary>
        /// Shows a new modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="owner">The owner window of the dialog.</param>
        /// <param name="modal">A flag that indicates if the dialog has to be shown as modal.</param>
        /// <param name="callback">Callback action that will be execute after the dialog as been colosed.</param>
        /// <param name="canExecuteCallback">A function that specify when the <paramref name="callback"/> action shall be executed.</param>
        /// <returns>A <see cref="DialogResult"/> value that specified whether the activity was accepted (true) or cancelled(false).</returns>
        private DialogResult ShowDialog(ViewModelBase viewModel, Window owner, bool modal, Action<ViewModelBase> callback, Func<DialogResult, bool> canExecuteCallback)
        {
            //check if already registred
            if (!m_ViewStore.ContainsKey(viewModel.GetType()))
            {
                throw new ZeusException(ErrorCodes.RegistrationDoNotExists, string.Format("The view model {0} is not registered.", viewModel.GetType().FullName));
            }
            Window dialog = Activator.CreateInstance(m_ViewStore[viewModel.GetType()]) as Window;
            //register the opened window
            m_ViewInstanceStore.Add(viewModel, new WindowRecord(modal, dialog));
            m_ViewInstaceExecuteCallbackStore.Add(viewModel, null);
            if (owner != null)
            {
                dialog.Owner = owner;
            }
            dialog.DataContext = viewModel;
            DialogResult result = DialogResult.Abort;
            Action<DialogResult, bool?> performCallback = (res, exec) =>
            {
                if (canExecuteCallback?.Invoke(res) != false)
                {
                    if (!exec.HasValue || exec.Value)
                    {
                        callback?.Invoke(viewModel);
                    }
                }
            };
            if (modal)
            {
                dialog.ShowInTaskbar = false;
                result = dialog.ShowDialog();
                performCallback(result, m_ViewInstaceExecuteCallbackStore[viewModel]);
                //remove registration
                m_ViewInstanceStore.Remove(viewModel);
                m_ViewInstaceExecuteCallbackStore.Remove(viewModel);
            }
            else
            {
                dialog.Show();
                EventHandler closeEventHandler = null;
                closeEventHandler = (sender, e) =>
                {
                    Window wnd = (sender as Window);
                    wnd.Closed -= closeEventHandler;
                    performCallback(wnd.DialogResult, m_ViewInstaceExecuteCallbackStore[viewModel]);
                    //remove registration
                    m_ViewInstanceStore.Remove(viewModel);
                    m_ViewInstaceExecuteCallbackStore.Remove(viewModel);
                };
                dialog.Closed += closeEventHandler;
            }
            return result;
        }
        /// <summary>
        /// Shows a new non modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        public void ShowDialog(ViewModelBase viewModel)
        {
            ShowDialog(viewModel, null, false, null, null);
        }
        /// <summary>
        /// Shows a new non modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="owner">The owner window of the dialog.</param>
        public void ShowDialog(ViewModelBase viewModel, Window owner)
        {
            ShowDialog(viewModel, owner, false, null, null);
        }
        /// <summary>
        /// Shows a new non modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="callback">Callback action that will be execute after the dialog as been colosed.</param>
        public void ShowDialog(ViewModelBase viewModel, Action<ViewModelBase> callback)
        {
            ShowDialog(viewModel, null, false, callback, null);
        }
        /// <summary>
        /// Shows a new non modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="callback">Callback action that will be execute after the dialog as been colosed.</param>
        /// <param name="canExecuteCallback">A function that specify when the <paramref name="callback"/> action shall be executed.</param>
        public void ShowDialog(ViewModelBase viewModel, Action<ViewModelBase> callback, Func<DialogResult, bool> canExecuteCallback)
        {
            ShowDialog(viewModel, null, false, callback, canExecuteCallback);
        }
        /// <summary>
        /// Shows a new non modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="owner">The owner window of the dialog.</param>
        /// <param name="callback">Callback action that will be execute after the dialog as been colosed.</param>
        public void ShowDialog(ViewModelBase viewModel, Window owner, Action<ViewModelBase> callback)
        {
            ShowDialog(viewModel, owner, false, callback, null);
        }
        /// <summary>
        /// Shows a new non modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>        
        /// <param name="owner">The owner window of the dialog.</param>
        /// <param name="callback">Callback action that will be execute after the dialog as been colosed.</param>
        /// <param name="canExecuteCallback">A function that specify when the <paramref name="callback"/> action shall be executed.</param>
        public void ShowDialog(ViewModelBase viewModel, Window owner, Action<ViewModelBase> callback, Func<DialogResult, bool> canExecuteCallback)
        {
            ShowDialog(viewModel, owner, false, callback, canExecuteCallback);
        }
        /// <summary>
        /// Shows a new modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <returns>A <see cref="Nullable"/> value that specified whether the activity was accepted (true) or cancelled(false).</returns>
        public DialogResult ShowModalDialog(ViewModelBase viewModel)
        {
            return ShowDialog(viewModel, null, true, null, null);
        }
        /// <summary>
        /// Shows a new modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="callback">Callback action that will be execute after the dialog as been colosed.</param>
        /// <returns>A <see cref="Nullable"/> value that specified whether the activity was accepted (true) or cancelled(false).</returns>
        public DialogResult ShowModalDialog(ViewModelBase viewModel, Action<ViewModelBase> callback)
        {
            return ShowDialog(viewModel, null, true, callback, null);
        }
        /// <summary>
        /// Shows a new modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="callback">Callback action that will be execute after the dialog as been colosed.</param>
        /// <param name="canExecuteCallback">A function that specify when the <paramref name="callback"/> action shall be executed.</param>
        /// <returns>A <see cref="Nullable"/> value that specified whether the activity was accepted (true) or cancelled(false).</returns>
        public DialogResult ShowModalDialog(ViewModelBase viewModel, Action<ViewModelBase> callback, Func<DialogResult, bool> canExecuteCallback)
        {
            return ShowDialog(viewModel, null, true, callback, canExecuteCallback);
        }
        /// <summary>
        /// Shows a new modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="owner">The owner window of the dialog.</param>
        /// <returns>A <see cref="Nullable"/> value that specified whether the activity was accepted (true) or cancelled(false).</returns>
        public DialogResult ShowModalDialog(ViewModelBase viewModel, Window owner)
        {
            return ShowDialog(viewModel, owner, true, null, null);
        }
        /// <summary>
        /// Shows a new modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="owner">The owner window of the dialog.</param>
        /// <param name="callback">Callback action that will be execute after the dialog as been colosed.</param>
        /// <returns>A <see cref="Nullable"/> value that specified whether the activity was accepted (true) or cancelled(false).</returns>
        public DialogResult ShowModalDialog(ViewModelBase viewModel, Window owner, Action<ViewModelBase> callback)
        {
            return ShowDialog(viewModel, owner, true, callback, null);
        }
        /// <summary>
        /// Shows a new modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="owner">The owner window of the dialog.</param>
        /// <param name="callback">Callback action that will be execute after the dialog as been colosed.</param>
        /// <param name="canExecuteCallback">A function that specify when the <paramref name="callback"/> action shall be executed.</param>
        /// <returns>A <see cref="Nullable"/> value that specified whether the activity was accepted (true) or cancelled(false).</returns>
        public DialogResult ShowModalDialog(ViewModelBase viewModel, Window owner, Action<ViewModelBase> callback, Func<DialogResult, bool> canExecuteCallback)
        {
            return ShowDialog(viewModel, owner, true, callback, canExecuteCallback);
        }
        /// <summary>
        /// Closes the dialog associated with the given view model.
        /// </summary>
        /// <param name="viewModel">The view model associated with the dialog that has to be closed.</param>
        /// <param name="dialogResult">The result that has to be associated with the dialog.</param>
        /// <param name="executeCallback">A flag that indicates if the callback action shall be executed.</param>
        private void CloseDialog(ViewModelBase viewModel, DialogResult dialogResult, bool? executeCallback)
        {
            //check if registrered
            if (m_ViewInstanceStore.ContainsKey(viewModel))
            {
                WindowRecord record = m_ViewInstanceStore[viewModel];
                if (record.IsModal)
                {
                    record.Dialog.DialogResult = dialogResult;
                }
                m_ViewInstaceExecuteCallbackStore[viewModel] = executeCallback;
                record.Dialog.Close();
            }
        }
        /// <summary>
        /// Closes the dialog associated with the given view model.
        /// </summary>
        /// <param name="viewModel">The view model associated with the dialog that has to be closed.</param>
        public void CloseDialog(ViewModelBase viewModel)
        {
            CloseDialog(viewModel, DialogResult.Abort, null);
        }
        /// <summary>
        /// Closes the dialog associated with the given view model.
        /// </summary>
        /// <param name="viewModel">The view model associated with the dialog that has to be closed.</param>
        /// <param name="executeCallback">A flag that indicates if the callback action shall be executed.</param>
        public void CloseDialog(ViewModelBase viewModel, bool executeCallback)
        {
            CloseDialog(viewModel, DialogResult.Abort, (bool?)executeCallback);
        }
        /// <summary>
        /// Closes the dialog associated with the given view model.
        /// </summary>
        /// <param name="viewModel">The view model associated with the dialog that has to be closed.</param>
        /// <param name="dialogResult">The result that has to be associated with the dialog.</param>
        public void CloseDialog(ViewModelBase viewModel, DialogResult dialogResult)
        {
            CloseDialog(viewModel, dialogResult, null);
        }
        /// <summary>
        /// Closes the dialog associated with the given view model.
        /// </summary>
        /// <param name="viewModel">The view model associated with the dialog that has to be closed.</param>
        /// <param name="dialogResult">The result that has to be associated with the dialog.</param>
        /// <param name="executeCallback">A flag that indicates if the callback action shall be executed.</param>
        public void CloseDialog(ViewModelBase viewModel, DialogResult dialogResult, bool executeCallback)
        {
            CloseDialog(viewModel, dialogResult, (bool?)executeCallback);
        }
        /// <summary>
        /// Register a view for a specific view model type.
        /// </summary>
        /// <typeparam name="TView">The view type.</typeparam>
        /// <typeparam name="TViewModel">The view model type.</typeparam>
        public void Register<TView, TViewModel>()
            where TView : Window
            where TViewModel : ViewModelBase
        {
            //check if already registred
            if (m_ViewStore.ContainsKey(typeof(TViewModel)))
            {
                throw new ZeusException(ErrorCodes.RegistrationAlreadyExists, string.Format("The view {0} is already registed with the view model {1}", typeof(TView).FullName, typeof(TViewModel).FullName));
            }
            m_ViewStore.Add(typeof(TViewModel), typeof(TView));
        }
        /// <summary>
        /// Update a registered view for a specific view model type.
        /// </summary>
        /// <typeparam name="TView">The view type.</typeparam>
        /// <typeparam name="TViewModel">The view model type.</typeparam>
        public void Update<TView, TViewModel>()
            where TView : Window
            where TViewModel : ViewModelBase
        {
            //check if already registred
            if (!m_ViewStore.ContainsKey(typeof(TViewModel)))
            {
                throw new ZeusException(ErrorCodes.RegistrationDoNotExists, string.Format("The view model {0} is not registered.", typeof(TViewModel).FullName));
            }
            m_ViewStore[typeof(TViewModel)] = typeof(TView);
        }
        /// <summary>
        /// Register or update a view for a specific view model type.
        /// </summary>
        /// <typeparam name="TView">The view type.</typeparam>
        /// <typeparam name="TViewModel">The view model type.</typeparam>
        public void RegisterOrUpdate<TView, TViewModel>()
            where TView : Window
            where TViewModel : ViewModelBase
        {
            //check if already registred
            if (m_ViewStore.ContainsKey(typeof(TViewModel)))
            {
                Update<TView, TViewModel>();
            }
            else
            {
                Register<TView, TViewModel>();
            }
        }
        /// <summary>
        /// Unregiser a view for a specific view model type.
        /// </summary>
        /// <typeparam name="TView">The view type.</typeparam>
        /// <typeparam name="TViewModel">The view model type.</typeparam>
        public void Unregister<TView, TViewModel>()
            where TView : Window
            where TViewModel : ViewModelBase
        {
            //check if already registred
            if (!m_ViewStore.ContainsKey(typeof(TViewModel)))
            {
                throw new ZeusException(ErrorCodes.RegistrationDoNotExists, string.Format("The view model {0} is not registered.", typeof(TViewModel).FullName));
            }
            m_ViewStore.Remove(typeof(TViewModel));
        }

        #endregion
    }
}
