using CRMSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace CRMSystem.View.CustomerViews
{
    public partial class CartFrame : Page
    {
        private DateTime delta;
        private CustomerWindow _customerWindow;

        public CartFrame(CustomerWindow customerWindow)
        {
            InitializeComponent();
            _customerWindow = customerWindow;
            Listing.ItemsSource = customerWindow.Cart.GroupBy(g => g.Id).Select(s => new ProductsEx
            {
                Product = s.Where(w => w.Id == s.Key).FirstOrDefault(),
                Count = s.Where(w => w.Id == s.Key).Count()
            }).ToList();
        }

        private void DeleteFromCart_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(DateTime.Now - delta < TimeSpan.FromMilliseconds(500))
            {
                ProductsEx productsEx = (sender as TextBlock).DataContext as ProductsEx;
                _customerWindow.Cart.RemoveAll(p => p.Id == productsEx.Product.Id);

                Listing.ItemsSource = _customerWindow.Cart.GroupBy(g => g.Id).Select(s => new ProductsEx
                {
                    Product = s.Where(w => w.Id == s.Key).FirstOrDefault(),
                    Count = s.Where(w => w.Id == s.Key).Count()
                }).ToList();
            }
            delta = DateTime.Now;
        }

        private void LossProduct_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ProductsEx productsEx = (sender as TextBlock).DataContext as ProductsEx;
            _customerWindow.Cart.Remove(productsEx.Product);
            productsEx.Count--;
        }

        private void AddProduct_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ProductsEx productsEx = (sender as TextBlock).DataContext as ProductsEx;
            _customerWindow.Cart.Add(productsEx.Product);
            productsEx.Count++;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CRMSystemEntities DB = new CRMSystemEntities();
            Orders order = new Orders
            {
                CreationDate = DateTime.Now,
                InvoiceNumber = "",
                OrderStatusId = 1,
                OrderStatus = DB.OrderStatus.First(f => f.Id == 1),
                UserId = 1,
                Users = DB.Users.First(f => f.Id == 1)
            };
            DB.Orders.Add(order);
            List<ProductOrder> productOrder = new List<ProductOrder>
            {

            };
        }
    }

    internal class ProductsEx : INotifyPropertyChanged
    {
        private Products _product;
        private int _count;
        public Products Product
        { 
            get { return _product; }
            set
            {
                _product = value;
                OnPropertyChanged();
            }
        }
        public int Count 
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
