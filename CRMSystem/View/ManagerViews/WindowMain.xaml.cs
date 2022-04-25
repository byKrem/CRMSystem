using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CRMSystem.View.ManagerViews
{
    public partial class WindowMain : Window
    {
        readonly Managers _currentManager;
        public DateTime StartupTime { get; private set; }
        public WindowMain(Managers manager)
        {
            InitializeComponent();
            StartupTime = DateTime.Now;
            _currentManager = manager;
            ManagerFoto.DataContext = _currentManager;
            if (manager?.Foto == null)
                ManagerFoto.Source = new BitmapImage(
                    new Uri(@"pack://application:,,,/CRMSystem;component/IMG/unknownImage.png"));
            MainFrame.Navigate(new OrdersFrame(_currentManager));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(MainFrame.Content.GetType() != typeof(OrdersFrame))
                MainFrame.Navigate(new OrdersFrame(_currentManager));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(MainFrame.Content.GetType() != typeof(ClientsFrame))
                MainFrame.Navigate(new ClientsFrame(_currentManager));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content.GetType() != typeof(StorageFrame))
                MainFrame.Navigate(new StorageFrame());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content.GetType() != typeof(PersonalAccountFrame))
                MainFrame.Navigate(new PersonalAccountFrame(this,_currentManager));
        }
    }
}
