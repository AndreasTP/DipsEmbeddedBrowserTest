using EmbeddedBrowserTest.ViewModels;
using System.Windows;

namespace EmbeddedBrowserTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainView = new MainView
            {
                DataContext = new MainViewModel()
            };

            MainWindow = mainView;
            MainWindow.Show();
        }
    }
}
