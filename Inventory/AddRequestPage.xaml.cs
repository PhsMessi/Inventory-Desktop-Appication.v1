using Inventory.Data;
using Inventory.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Inventory
{
    public partial class AddRequestPage : Page
    {
        private readonly DatabaseService _db = new DatabaseService();

        public AddRequestPage()
        {
            InitializeComponent();
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            txtError.Text = "";
            txtSuccess.Text = "";

            if (string.IsNullOrWhiteSpace(txtRequestedItems.Text) ||
                string.IsNullOrWhiteSpace(txtRequestedBy.Text) ||
                string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                txtError.Text = "⚠️ Please fill in all required fields.";
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int qty))
            {
                txtError.Text = "⚠️ Quantity must be a valid number.";
                return;
            }

            var status = (cmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Pending";

            try
            {
                var entity = new RequestEntity
                {
                    RequestedItems = txtRequestedItems.Text.Trim(),
                    RequestedBy = txtRequestedBy.Text.Trim(),
                    Quantity = qty,
                    Date = DateTime.Today,
                    Status = status,
                    IsSynced = false
                };

                await _db.AddRequestAsync(entity);

                // Reload requests list
                var requests = await _db.GetAllRequestsAsync();
                RequestsPage.RequestList.Clear();
                foreach (var r in requests)
                {
                    RequestsPage.RequestList.Add(new RequestItem
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

                txtSuccess.Text = "✅ Request submitted successfully!";
                ClearFields();
            }
            catch (Exception ex)
            {
                txtError.Text = $"⚠️ Database error: {ex.Message}";
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            txtError.Text = "";
            txtSuccess.Text = "";
        }

        private void ClearFields()
        {
            txtRequestedItems.Text = "";
            txtRequestedBy.Text = "";
            txtQuantity.Text = "";
            cmbStatus.SelectedIndex = 0;
        }
    }
}