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

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize the class fields.
        /// </summary>
        public DialogService()
        {
            m_ViewStore = new Dictionary<Type, Type>();
            m_ViewInstanceStore = new Dictionary<ViewModelBase, WindowRecord>();
        }

        #endregion

        #region IDialogService interface

        /// <summary>
        /// Shows an information dialog.
        /// </summary>
        /// <param name="text">The information text that will be shown.</param>
        public void ShowInfo(string text)
        {
            MessageBox.Show(text, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        /// <summary>
        /// Shows a warning dialog.
        /// </summary>
        /// <param name="text">The warning text that will be shown.</param>
        public void ShowWarning(string text)
        {
            MessageBox.Show(text, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        /// <summary>
        /// Shows an error dialog.
        /// </summary>
        /// <param name="text">The error text that will be shown.</param>
        public void ShowError(string text)
        {
            MessageBox.Show(text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        /// <summary>
        /// Shows a confirmation dialog.
        /// </summary>
        /// <param name="text">The confirmation text that will be shown.</param>
        public bool ShowConfirmation(string text)
        {
            return MessageBox.Show(text, "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes;
        }
        /// <summary>
        /// Shows a new modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="owner">The owner window of the dialog.</param>
        /// <param name="modal">A flag that indicates if the dialog has to be shown as modal.</param>
        /// <param name="callback">Callback action that will be execute after the dialog as been colosed.</param>
        /// <returns>A <see cref="Nullable"/> <see cref="bool"/> value hat specified whether the activity was accepted (true) or cancelled(false).</returns>
        private bool? ShowDialog(ViewModelBase viewModel, Window owner, bool modal, Action<ViewModelBase> callback)
        {
            //check if already registred
            if (!m_ViewStore.ContainsKey(viewModel.GetType()))
            {
                throw new ZeusException(ErrorCodes.RegistrationDoNotExists, string.Format("The view model {0} is not registered.", viewModel.GetType().FullName));
            }
            Window dialog = Activator.CreateInstance(m_ViewStore[viewModel.GetType()]) as Window;
            //register the opened window
            m_ViewInstanceStore.Add(viewModel, new WindowRecord(modal, dialog));
            if (owner != null)
            {
                dialog.Owner = owner;
            }
            dialog.DataContext = viewModel;
            bool? result = null;
            if (modal)
            {
                result = dialog.ShowDialog();
                callback?.Invoke(viewModel);
                //remove registration
                m_ViewInstanceStore.Remove(viewModel);
            }
            else
            {
                dialog.Show();
                EventHandler closeEventHandler = null;
                closeEventHandler = (sender, e) =>
                {
                    m_ViewInstanceStore.Remove(viewModel);
                    (sender as Window).Closed -= closeEventHandler;
                    callback?.Invoke(viewModel);
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
            ShowDialog(viewModel, null, false, null);
        }
        /// <summary>
        /// Shows a new non modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="owner">The owner window of the dialog.</param>
        public void ShowDialog(ViewModelBase viewModel, Window owner)
        {
            ShowDialog(viewModel, owner, false, null);
        }
        /// <summary>
        /// Shows a new non modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="callback">Callback action that will be execute after the dialog as been colosed.</param>
        public void ShowDialog(ViewModelBase viewModel, Action<ViewModelBase> callback)
        {
            ShowDialog(viewModel, null, false, callback);
        }
        /// <summary>
        /// Shows a new non modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="owner">The owner window of the dialog.</param>
        /// <param name="callback">Callback action that will be execute after the dialog as been colosed.</param>
        public void ShowDialog(ViewModelBase viewModel, Window owner, Action<ViewModelBase> callback)
        {
            ShowDialog(viewModel, owner, false, callback);
        }
        /// <summary>
        /// Shows a new modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <returns>A <see cref="Nullable"/> <see cref="bool"/> value hat specified whether the activity was accepted (true) or cancelled(false).</returns>
        public bool? ShowModalDialog(ViewModelBase viewModel)
        {
            return ShowDialog(viewModel, null, true, null);
        }
        /// <summary>
        /// Shows a new modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <param name="owner">The owner window of the dialog.</param>
        /// <returns>A <see cref="Nullable"/> <see cref="bool"/> value hat specified whether the activity was accepted (true) or cancelled(false).</returns>
        public bool? ShowModalDialog(ViewModelBase viewModel, Window owner)
        {
            return ShowDialog(viewModel, owner, true, null);
        }
        /// <summary>
        /// Closes the dialog associated with the given view model.
        /// </summary>
        /// <param name="viewModel">The view model associated with the dialog that has to be closed.</param>
        public void CloseDialog(ViewModelBase viewModel)
        {
            CloseDialog(viewModel, null);
        }
        /// <summary>
        /// Closes the dialog associated with the given view model.
        /// </summary>
        /// <param name="viewModel">The view model associated with the dialog that has to be closed.</param>
        /// <param name="dialogResult">The result that has to be associated with the dialog.</param>
        public void CloseDialog(ViewModelBase viewModel, bool? dialogResult)
        {
            //check if registrered
            if (m_ViewInstanceStore.ContainsKey(viewModel))
            {
                WindowRecord record = m_ViewInstanceStore[viewModel];
                if (!record.IsModal)
                {
                    record.Dialog.DialogResult = dialogResult;
                }
                record.Dialog.Close();
            }
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
