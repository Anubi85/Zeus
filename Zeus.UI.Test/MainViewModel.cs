using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Zeus.UI.Mvvm;
using Zeus.UI.Mvvm.Interfaces;

namespace Zeus.UI.Test
{
    class MainViewModel : ViewModelBase
    {
        public ICommand OpenDialogCommand { get; private set; }

        private void OpenDialog()
        {
            ServiceLocator.Resolve<IDialogService>().ShowModalDialog(new DialogViewModel(), (vm) => Console.WriteLine("Callback"));
        }

        public MainViewModel()
        {
            OpenDialogCommand = new RelayCommand(OpenDialog);
        }
    }
}
