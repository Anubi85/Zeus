using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled dialog <see cref="Window"/>.
    /// </summary>
    public class ZeusDialog : Window
    {
        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusDialog), new FrameworkPropertyMetadata(typeof(ZeusDialog)));
        }

        /// <summary>
        /// Create a new instance of <see cref="ZeusDialog"/> and initialize its internal structures.
        /// </summary>
        public ZeusDialog()
        {
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, (sender, e) => SystemCommands.CloseWindow(this)));
            Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("/Zeus.UI;component/Styles/Zeus.xaml", UriKind.Relative) });
            CloseTrueCommand = new Mvvm.RelayCommand(() =>
            {
                DialogResult = true;
                Close();
            });
            CloseFalseCommand = new Mvvm.RelayCommand(() =>
            {
                DialogResult = false;
                Close();
            });
            CloseNullCommand = new Mvvm.RelayCommand(() =>
            {
                DialogResult = null;
                Close();
            });
            CloseCommand = new Mvvm.RelayCommand(Close);
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle window close button visibility.
        /// </summary>
        public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register("ShowCloseButton", typeof(bool), typeof(ZeusDialog));

        #endregion

        #region Properties

        /// <summary>
        /// Flag that handle window close button visibility.
        /// </summary>
        public bool ShowCloseButton
        {
            get { return (bool)GetValue(ShowCloseButtonProperty); }
            set { SetValue(ShowCloseButtonProperty, value); }
        }
        /// <summary>
        /// Gets a command that close the dialog and set the result to true.
        /// </summary>
        public ICommand CloseTrueCommand { get; private set; }
        /// <summary>
        /// Gets a command that close the dialog and set the result to false.
        /// </summary>
        public ICommand CloseFalseCommand { get; private set; }
        /// <summary>
        /// Gets a command that close the dialog and set the result to null.
        /// </summary>
        public ICommand CloseNullCommand { get; private set; }
        /// <summary>
        /// Gets a command that close the dialog without setting the result.
        /// </summary>
        public ICommand CloseCommand { get; private set; }

        #endregion
    }
}
