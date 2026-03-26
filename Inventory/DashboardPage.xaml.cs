using System;
using System.Linq;
using System.Windows.Controls;

namespace Inventory
{
    public partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            InitializeComponent();
            LoadWeeklyData();
        }

        private void LoadWeeklyData()
        {
            // Get start and end of current week (Monday to Sunday)
            DateTime today = DateTime.Today;
            int daysFromMonday = (int)today.DayOfWeek - (int)DayOfWeek.Monday;
            if (daysFromMonday < 0) daysFromMonday += 7;
            DateTime weekStart = today.AddDays(-daysFromMonday);
            DateTime weekEnd = weekStart.AddDays(6);

            // Show week range label
            txtWeekRange.Text = $"{weekStart:MMM dd} – {weekEnd:MMM dd, yyyy}";

            // New Stocks Added this week
            int newStocks = StocksPage.StockList.Count(s =>
            {
                if (DateTime.TryParse(s.DateAdded, out DateTime d))
                    return d >= weekStart && d <= weekEnd;
                return false;
            });
            txtNewStocks.Text = newStocks.ToString();

            // Total Returns this week
            int totalReturns = ReturnsPage.ReturnList.Count(r =>
            {
                if (DateTime.TryParse(r.Date, out DateTime d))
                    return d >= weekStart && d <= weekEnd;
                return false;
            });
            txtTotalReturns.Text = totalReturns.ToString();

            // Pending Requests this week
            int pendingRequests = RequestsPage.RequestList.Count(r =>
                r.Status == "Pending" &&
                DateTime.TryParse(r.Date, out DateTime d) &&
                d >= weekStart && d <= weekEnd);
            txtPendingRequests.Text = pendingRequests.ToString();

            // Defective Items this week
            // Counts returns where reason contains "defect" or "broken" or "damaged"
            int defectiveItems = ReturnsPage.ReturnList.Count(r =>
            {
                if (!DateTime.TryParse(r.Date, out DateTime d)) return false;
                if (d < weekStart || d > weekEnd) return false;
                string reason = r.Reason?.ToLower() ?? "";
                return reason.Contains("defect") ||
                       reason.Contains("broken") ||
                       reason.Contains("damaged") ||
                       reason.Contains("faulty");
            });
            txtDefectiveItems.Text = defectiveItems.ToString();
        }
    }
}