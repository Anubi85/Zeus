using System.Windows;
using Zeus.UI.Mvvm;
using Zeus.UI.Mvvm.Interfaces;

namespace Zeus.UI.Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            DialogService ds = new DialogService();
            ds.Register<Dialog, DialogViewModel>();
            ServiceLocator.Register<IDialogService>(ds);
            base.OnStartup(e);
        }
    }
}
