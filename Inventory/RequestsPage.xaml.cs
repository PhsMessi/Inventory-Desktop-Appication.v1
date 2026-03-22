using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Inventory
{
    public class RequestItem
    {
        public string ItemName { get; set; }
        public string RequestedBy { get; set; }
        public int Quantity { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
    }

    public partial class RequestsPage : Page
    {
        public static ObservableCollection<RequestItem> RequestList = new ObservableCollection<RequestItem>();

        public RequestsPage()
        {
            InitializeComponent();
            dgRequests.ItemsSource = RequestList;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = txtSearch.Text.ToLower();
            var filtered = RequestList.Where(r =>
                r.ItemName.ToLower().Contains(keyword) ||
                r.RequestedBy.ToLower().Contains(keyword) ||
                r.Status.ToLower().Contains(keyword));
            dgRequests.ItemsSource = new ObservableCollection<RequestItem>(filtered);
        }

        private void btnAddRequest_Click(object sender, RoutedEventArgs e)
        {
            var parentFrame = this.Parent as Frame;
            parentFrame?.Navigate(new AddRequestPage());
        }

        private void btnComplete_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var item = btn.Tag as RequestItem;
            if (item.Status == "Completed")
            {
                MessageBox.Show("This request is already completed.", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            item.Status = "Completed";
            dgRequests.ItemsSource = null;
            dgRequests.ItemsSource = RequestList;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var item = btn.Tag as RequestItem;
            var result = MessageBox.Show($"Delete request for '{item.ItemName}'?", "Confirm Delete",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                RequestList.Remove(item);
                dgRequests.ItemsSource = null;
                dgRequests.ItemsSource = RequestList;
            }
        }
    }
}