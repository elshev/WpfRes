using System.Globalization;
using System.Windows.Controls;

namespace ResApp
{
    public partial class MainView
    {
        public MainView(IMainViewModel viewModel)
        {
            InitializeComponent();
            InitializeMenu();
            DataContext = viewModel;
        }

        private void InitializeMenu()
        {
            AddLanguageMenuItem("en-US");
            AddLanguageMenuItem("ru-RU");
        }

        private void AddLanguageMenuItem(string cultureName)
        {
            CultureInfo cultureInfo = new CultureInfo(cultureName);
            MenuItem menuItem = new MenuItem
            {
                Header = cultureInfo.NativeName,
                Tag = cultureInfo.TwoLetterISOLanguageName
            };
            menuItem.Click += (s, e) => ChangeLanguage(cultureInfo);
            LanguageMenuItem.Items.Add(menuItem);
        }

        private void ChangeLanguage(CultureInfo cultureInfo)
        {
            ViewModel.ChangeLanguage(cultureInfo);
        }

        public IMainViewModel ViewModel { get { return DataContext as IMainViewModel; } }
    }
}
