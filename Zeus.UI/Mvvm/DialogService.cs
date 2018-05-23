using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        #region Fields

        /// <summary>
        /// The dictionary that stores the association between the view models and the views.
        /// </summary>
        private Dictionary<Type, Type> m_ViewStore;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize the class fields.
        /// </summary>
        public DialogService()
        {
            m_ViewStore = new Dictionary<Type, Type>();
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
        /// <param name="modal">A flag that indicates if the dialog has to be shown as modal.</param>
        /// <returns>A <see cref="Nullable"/> <see cref="bool"/> value hat specified whether the activity was accepted (true) or cancelled(false).</returns>
        private bool? ShowDialog(ViewModelBase viewModel, bool modal)
        {
            //check if already registred
            if (!m_ViewStore.ContainsKey(viewModel.GetType()))
            {
                throw new ZeusException(ErrorCodes.RegistrationDoNotExists, string.Format("The view model {0} is not registered.", viewModel.GetType().FullName));
            }
            Window dialog = Activator.CreateInstance(m_ViewStore[viewModel.GetType()]) as Window;
            dialog.DataContext = viewModel;
            if (modal)
            {
                return dialog.ShowDialog();
            }
            else
            {
                dialog.Show();
                return null;
            }
        }
        /// <summary>
        /// Shows a new non modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        public void ShowDialog(ViewModelBase viewModel)
        {
            ShowDialog(viewModel, false);
        }
        /// <summary>
        /// Shows a new modal dialog that binds to the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model of the dialog that has to be shown.</param>
        /// <returns>A <see cref="Nullable"/> <see cref="bool"/> value hat specified whether the activity was accepted (true) or cancelled(false).</returns>
        public bool? ShowModalDialog(ViewModelBase viewModel)
        {
            return ShowDialog(viewModel, true);
        }
        /// <summary>
        /// Register a view for a specific view model type.
        /// </summary>
        /// <typeparam name="TView">The view type.</typeparam>
        /// <typeparam name="TViewModel">The view model type.</typeparam>
        public void Register<TView, TViewModel>()
            where TView: Window
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
            where TView: Window
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
            where TView: Window
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
