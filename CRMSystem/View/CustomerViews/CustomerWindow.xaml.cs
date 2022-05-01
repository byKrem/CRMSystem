using System.Windows;

namespace CRMSystem.View.CustomerViews
{
    public partial class CustomerWindow : Window
    {

        public CustomerWindow(Users customer)
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CatalogFrame());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
