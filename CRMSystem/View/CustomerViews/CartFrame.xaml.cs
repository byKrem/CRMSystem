using CRMSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace CRMSystem.Views.CustomerViews
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
            if (productsEx.Count == 1) return;
            _customerWindow.Cart.Remove(productsEx.Product);
            productsEx.Count--;
        }

        private void AddProduct_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ProductsEx productsEx = (sender as TextBlock).DataContext as ProductsEx;
            if (productsEx.Count == 99 ||
                productsEx.Product.Count == productsEx.Count) return;
            _customerWindow.Cart.Add(productsEx.Product);
            productsEx.Count++;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CRMSystemEntities DB = new CRMSystemEntities();
            Orders order = new Orders
            {
                Id = DB.Orders.ToList().Last().Id + 1,
                CreationDate = DateTime.Now,
                InvoiceNumber = GenerateNewInvoiceNumber(),
                OrderStatusId = 1,
                OrderStatus = DB.OrderStatus.First(f => f.Id == 1),
                UserId = DB.Users.Find(_customerWindow.CurrentCustomer.Id).Id,
                Users = DB.Users.Find(_customerWindow.CurrentCustomer.Id)
            };
            DB.Orders.Add(order);
            foreach(var item in _customerWindow.Cart.GroupBy(g => g.Id))
            {
                DB.ProductOrder.Add(new ProductOrder
                {
                    Orders = order,
                    Products = DB.Products.Where(w => w.Id == item.Key).First(),
                    ProductCount =
                    _customerWindow.Cart.Where(w => w.Id == item.Key).Count()
                });
                DB.Products.Find(item.Key).Count -= _customerWindow.Cart.Where(w => w.Id == item.Key).Count();
            }
            DB.SaveChanges();
            _customerWindow.Cart.Clear();
            Listing.ItemsSource = null;
        }

        private string GenerateNewInvoiceNumber()
        {
            CRMSystemEntities DB = new CRMSystemEntities();
            Random rnd = new Random();
            char[] alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЫЭЮЯ".ToCharArray();
            string invoice = "";
            
            do
            {
                invoice = $"{alphabet[rnd.Next(0, alphabet.Length)]}-{rnd.Next(1, 99999999):00000000}";
            } while (DB.Orders.Where(w => w.InvoiceNumber == invoice).Count() == 1);
            
            return invoice;
        }
    }

    internal class ProductsEx : INotifyPropertyChanged
    {
        private Products _product;
        private int _count;
        private decimal _totalPrice;
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
                TotalPrice = _count * _product.Price;
                OnPropertyChanged();
            }
        }
        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set
            {
                _totalPrice = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
