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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CRMSystem.View.ManagerViews
{
    /// <summary>
    /// Логика взаимодействия для ClientsFrame.xaml
    /// </summary>
    public partial class ClientsFrame : UserControl
    {
        CRMSystemEntities DB;
        private readonly Managers _currentManager;
        public ClientsFrame(Managers manager)
        {
            InitializeComponent();
            _currentManager = manager;
            DB = new CRMSystemEntities();
            grid.ItemsSource = DB.Customers.Where(w => w.ManagerId == _currentManager.Id).ToList();
        }

        private void grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}
