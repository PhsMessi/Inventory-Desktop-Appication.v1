using Inventory.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Inventory
{
    public partial class MainWindow : Window
    {
        private readonly SupabaseSyncService _sync = new SupabaseSyncService();
        private DispatcherTimer _syncTimer;

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new DashboardPage());
            StartSyncTimer();
        }

        //private void StartSyncTimer()
        //{
        //    _syncTimer = new DispatcherTimer();
        //    _syncTimer.Interval = TimeSpan.FromMinutes(2);
        //    _syncTimer.Tick += async (s, e) => await RunSyncAsync();
        //    _syncTimer.Start();

        //    // Run once on startup
        //    Loaded += async (s, e) => await RunSyncAsync();
        //}

        private void StartSyncTimer()
        {
            _syncTimer = new DispatcherTimer();
            _syncTimer.Interval = TimeSpan.FromMinutes(2);
            _syncTimer.Tick += async (s, e) => await RunSyncAsync();
            _syncTimer.Start();

            // Delay first sync by 5 seconds to let app fully load
            var startupTimer = new DispatcherTimer();
            startupTimer.Interval = TimeSpan.FromSeconds(5);
            startupTimer.Tick += async (s, e) =>
            {
                startupTimer.Stop();
                await RunSyncAsync();
            };
            startupTimer.Start();
        }

        private async System.Threading.Tasks.Task RunSyncAsync()
        {
            try
            {
                txtSyncStatus.Text = "Syncing...";
                txtSyncDot.Foreground = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#F59E0B"));

                var result = await _sync.SyncAllAsync();

                if (result.IsOnline)
                {
                    txtSyncStatus.Text = "Synced";
                    txtSyncDot.Foreground = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#22C55E"));
                }
                else
                {
                    txtSyncStatus.Text = "Offline";
                    txtSyncDot.Foreground = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#EF4444"));
                }
            }
            catch
            {
                txtSyncStatus.Text = "Sync Failed";
                txtSyncDot.Foreground = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#EF4444"));
            }
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();

            switch (tag)
            {
                case "Dashboard":
                    MainFrame.Navigate(new DashboardPage());
                    break;
                case "Stocks":
                    MainFrame.Navigate(new StocksPage());
                    break;
                case "Requests":
                    MainFrame.Navigate(new RequestsPage());
                    break;
                case "Returns":
                    MainFrame.Navigate(new ReturnsPage());
                    break;
                case "AddStock":
                    MainFrame.Navigate(new AddStockPage());
                    break;
                case "AddReturn":
                    MainFrame.Navigate(new AddReturnPage());
                    break;
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            _syncTimer?.Stop();
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}