using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CRMSystem.DataBase;

namespace CRMSystem.Views.ManagerViews
{
    class UsersEx
    {
        public Users User { get; set; }
        public double Buyout { get; set; }
    }
    public partial class ClientsFrame : UserControl
    {
        private readonly Users _currentManager;
        public ClientsFrame(Users manager)
        {
            InitializeComponent();

            _currentManager = manager;

            List<UsersEx> usersEx = DB.Context.Users.Where(w => w.UserId == _currentManager.Id).Select(s => new UsersEx
            {
                User = s,
                Buyout = (double) s.Orders.Where(w => w.OrderStatusId == 6
                && w.UserId == s.Id).Count() / s.Orders.Where(w => w.UserId == s.Id).Count(),
            }).ToList();

            grid.ItemsSource = usersEx;
        }

        private void grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // TODO: Открывать подробную информацию о клиенте + история его заказов
            Users user = ((sender as DataGrid).SelectedItem as UsersEx).User;

            if (user == null) return;

            List<OrderDetails> details = DB.Context.ProductOrder.GroupBy(i => i.OrderId).Select(x => new OrderDetails
            {
                Order = DB.Context.Orders.FirstOrDefault(i => i.Id == x.Key),
                Cost = x.Sum(s => s.Products.Price * s.ProductCount),
                Customer = DB.Context.Orders.FirstOrDefault(c => c.Id == x.Key).Users
            }).Where(w => w.Customer.UserId == _currentManager.Id && w.Customer.Id == user.Id).ToList();

            new ClientsDetailWindow(details).ShowDialog();
        }

        private void Image_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Image image = sender as Image;

            if ((image?.DataContext as UsersEx).User.Foto == null)
                image.Source = new BitmapImage(
                    new Uri(@"pack://application:,,,/CRMSystem;component/Resources/Images/Placeholders/ImagePlaceholder.png"));
        }
    }
}
