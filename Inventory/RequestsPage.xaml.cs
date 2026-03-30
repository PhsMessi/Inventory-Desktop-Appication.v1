using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Inventory
{
    public class RequestItem
    {
        public string RequestedItems { get; set; }
        public string RequestedBy { get; set; }
        public int Quantity { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
    }

    public partial class RequestsPage : Page
    {
        public static ObservableCollection<RequestItem> RequestList
            = new ObservableCollection<RequestItem>();

        public RequestsPage()
        {
            InitializeComponent();
            dgRequests.ItemsSource = RequestList;
        }
    }
}