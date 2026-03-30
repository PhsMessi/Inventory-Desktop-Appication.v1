using Inventory.Data;
using Inventory.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Inventory
{
    public partial class AddReturnPage : Page
    {
        private readonly DatabaseService _db = new DatabaseService();

        public AddReturnPage()
        {
            InitializeComponent();
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            txtError.Text = "";
            txtSuccess.Text = "";

            if (string.IsNullOrWhiteSpace(txtItemName.Text) ||
                string.IsNullOrWhiteSpace(txtReturnedBy.Text) ||
                string.IsNullOrWhiteSpace(txtQuantity.Text) ||
                string.IsNullOrWhiteSpace(txtReason.Text))
            {
                txtError.Text = "⚠️ Please fill in all required fields.";
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int qty))
            {
                txtError.Text = "⚠️ Quantity must be a valid number.";
                return;
            }

            var status = (cmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Not Yet";

            try
            {
                var entity = new ReturnEntity
                {
                    ItemName = txtItemName.Text.Trim(),
                    ReturnedBy = txtReturnedBy.Text.Trim(),
                    Quantity = qty,
                    Reason = txtReason.Text.Trim(),
                    Date = DateTime.Today,
                    Status = status,
                    IsSynced = false
                };

                await _db.AddReturnAsync(entity);

                // Reload returns list
                var returns = await _db.GetAllReturnsAsync();
                ReturnsPage.ReturnList.Clear();
                foreach (var r in returns)
                {
                    ReturnsPage.ReturnList.Add(new ReturnItem
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

                txtSuccess.Text = "✅ Return recorded successfully!";
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
            txtItemName.Text = "";
            txtReturnedBy.Text = "";
            txtQuantity.Text = "";
            txtReason.Text = "";
            cmbStatus.SelectedIndex = 0;
        }
    }
}