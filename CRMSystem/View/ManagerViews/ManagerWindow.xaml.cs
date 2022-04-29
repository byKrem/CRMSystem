using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CRMSystem.View.ManagerViews
{
    public partial class ManagerWindow : Window
    {
        public Users CurrentManager { get; private set; }
        public DateTime StartupTime { get; private set; }
        public ManagerWindow(Users manager)
        {
            InitializeComponent();
            StartupTime = DateTime.Now;
            CurrentManager = manager;
            if (manager?.Foto == null)
                ManagerFoto.Source = new BitmapImage(
                    new Uri(@"pack://application:,,,/CRMSystem;component/IMG/unknownImage.png"));
            MainFrame.Navigate(new OrdersFrame(CurrentManager));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(MainFrame.Content.GetType() != typeof(OrdersFrame))
                MainFrame.Navigate(new OrdersFrame(CurrentManager));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(MainFrame.Content.GetType() != typeof(ClientsFrame))
                MainFrame.Navigate(new ClientsFrame(CurrentManager));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content.GetType() != typeof(StorageFrame))
                MainFrame.Navigate(new StorageFrame());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content.GetType() != typeof(PersonalAccountFrame))
                MainFrame.Navigate(new PersonalAccountFrame(this,CurrentManager));
        }
    }
}
