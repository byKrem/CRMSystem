using System;
using System.Windows;

namespace CRMSystem.View
{
    public partial class WindowMain : Window
    {

        public WindowMain()
        {
            InitializeComponent();
            MainFrame.Navigate(new OrdersFrame());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(MainFrame.Content.GetType() != typeof(OrdersFrame))
                MainFrame.Navigate(new OrdersFrame());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(MainFrame.Content.GetType() != typeof(ClientsFrame))
                MainFrame.Navigate(new ClientsFrame());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content.GetType() != typeof(StorageFrame))
                MainFrame.Navigate(new StorageFrame());
        }
    }
}
