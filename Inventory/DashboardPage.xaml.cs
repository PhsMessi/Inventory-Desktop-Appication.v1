using System.Windows.Controls;

namespace Inventory
{
    public partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            InitializeComponent();

            // Placeholder values for now
            // These will update automatically once we connect Stocks, Requests, Returns
            txtTotalStocks.Text = "0";
            txtPendingRequests.Text = "0";
            txtTotalReturns.Text = "0";
            txtLowStock.Text = "0";
        }
    }
}