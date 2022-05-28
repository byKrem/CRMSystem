using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CRMSystem.Views.CustomerViews
{
    /// <summary>
    /// Логика взаимодействия для OrderDetailsWindow.xaml
    /// </summary>
    public partial class OrderDetailsWindow : Window
    {
        private Orders _currentOrder;

        public OrderDetailsWindow(List<ProductOrder> orderDetails)
        {
            InitializeComponent();
            _currentOrder = orderDetails[0].Orders;
            DeclineOrderBtn.IsEnabled = _currentOrder.OrderStatusId != 5 && _currentOrder.OrderStatusId != 6;
            ProductsListDataGrid.ItemsSource = orderDetails;
            CustomerInfoGroupBox.DataContext = _currentOrder.Users;
            OrderInfoGroupBox.DataContext = _currentOrder;
        }

        private void DeclineOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Это действие не возможно отменить! Вы уверены?","Вы уверены?",MessageBoxButton.YesNo,MessageBoxImage.Question);

            if (result == MessageBoxResult.No) return;

            using (CRMSystemEntities DB = new CRMSystemEntities())
            {
                DB.Orders.First(f => f.Id == _currentOrder.Id).OrderStatusId = 5;
                DB.SaveChanges();
            }

            DeclineOrderBtn.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
