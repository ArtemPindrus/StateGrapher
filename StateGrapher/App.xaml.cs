using StateGrapher.Models;
using StateGrapher.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace StateGrapher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e) {
            MainViewModel mainViewModel = new MainViewModel();

            MainWindow mainWindow = new() {
                DataContext = mainViewModel
            };

            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
