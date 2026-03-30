using Inventory.Data;
using Inventory.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Inventory
{
    public class RequestItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string RequestedItems { get; set; }
        public string RequestedBy { get; set; }
        public int Quantity { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
        public bool IsSynced { get; set; } = false;
    }

    public partial class RequestsPage : Page
    {
        public static ObservableCollection<RequestItem> RequestList = new ObservableCollection<RequestItem>();
        private readonly DatabaseService _db = new DatabaseService();

        public RequestsPage()
        {
            InitializeComponent();
            LoadRequestsAsync();
        }

        private async void LoadRequestsAsync()
        {
            try
            {
                var requests = await _db.GetAllRequestsAsync();
                RequestList.Clear();
                foreach (var r in requests)
                {
                    RequestList.Add(new RequestItem
                    {
                        Id = r.Id,
                        RequestedItems = r.RequestedItems,
                        RequestedBy = r.RequestedBy,
                        Quantity = r.Quantity,
                        Date = r.Date.ToString("MM/dd/yyyy"),
                        Status = r.Status,
                        IsSynced = r.IsSynced
                    });
                }
                dgRequests.ItemsSource = null;
                dgRequests.ItemsSource = RequestList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading requests: {ex.Message}", "Database Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}