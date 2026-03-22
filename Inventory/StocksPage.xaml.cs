using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Inventory
{
    public class StockItem
    {
        public string ItemName { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
    }

    public partial class StocksPage : Page
    {
        public static ObservableCollection<StockItem> StockList = new ObservableCollection<StockItem>();

        public StocksPage()
        {
            InitializeComponent();
            dgStocks.ItemsSource = StockList;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = txtSearch.Text.ToLower();
            var filtered = StockList.Where(s =>
                s.ItemName.ToLower().Contains(keyword) ||
                s.Category.ToLower().Contains(keyword) ||
                s.Description.ToLower().Contains(keyword));
            dgStocks.ItemsSource = new ObservableCollection<StockItem>(filtered);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var item = btn.Tag as StockItem;
            // Navigate to AddStockPage in edit mode
            var parentFrame = GetParentFrame();
            parentFrame?.Navigate(new AddStockPage(item));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var item = btn.Tag as StockItem;
            var result = MessageBox.Show($"Delete '{item.ItemName}'?", "Confirm Delete",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                StockList.Remove(item);
                dgStocks.ItemsSource = null;
                dgStocks.ItemsSource = StockList;
            }
        }

        private Frame GetParentFrame()
        {
            var parent = this.Parent as Frame;
            return parent;
        }
    }
}