using System.Collections.ObjectModel;
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
        public static ObservableCollection<ReturnItem> ReturnList
            = new ObservableCollection<ReturnItem>();

        public ReturnsPage()
        {
            InitializeComponent();
            dgReturns.ItemsSource = ReturnList;
        }
    }
}