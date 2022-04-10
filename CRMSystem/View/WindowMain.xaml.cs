using System;
using System.Windows;

namespace CRMSystem.View
{
    public partial class WindowMain : Window
    {

        public WindowMain()
        {
            InitializeComponent();
            MainFrame.Navigate(new OrdersFrame());
        }

    }
}
