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
    class DialogViewModel : ViewModelBase
    {
        public ICommand CloseDialogCommand { get; private set; }

        private void CloseDialog()
        {
            ServiceLocator.Resolve<IDialogService>().CloseDialog(this, true);
        }

        public DialogViewModel()
        {
            CloseDialogCommand = new RelayCommand(CloseDialog);
        }
    }
}
