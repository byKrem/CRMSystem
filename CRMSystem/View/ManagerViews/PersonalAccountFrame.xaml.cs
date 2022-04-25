using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CRMSystem.View.ManagerViews
{
    /// <summary>
    /// Логика взаимодействия для PersonalAccountFrame.xaml
    /// </summary>
    
    class PersonalAnalityc
    {
        public Managers Manager { get; set; }
        public int CompleatedOrdersCount { get; set; }
        public int InProcessingOrdersCount { get; set; }
        public int NewOrdersCount { get; set; }
        public double Score { get; set; }
    }

    public partial class PersonalAccountFrame : Page
    {
        readonly Managers _currentManager;
        PersonalAnalityc personalAnalityc;
        CRMSystemEntities DB;
        WindowMain windowMain;
        DateTime delta;
        public PersonalAccountFrame(WindowMain wm, Managers manager)
        {
            InitializeComponent();
            _currentManager = manager;
            StartupTimeBlock.DataContext = wm;
            ImageFoto.DataContext = _currentManager;
            windowMain = wm;
            DB = new CRMSystemEntities();
            personalAnalityc = new PersonalAnalityc
            {
                Manager = DB.Managers.FirstOrDefault(f => f.Id == _currentManager.Id),
                CompleatedOrdersCount = DB.Orders.Where(w => w.OrderStatusId == 6 &&
                    w.Customers.ManagerId == _currentManager.Id).Count(),
                InProcessingOrdersCount = DB.Orders.Where(w => w.Customers.ManagerId == _currentManager.Id &&
                w.OrderStatusId != 6 && w.OrderStatusId != 5).Count(),
                NewOrdersCount = DB.Orders.Where(w => w.Customers.ManagerId == _currentManager.Id &&
                    w.OrderStatusId == 1).Count(),
                Score = 5.2
            };
            grid.DataContext = personalAnalityc;

            LiveCharts.SeriesCollection seriesViews = new LiveCharts.SeriesCollection
            {
                new LiveCharts.Wpf.PieSeries
                {
                    Values = new LiveCharts.ChartValues<decimal>
                    {DB.Orders.Where(w => w.Customers.ManagerId == _currentManager.Id &&
                        w.OrderStatusId == 6).Count() },
                    Title = "Завершённые заказы"
                },
                new LiveCharts.Wpf.PieSeries
                {
                    Values = new LiveCharts.ChartValues<decimal>
                    {DB.Orders.Where(w => w.Customers.ManagerId == _currentManager.Id &&
                        w.OrderStatusId != 5 && w.OrderStatusId != 6).Count()},
                    Title = "Заказы в обработке"
                },
                new LiveCharts.Wpf.PieSeries
                {
                    Values = new LiveCharts.ChartValues<decimal>
                    {DB.Orders.Where(w => w.Customers.ManagerId == _currentManager.Id &&
                    w.OrderStatusId == 1).Count()},
                    Title = "Новые заказы"
                }
            };
            PieChart.Series = seriesViews;

            SalaryGrid.ItemsSource = DB.PaymentHistory.Where(w =>
                    w.ManagerId == _currentManager.Id).ToList();
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is Image)) return;
            
            Image image = sender as Image;
            if (personalAnalityc?.Manager?.Foto == null)
                image.Source = new BitmapImage(
                    new Uri(@"pack://application:,,,/CRMSystem;component/IMG/unknownImage.png"));
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            windowMain.Close();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(DateTime.Now - delta < TimeSpan.FromMilliseconds(500))
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.FileName = "";
                dlg.DefaultExt = ".png";
                dlg.Filter = "Image files (.png)|*.png;*.jpg;*.jpeg";

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    byte[] imageBytes = null;
                    using (var fs = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read))
                    {
                        imageBytes = new byte[fs.Length];
                        fs.Read(imageBytes, 0, System.Convert.ToInt32(fs.Length));
                    }
                    DB.Managers.FirstOrDefault(f => f.Id == _currentManager.Id).Foto = imageBytes;
                    DB.SaveChanges();
                    // (sender as Image).Source = imageBytes;

                }
            }
            delta = DateTime.Now;
        }
    }
}
