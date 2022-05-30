using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CRMSystem.Views.CustomerViews
{
    public class OrdersEx : Orders
    {
        public decimal Price { get; set; }
    }
    public partial class PersonalAccountFrame : Page
    {
        public Users Customer { get; }
        CRMSystemEntities DB;
        CustomerWindow windowMain;
        DateTime delta;
        public PersonalAccountFrame(CustomerWindow customerWindow, Users currentCustomer)
        {
            InitializeComponent();
            DB = new CRMSystemEntities();
            windowMain = customerWindow;
            Customer = currentCustomer;
            this.DataContext = Customer;
            OrdersGrid.ItemsSource = DB.Orders.Where(w => w.UserId == currentCustomer.Id)
                .Select(s => new OrdersEx
                {
                    Id = s.Id,
                    CreationDate = s.CreationDate,
                    OrderStatus = s.OrderStatus,
                    OrderStatusId = s.OrderStatusId,
                    Description = s.Description,
                    InvoiceNumber = s.InvoiceNumber,
                    Price = s.ProductOrder.Select(ss => ss.ProductCount + ss.Products.Price).Sum(),
                    ProductOrder = s.ProductOrder,
                    UserId = s.UserId,
                    Users = s.Users
                }).ToList();


        }
        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is Image)) return;

            Image image = sender as Image;
            if (Customer.Foto == null)
                image.Source = new BitmapImage(
                    new Uri(@"pack://application:,,,/CRMSystem;component/IMG/unknownImage.png"));
        }

        private void ButtonSaveEmailPhone_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(EmailBox.Text) && IsValidEmail(EmailBox.Text))
                Customer.Email = EmailBox.Text;

            if (!string.IsNullOrEmpty(PhoneBox.Text))
                Customer.Phone = long.Parse(PhoneBox.Text);

            DB.Users.Append(Customer);
            DB.SaveChanges();
        }
        private void grid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var order = (sender as DataGrid).SelectedItem as Orders;

            if (order == null) return;

            new OrderDetailsWindow(DB.ProductOrder.Where(w =>
                w.OrderId == order.Id && w.Orders.UserId == Customer.Id).ToList()).ShowDialog();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DateTime.Now - delta < TimeSpan.FromMilliseconds(500))
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.FileName = "";
                dlg.DefaultExt = ".png";
                dlg.Filter = "Image files (.png)|*.png;*.jpg;*.jpeg";

                if (dlg.ShowDialog() != true) return;

                byte[] imageBytes = null;
                using (var fs = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read))
                {
                    imageBytes = new byte[fs.Length];
                    fs.Read(imageBytes, 0, Convert.ToInt32(fs.Length));
                }
                DB.Users.First(f => f.Id == Customer.Id).Foto = imageBytes;
                DB.SaveChanges();
            }
            delta = DateTime.Now;
        }
        private bool IsValidEmail(string Email)
        {
            string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            Match isMatch = Regex.Match(Email.ToLower(), pattern, RegexOptions.IgnoreCase);
            return isMatch.Success;
        }

        private void PhoneBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string pattern = "[^0-9.-]+";
            Match isMatch = Regex.Match(e.Text, pattern, RegexOptions.IgnoreCase);
            if (isMatch.Success)
                e.Handled = true;
        }
    }
}
