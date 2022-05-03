using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using CRMSystem.Native.Window;
using CRMSystem.Native.Window.Utils;
using CRMSystem.Settings;

namespace CRMSystem.Views.CustomerViews
{
    public partial class CustomerWindow : Window, INativelyRestorableWindow
    {
        private HwndSource _hwndSource;
        private WindowState _previousState;



        public bool DuringRestoreToMaximized { get; private set; }



        public Users CurrentCustomer { get; private set; }
        public DateTime StartupTime { get; private set; }



#pragma warning disable SS002 // DateTime.Now was referenced
        public CustomerWindow(Users customer)
        {
            InitializeComponent();
            DataContext = this;

            _previousState = WindowState;
            DuringRestoreToMaximized = WindowState == WindowState.Maximized;

            WindowStartupLocation = WindowStartupLocation.Manual;
            Width = SettingsManager.AppSettings.CustomerWindowWidth;
            Height = SettingsManager.AppSettings.CustomerWindowHeight;
            Left = SettingsManager.AppSettings.CustomerWindowPositionX - (Width / 2.0);
            Top = SettingsManager.AppSettings.CustomerWindowPositionY - (Height / 2.0);
            WindowState = SettingsManager.AppSettings.CustomerWindowState;

            CurrentCustomer = customer;
            StartupTime = DateTime.Now;

            if (customer?.Foto == null)
            {
                CustomerAvatar.Source = new BitmapImage(new Uri(
                    "pack://application:,,,/CRMSystem;component/Resources/Images/Placeholders/ImagePlaceholder.png"));
            }

            MainFrame.Navigate(
                new CatalogFrame());
        }
#pragma warning restore SS002 // DateTime.Now was referenced



        protected override void OnSourceInitialized(
            EventArgs e)
        {
            base.OnSourceInitialized(e);

            _hwndSource = (HwndSource)PresentationSource.FromVisual(this);

            _hwndSource?.AddHook(WindowUtils.HwndSourceHook);
        }

        protected override void OnStateChanged(
            EventArgs e)
        {
            _previousState = WindowState;

            base.OnStateChanged(e);

            if (_previousState != WindowState.Minimized)
            {
                DuringRestoreToMaximized = WindowState == WindowState.Maximized;
            }
        }



        private void CustomerWindow_Closed(object sender,
            EventArgs e)
        {
            SettingsManager.AppSettings.CustomerWindowWidth = Width;
            SettingsManager.AppSettings.CustomerWindowHeight = Height;
            SettingsManager.AppSettings.CustomerWindowPositionX = Left + (Width / 2.0);
            SettingsManager.AppSettings.CustomerWindowPositionY = Top + (Height / 2.0);
            SettingsManager.AppSettings.CustomerWindowState = WindowState;

            SettingsManager.AppSettings.Save();

            _hwndSource?.RemoveHook(WindowUtils.HwndSourceHook);
        }



        private void CatalogButton_Click(object sender,
            RoutedEventArgs e)
        {
            MainFrame.Navigate(
                new CatalogFrame());
        }

        private void CartButton_Click(object sender,
            RoutedEventArgs e)
        {
            MainFrame.Navigate(
                new CartFrame());
        }
    }
}
