using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CRMSystem.View.ManagerViews
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
            ProductsListDataGrid.ItemsSource = orderDetails;
            CustomerInfoGroupBox.DataContext = _currentOrder.Customers;
            OrderInfoGroupBox.DataContext = _currentOrder;
            using (CRMSystemEntities DB = new CRMSystemEntities())
            {
                OrderStatusComboBox.ItemsSource = DB.OrderStatus.ToList();
            }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            using (CRMSystemEntities DB = new CRMSystemEntities())
            {
                _currentOrder.OrderStatusId = OrderStatusComboBox.SelectedIndex+1;
                _currentOrder.OrderStatus = OrderStatusComboBox.SelectedItem as OrderStatus;
                DB.Orders.Append(_currentOrder);
                DB.SaveChanges();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
