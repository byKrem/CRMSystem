using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CRMSystem.Views.ManagerViews
{
    public partial class OrderDetailsWindow : Window
    {
        private Orders _currentOrder;
        public OrderDetailsWindow(List<ProductOrder> orderDetails)
        {
            InitializeComponent();
            _currentOrder = orderDetails[0].Orders;
            ProductsListDataGrid.ItemsSource = orderDetails;
            CustomerInfoGroupBox.DataContext = _currentOrder.Users;
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
                DB.Orders.First(f => f.Id == _currentOrder.Id).OrderStatusId = OrderStatusComboBox.SelectedIndex + 1;
                DB.SaveChanges();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
