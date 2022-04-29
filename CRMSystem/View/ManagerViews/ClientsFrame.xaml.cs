using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace CRMSystem.View.ManagerViews
{
    public partial class ClientsFrame : UserControl
    {
        CRMSystemEntities DB;
        private readonly Users _currentManager;
        public ClientsFrame(Users manager)
        {
            InitializeComponent();
            _currentManager = manager;
            DB = new CRMSystemEntities();
            grid.ItemsSource = DB.Users.Where(w => w.UserId == _currentManager.Id).ToList();
        }

        private void grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // TODO: Открывать подробную информацию о клиенте + история его заказов
        }
    }
}
