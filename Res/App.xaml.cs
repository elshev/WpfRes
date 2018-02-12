using System.Windows;
using ResApp.Resources;

namespace ResApp
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            InitializeApplication();
            var mainView = new MainView(new MainViewModel());
            mainView.Show();
        }

        private void InitializeApplication()
        {
            var appResRegistrator = new AppResRegistrator();
            appResRegistrator.Initialize();
        }
    }
}
