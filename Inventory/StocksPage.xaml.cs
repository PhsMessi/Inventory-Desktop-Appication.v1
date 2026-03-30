using Inventory.Data;
using Inventory.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Inventory
{
    public class StockItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string SerialNumber { get; set; }
        public string ModelNumber { get; set; }
        public string ItemName { get; set; }
        public string Category { get; set; }
        public string AddedBy { get; set; }
        public string Warranty { get; set; }
        public string DateAdded { get; set; }
        public bool IsSynced { get; set; } = false;
    }

    public partial class StocksPage : Page
    {
        public static ObservableCollection<StockItem> StockList = new ObservableCollection<StockItem>();
        private string _activeTab = "All";
        private readonly DatabaseService _db = new DatabaseService();

        public StocksPage()
        {
            InitializeComponent();
            LoadStocksAsync();
        }

        private async void LoadStocksAsync()
        {
            try
            {
                var stocks = await _db.GetAllStocksAsync();
                StockList.Clear();
                foreach (var s in stocks)
                {
                    StockList.Add(new StockItem
                    {
                        Id = s.Id,
                        SerialNumber = s.SerialNumber,
                        ModelNumber = s.ModelNumber,
                        ItemName = s.ProductName,
                        Category = s.Category,
                        AddedBy = s.AddedBy,
                        Warranty = s.Warranty,
                        DateAdded = s.DateAdded.ToString("MM/dd/yyyy"),
                        IsSynced = s.IsSynced
                    });
                }
                dgStocks.ItemsSource = null;
                dgStocks.ItemsSource = StockList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading stocks: {ex.Message}", "Database Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TabButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            _activeTab = btn.Tag.ToString();

            var tabs = new[] { tabAll, tabCameras, tabDVR, tabNVR, tabPOE, tabHDD, tabAdaptor };
            foreach (var tab in tabs)
            {
                tab.Background = System.Windows.Media.Brushes.Transparent;
                tab.Foreground = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#64748B"));
            }

            btn.Background = new System.Windows.Media.SolidColorBrush(
                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#0F172A"));
            btn.Foreground = new System.Windows.Media.SolidColorBrush(
                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#F8FAFC"));

            FilterTable();
        }

        private void FilterTable()
        {
            if (_activeTab == "All")
                dgStocks.ItemsSource = new ObservableCollection<StockItem>(StockList);
            else
            {
                var filtered = StockList.Where(s =>
                    s.Category.Equals(_activeTab, StringComparison.OrdinalIgnoreCase));
                dgStocks.ItemsSource = new ObservableCollection<StockItem>(filtered);
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = txtSearch.Text.ToLower();
            var source = _activeTab == "All" ? StockList :
                new ObservableCollection<StockItem>(
                    StockList.Where(s => s.Category.Equals(_activeTab, StringComparison.OrdinalIgnoreCase)));

            var filtered = source.Where(s =>
                s.ItemName.ToLower().Contains(keyword) ||
                s.Category.ToLower().Contains(keyword) ||
                s.ModelNumber.ToLower().Contains(keyword) ||
                s.AddedBy.ToLower().Contains(keyword));

            dgStocks.ItemsSource = new ObservableCollection<StockItem>(filtered);
        }

        private Frame GetParentFrame()
        {
            return this.Parent as Frame;
        }
    }
}