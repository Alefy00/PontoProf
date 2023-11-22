using Ponto.Configs;

namespace Ponto
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new Login();
            //MainPage = new NavigationPage(new Login());
            MainPage = new AppFlyout();
        }
    }
}