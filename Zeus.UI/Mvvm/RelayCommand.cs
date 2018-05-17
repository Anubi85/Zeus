using System;
using System.Windows.Input;

namespace Zeus.UI.Mvvm
{
    /// <summary>
    /// Represents a command object that can be used in databinding.
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Fields

        /// <summary>
        /// The action that has to be executed.
        /// </summary>
        private Action<object> m_ExecuteWithParam;
        /// <summary>
        /// The method that determine if the action can be executed.
        /// </summary>
        private Func<object, bool> m_canExecute;

        #endregion

        #region ICommand interface

        /// <summary>
        /// Event that occours when the can execute condition changes.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// Determine if the <see cref="Execute(object)"/> method can be executed.
        /// </summary>
        /// <param name="parameter">Object used to determine execute conditions.</param>
        /// <returns>True if can execute, false otherwise</returns>
        public bool CanExecute(object parameter)
        {
            return m_canExecute(parameter);
        }

        /// <summary>
        /// Execute the command action.
        /// </summary>
        /// <param name="parameter">Action parameter.</param>
        public void Execute(object parameter)
        {
            m_ExecuteWithParam(parameter);
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">Action that the command shall execute.</param>
        public RelayCommand(Action execute) : this(ToActionWithParam(execute), null as Func<object, bool>)
        {

        }
        /// <summary>
        /// Create a new instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">Action that the command shall execute.</param>
        /// <param name="canExecute">Function that determine if the command can be executed.</param>
        public RelayCommand(Action execute, Func<object, bool> canExecute) : this(ToActionWithParam(execute), canExecute)
        {

        }
        /// <summary>
        /// Create a new instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">Action that the command shall execute.</param>
        /// <param name="canExecute">Function that determine if the command can be executed.</param>
        public RelayCommand(Action execute, Func<bool> canExecute) : this(ToActionWithParam(execute), ToFuncWithParam(canExecute))
        {

        }
        /// <summary>
        /// Create a new instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">Action that the command shall execute.</param>
        public RelayCommand(Action<object> execute) : this(execute, null as Func<object, bool>)
        {

        }
        /// <summary>
        /// Create a new instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">Action that the command shall execute.</param>
        /// <param name="canExecute">Function that determine if the command can be executed.</param>
        public RelayCommand(Action<object> execute, Func<bool> canExecute) : this(execute, ToFuncWithParam(canExecute))
        {

        }
        /// <summary>
        /// Create a new instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">Action that the command shall execute.</param>
        /// <param name="canExecute">Function that determine if the command can be executed.</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            m_ExecuteWithParam = execute;
            m_canExecute = canExecute ?? ((obj) => true);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts an <see cref="Action"/> into an <see cref="Action{T}"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> object that has to be converted.</param>
        /// <returns>The <see cref="Action{T}"/> converted object.</returns>
        private static Action<object> ToActionWithParam(Action action)
        {
            return (obj) => { action(); };
        }
        /// <summary>
        /// Converts a <see cref="Func{TResult}"/> into a <see cref="Func{T, TResult}"/>.
        /// </summary>
        /// <param name="func">The <see cref="Func{TResult}"/> object that has to be converted.</param>
        /// <returns>The <see cref="Func{T, TResult}"/> converted object.</returns>
        private static Func<object, bool> ToFuncWithParam(Func<bool> func)
        {
            return (obj) => { return func(); };
        }

        #endregion
    }
}
