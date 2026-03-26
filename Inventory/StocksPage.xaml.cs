using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Inventory
{
    public class StockItem
    {
        public int SerialNumber { get; set; }
        public string ItemName { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string Warranty { get; set; }
        public string DateAdded { get; set; }
    }

    public partial class StocksPage : Page
    {
        public static ObservableCollection<StockItem> StockList = new ObservableCollection<StockItem>();
        private string _activeTab = "All";

        public StocksPage()
        {
            InitializeComponent();
            dgStocks.ItemsSource = StockList;
        }

        private void TabButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            _activeTab = btn.Tag.ToString();

            // Reset all tab styles
            var tabs = new[] { tabAll, tabCameras, tabDVR, tabNVR, tabPOE, tabHDD, tabAdaptor };
            foreach (var tab in tabs)
            {
                tab.Background = System.Windows.Media.Brushes.Transparent;
                tab.Foreground = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#64748B"));
            }

            // Set active tab style
            btn.Background = new System.Windows.Media.SolidColorBrush(
                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#0F172A"));
            btn.Foreground = new System.Windows.Media.SolidColorBrush(
                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#F8FAFC"));

            FilterTable();
        }

        private void FilterTable()
        {
            if (_activeTab == "All")
            {
                dgStocks.ItemsSource = new ObservableCollection<StockItem>(StockList);
            }
            else
            {
                var filtered = StockList.Where(s =>
                    s.Category.Equals(_activeTab, System.StringComparison.OrdinalIgnoreCase));
                dgStocks.ItemsSource = new ObservableCollection<StockItem>(filtered);
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = txtSearch.Text.ToLower();
            var source = _activeTab == "All" ? StockList :
                new ObservableCollection<StockItem>(
                    StockList.Where(s => s.Category.Equals(_activeTab, System.StringComparison.OrdinalIgnoreCase)));

            var filtered = source.Where(s =>
                s.ItemName.ToLower().Contains(keyword) ||
                s.Category.ToLower().Contains(keyword) ||
                s.Description.ToLower().Contains(keyword));

            dgStocks.ItemsSource = new ObservableCollection<StockItem>(filtered);
        }

        private Frame GetParentFrame()
        {
            var parent = this.Parent as Frame;
            return parent;
        }
    }
}