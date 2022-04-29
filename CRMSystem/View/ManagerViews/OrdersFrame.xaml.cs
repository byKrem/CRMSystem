using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace CRMSystem.View.ManagerViews
{
    class OrderDetails
    {
        public Orders Order { get; set; }
        public decimal Cost { get; set; }
        public Users Customer { get; set; }
    }
    class Analytics
    {
        public decimal Cost { get; set; }
        public decimal AvarageCost { get; set; }
        public int InProcessCount { get; set; }
        public float InProcessPercent { get; set; }
        public int ClosedOrdersCount { get; set; }
        public float ClosedOrdersPercent { get; set; }
        public int NewOrdersCount { get; set; }
    }
    public partial class OrdersFrame : UserControl
    {
        private CRMSystemEntities DB;
        private List<OrderDetails> OrderDetails;
        private Analytics Analytics;
        private readonly Users _currentManager;
        public OrdersFrame(Users manager)
        {
            InitializeComponent();

            _currentManager = manager;

            DB = new CRMSystemEntities();
            OrderDetails = DB.ProductOrder.GroupBy(i => i.OrderId).Select(x => new OrderDetails
            {
                Order = DB.Orders.FirstOrDefault(i => i.Id == x.Key),
                Cost = x.Sum(s => s.Products.Price * s.ProductCount),
                Customer = DB.Orders.FirstOrDefault(c => c.Id == x.Key).Users
            }).Where(w => w.Customer.UserId == _currentManager.Id).ToList();
            grid.ItemsSource = OrderDetails;

            Analytics = new Analytics()
            {
                Cost = DB.ProductOrder.Where(w => w.Orders.OrderStatusId != 5 &&
                    w.Orders.Users.UserId == _currentManager.Id)
                    .Sum(s => s.Products.Price * s.ProductCount),

                AvarageCost = DB.ProductOrder.Where(w => w.Orders.OrderStatusId != 5 &&
                    w.Orders.Users.UserId == _currentManager.Id)
                    .Sum(s => s.Products.Price * s.ProductCount)
                    / DB.Orders.Where(w => w.Users.UserId == _currentManager.Id).Count(),

                InProcessCount = DB.Orders.Where(w => w.OrderStatusId != 6 && w.OrderStatusId != 5 &&
                    w.Users.UserId == _currentManager.Id).Count(),

                InProcessPercent = (float)DB.Orders.Where(w => w.OrderStatusId != 6 &&
                    w.OrderStatusId != 5 && w.Users.UserId == _currentManager.Id).Count()
                    / DB.Orders.Where(w => w.Users.UserId == _currentManager.Id).Count(),

                ClosedOrdersCount = DB.Orders.Where(w => w.OrderStatusId == 6 &&
                    w.Users.UserId == _currentManager.Id).Count(),

                ClosedOrdersPercent = (float)DB.Orders.Where(w => w.OrderStatusId == 6 &&
                   w.Users.UserId == _currentManager.Id).Count()
                    / DB.Orders.Where(w => w.Users.UserId == _currentManager.Id).Count(),

                NewOrdersCount = DB.Orders.Where(w => w.OrderStatusId == 1 &&
                    w.Users.UserId == _currentManager.Id).Count()
            };
            this.DataContext = Analytics;
        }

        private void grid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var order = (sender as DataGrid).SelectedItem as OrderDetails;
            new OrderDetailsWindow(DB.ProductOrder.Where(w => 
                w.OrderId == order.Order.Id).ToList()).ShowDialog();
        }

        private void InProcessBox_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(InProcessPercentBox.Visibility == System.Windows.Visibility.Visible)
            {
                InProcessPercentBox.Visibility = System.Windows.Visibility.Hidden;
                InProcessCountBox.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                InProcessPercentBox.Visibility = System.Windows.Visibility.Visible;
                InProcessCountBox.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void ClosedOrdersBox_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ClosedOrdersPercentBox.Visibility == System.Windows.Visibility.Visible)
            {
                ClosedOrdersPercentBox.Visibility = System.Windows.Visibility.Hidden;
                ClosedOrdersCountBox.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                ClosedOrdersPercentBox.Visibility = System.Windows.Visibility.Visible;
                ClosedOrdersCountBox.Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}
