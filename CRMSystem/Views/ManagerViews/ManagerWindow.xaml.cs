using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using CRMSystem.Native.Window;
using CRMSystem.Native.Window.Utils;
using CRMSystem.Settings;

namespace CRMSystem.Views.ManagerViews
{
    public partial class ManagerWindow : Window, INativelyRestorableWindow
    {
        private HwndSource _hwndSource;
        private WindowState _previousState;



        public bool DuringRestoreToMaximized { get; private set; }



        public Users CurrentManager { get; private set; }
        public DateTime StartupTime { get; private set; }



#pragma warning disable SS002 // DateTime.Now was referenced
        public ManagerWindow(Users manager)
        {
            InitializeComponent();
            DataContext = this;

            _previousState = WindowState;
            DuringRestoreToMaximized = WindowState == WindowState.Maximized;

            WindowStartupLocation = WindowStartupLocation.Manual;
            Width = SettingsManager.AppSettings.ManagerWindowWidth;
            Height = SettingsManager.AppSettings.ManagerWindowHeight;
            Left = SettingsManager.AppSettings.ManagerWindowPositionX - (Width / 2.0);
            Top = SettingsManager.AppSettings.ManagerWindowPositionY - (Height / 2.0);
            WindowState = SettingsManager.AppSettings.ManagerWindowState;

            CurrentManager = manager;
            StartupTime = DateTime.Now;

            if (manager?.Foto == null)
            {
                ManagerAvatar.Source = new BitmapImage(new Uri(
                    "pack://application:,,,/CRMSystem;component/Resources/Images/Placeholders/ImagePlaceholder.png"));
            }

            MainFrame.Navigate(
                new OrdersFrame(CurrentManager));
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



        private void ManagerWindow_Closed(object sender,
            EventArgs e)
        {
            SettingsManager.AppSettings.ManagerWindowWidth = Width;
            SettingsManager.AppSettings.ManagerWindowHeight = Height;
            SettingsManager.AppSettings.ManagerWindowPositionX = Left + (Width / 2.0);
            SettingsManager.AppSettings.ManagerWindowPositionY = Top + (Height / 2.0);
            SettingsManager.AppSettings.ManagerWindowState = WindowState;

            SettingsManager.AppSettings.Save();

            _hwndSource?.RemoveHook(WindowUtils.HwndSourceHook);
        }



        private void PersonalAccountButton_Click(object sender,
            RoutedEventArgs e)
        {
            MainFrame.Navigate(
                new PersonalAccountFrame(this, CurrentManager));
        }

        private void OrdersButton_Click(object sender,
            RoutedEventArgs e)
        {
            MainFrame.Navigate(
                new OrdersFrame(CurrentManager));
        }

        private void StorageButton_Click(object sender,
            RoutedEventArgs e)
        {
            MainFrame.Navigate(
                new StorageFrame());
        }

        private void ClientsButton_Click(object sender,
            RoutedEventArgs e)
        {
            MainFrame.Navigate(
                new ClientsFrame(CurrentManager));
        }
    }
}
