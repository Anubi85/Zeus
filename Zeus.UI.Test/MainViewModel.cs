using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Zeus.UI.Mvvm;
using Zeus.UI.Mvvm.Interfaces;

namespace Zeus.UI.Test
{
    class MainViewModel : ViewModelBase
    {
        public ICommand OpenDialogCommand { get; private set; }

        private void OpenDialog(object owner)
        {
            ServiceLocator.Resolve<IDialogService>().ShowModalDialog(new DialogViewModel(), owner as Window, (vm) => Console.WriteLine("Callback"), (res) => res == DialogResult.Success);
        }

        public MainViewModel()
        {
            OpenDialogCommand = new RelayCommand(OpenDialog);
        }
    }
}
