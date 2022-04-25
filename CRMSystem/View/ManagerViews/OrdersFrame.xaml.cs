﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace CRMSystem.View.ManagerViews
{
    class OrderDetails
    {
        public Orders Order { get; set; }
        public decimal Cost { get; set; }
        public Customers Customer { get; set; }
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
        private DateTime delta;
        private CRMSystemEntities DB;
        private List<OrderDetails> OrderDetails;
        private Analytics Analytics;
        private readonly Managers _currentManager;
        public OrdersFrame(Managers manager)
        {
            InitializeComponent();

            _currentManager = manager;

            DB = new CRMSystemEntities();
            OrderDetails = DB.ProductOrder.GroupBy(i => i.OrderId).Select(x => new OrderDetails
            {
                Order = DB.Orders.FirstOrDefault(i => i.Id == x.Key),
                Cost = x.Sum(s => s.Products.Price * s.ProductCount),
                Customer = DB.Orders.FirstOrDefault(c => c.Id == x.Key).Customers
            }).Where(w => w.Customer.ManagerId == _currentManager.Id).ToList();
            grid.ItemsSource = OrderDetails;

            Analytics = new Analytics()
            {
                Cost = DB.ProductOrder.Where(w => w.Orders.OrderStatusId != 5 &&
                    w.Orders.Customers.ManagerId == _currentManager.Id)
                    .Sum(s => s.Products.Price * s.ProductCount),

                AvarageCost = DB.ProductOrder.Where(w => w.Orders.OrderStatusId != 5 &&
                    w.Orders.Customers.ManagerId == _currentManager.Id)
                    .Sum(s => s.Products.Price * s.ProductCount)
                    / DB.Orders.Where(w => w.Customers.ManagerId == _currentManager.Id).Count(),

                InProcessCount = DB.Orders.Where(w => w.OrderStatusId != 6 && w.OrderStatusId != 5 &&
                    w.Customers.ManagerId == _currentManager.Id).Count(),

                InProcessPercent = (float)DB.Orders.Where(w => w.OrderStatusId != 6 &&
                    w.OrderStatusId != 5 && w.Customers.ManagerId == _currentManager.Id).Count()
                    / DB.Orders.Where(w => w.Customers.ManagerId == _currentManager.Id).Count(),

                ClosedOrdersCount = DB.Orders.Where(w => w.OrderStatusId == 6 &&
                    w.Customers.ManagerId == _currentManager.Id).Count(),

                ClosedOrdersPercent = (float)DB.Orders.Where(w => w.OrderStatusId == 6 &&
                   w.Customers.ManagerId == _currentManager.Id).Count()
                    / DB.Orders.Where(w => w.Customers.ManagerId == _currentManager.Id).Count(),

                NewOrdersCount = DB.Orders.Where(w => w.OrderStatusId == 1 &&
                    w.Customers.ManagerId == _currentManager.Id).Count()
            };
            this.DataContext = Analytics;
        }

        private void Listing_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DateTime.Now - delta <= TimeSpan.FromMilliseconds(400))
            {
                var t = (sender as Grid).DataContext as OrderDetails;
            }
            delta = DateTime.Now;
        }

        private void grid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //(sender as DataGrid).
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