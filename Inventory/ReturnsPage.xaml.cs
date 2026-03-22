using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Inventory
{
    public class ReturnItem
    {
        public string ItemName { get; set; }
        public string ReturnedBy { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
    }

    public partial class ReturnsPage : Page
    {
        public static ObservableCollection<ReturnItem> ReturnList = new ObservableCollection<ReturnItem>();

        public ReturnsPage()
        {
            InitializeComponent();
            dgReturns.ItemsSource = ReturnList;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = txtSearch.Text.ToLower();
            var filtered = ReturnList.Where(r =>
                r.ItemName.ToLower().Contains(keyword) ||
                r.ReturnedBy.ToLower().Contains(keyword) ||
                r.Reason.ToLower().Contains(keyword) ||
                r.Status.ToLower().Contains(keyword));
            dgReturns.ItemsSource = new ObservableCollection<ReturnItem>(filtered);
        }

        private void btnAddReturn_Click(object sender, RoutedEventArgs e)
        {
            var parentFrame = this.Parent as Frame;
            parentFrame?.Navigate(new AddReturnPage());
        }

        private void btnMarkReturned_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var item = btn.Tag as ReturnItem;
            if (item.Status == "Returned")
            {
                MessageBox.Show("This item is already marked as returned.", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            item.Status = "Returned";
            dgReturns.ItemsSource = null;
            dgReturns.ItemsSource = ReturnList;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var item = btn.Tag as ReturnItem;
            var result = MessageBox.Show($"Delete return for '{item.ItemName}'?", "Confirm Delete",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                ReturnList.Remove(item);
                dgReturns.ItemsSource = null;
                dgReturns.ItemsSource = ReturnList;
            }
        }
    }
}