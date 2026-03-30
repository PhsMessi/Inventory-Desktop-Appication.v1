using Inventory.Data;
using Inventory.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Inventory
{
    public partial class AddStockPage : Page
    {
        private readonly DatabaseService _db = new DatabaseService();

        public AddStockPage()
        {
            InitializeComponent();
        }

        public AddStockPage(StockItem existing)
        {
            InitializeComponent();
            btnSave.Content = "Update Stock";
            txtSerialNumber.Text = existing.SerialNumber;
            txtModelNumber.Text = existing.ModelNumber;
            txtProductName.Text = existing.ItemName;
            txtAddedBy.Text = existing.AddedBy;

            foreach (ComboBoxItem item in cmbCategory.Items)
                if (item.Content.ToString() == existing.Category)
                { cmbCategory.SelectedItem = item; break; }

            foreach (ComboBoxItem item in cmbWarranty.Items)
                if (item.Content.ToString() == existing.Warranty)
                { cmbWarranty.SelectedItem = item; break; }

            Tag = existing;
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            HideMessages();

            if (string.IsNullOrWhiteSpace(txtSerialNumber.Text) ||
                string.IsNullOrWhiteSpace(txtModelNumber.Text) ||
                string.IsNullOrWhiteSpace(txtProductName.Text) ||
                cmbCategory.SelectedItem == null ||
                string.IsNullOrWhiteSpace(txtAddedBy.Text))
            {
                ShowError("Please fill in all required fields.");
                return;
            }

            var category = (cmbCategory.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
            var warranty = (cmbWarranty.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "1 Year";

            try
            {
                if (Tag is StockItem existing)
                {
                    // Edit mode — update in DB
                    var entity = new StockEntity
                    {
                        Id = existing.Id,
                        SerialNumber = txtSerialNumber.Text.Trim(),
                        ModelNumber = txtModelNumber.Text.Trim(),
                        ProductName = txtProductName.Text.Trim(),
                        Category = category,
                        AddedBy = txtAddedBy.Text.Trim(),
                        Warranty = warranty,
                        DateAdded = DateTime.Parse(existing.DateAdded),
                        IsSynced = false
                    };
                    await _db.UpdateStockAsync(entity);
                    ShowSuccess("Stock item updated successfully!");
                }
                else
                {
                    // Add mode — insert to DB
                    var entity = new StockEntity
                    {
                        SerialNumber = txtSerialNumber.Text.Trim(),
                        ModelNumber = txtModelNumber.Text.Trim(),
                        ProductName = txtProductName.Text.Trim(),
                        Category = category,
                        AddedBy = txtAddedBy.Text.Trim(),
                        Warranty = warranty,
                        DateAdded = DateTime.Today,
                        IsSynced = false
                    };
                    await _db.AddStockAsync(entity);
                    ShowSuccess("Stock item added successfully!");
                    ClearFields();
                }

                // Reload stocks list
                var stocks = await _db.GetAllStocksAsync();
                StocksPage.StockList.Clear();
                foreach (var s in stocks)
                {
                    StocksPage.StockList.Add(new StockItem
                    {
                        Id = s.Id,
                        SerialNumber = s.SerialNumber,
                        ModelNumber = s.ModelNumber,
                        ItemName = s.ProductName,
                        Category = s.Category,
                        AddedBy = s.AddedBy,
                        Warranty = s.Warranty,
                        DateAdded = s.DateAdded.ToString("MM/dd/yyyy"),
                        IsSynced = s.IsSynced
                    });
                }
            }
            catch (Exception ex)
            {
                ShowError($"Database error: {ex.Message}");
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            HideMessages();
        }

        private void ClearFields()
        {
            txtSerialNumber.Text = "";
            txtModelNumber.Text = "";
            txtProductName.Text = "";
            cmbCategory.SelectedIndex = -1;
            txtAddedBy.Text = "";
            cmbWarranty.SelectedIndex = 2;
        }

        private void ShowError(string message)
        {
            txtError.Text = "⚠️ " + message;
            errorBorder.Visibility = Visibility.Visible;
            successBorder.Visibility = Visibility.Collapsed;
        }

        private void ShowSuccess(string message)
        {
            txtSuccess.Text = "✅ " + message;
            successBorder.Visibility = Visibility.Visible;
            errorBorder.Visibility = Visibility.Collapsed;
        }

        private void HideMessages()
        {
            errorBorder.Visibility = Visibility.Collapsed;
            successBorder.Visibility = Visibility.Collapsed;
        }
    }
}