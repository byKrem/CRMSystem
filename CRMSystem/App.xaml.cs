using System.Windows;

namespace CRMSystem
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            SplashScreen splash = new SplashScreen("\\IMG\\OrionTech.PNG");
            splash.Show(true);
        }
    }
}
