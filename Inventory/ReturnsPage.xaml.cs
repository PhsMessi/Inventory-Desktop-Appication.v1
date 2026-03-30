using Inventory.Data;
using Inventory.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Inventory
{
    public class ReturnItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ItemName { get; set; }
        public string ReturnedBy { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
        public bool IsSynced { get; set; } = false;
    }

    public partial class ReturnsPage : Page
    {
        public static ObservableCollection<ReturnItem> ReturnList = new ObservableCollection<ReturnItem>();
        private readonly DatabaseService _db = new DatabaseService();

        public ReturnsPage()
        {
            InitializeComponent();
            LoadReturnsAsync();
        }

        private async void LoadReturnsAsync()
        {
            try
            {
                var returns = await _db.GetAllReturnsAsync();
                ReturnList.Clear();
                foreach (var r in returns)
                {
                    ReturnList.Add(new ReturnItem
                    {
                        Id = r.Id,
                        ItemName = r.ItemName,
                        ReturnedBy = r.ReturnedBy,
                        Quantity = r.Quantity,
                        Reason = r.Reason,
                        Date = r.Date.ToString("MM/dd/yyyy"),
                        Status = r.Status,
                        IsSynced = r.IsSynced
                    });
                }
                dgReturns.ItemsSource = null;
                dgReturns.ItemsSource = ReturnList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading returns: {ex.Message}", "Database Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}