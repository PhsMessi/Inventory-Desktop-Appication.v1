using System.Windows;
using System.Windows.Controls;

namespace Inventory
{
    public partial class AddStockPage : Page
    {
        public AddStockPage()
        {
            InitializeComponent();
        }

        // Edit mode constructor
        public AddStockPage(StockItem existing)
        {
            InitializeComponent();
            btnSave.Content = "Update Stock";

            txtSerialNumber.Text = existing.SerialNumber.ToString();
            txtModelNumber.Text = existing.ModelNumber;
            txtProductName.Text = existing.ItemName;
            txtAddedBy.Text = existing.AddedBy;

            foreach (ComboBoxItem item in cmbCategory.Items)
            {
                if (item.Content.ToString() == existing.Category)
                {
                    cmbCategory.SelectedItem = item;
                    break;
                }
            }

            foreach (ComboBoxItem item in cmbWarranty.Items)
            {
                if (item.Content.ToString() == existing.Warranty)
                {
                    cmbWarranty.SelectedItem = item;
                    break;
                }
            }

            Tag = existing;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
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

            var newItem = new StockItem
            {
                SerialNumber = StocksPage.StockList.Count + 1,
                ModelNumber = txtModelNumber.Text.Trim(),
                ItemName = txtProductName.Text.Trim(),
                Category = category,
                AddedBy = txtAddedBy.Text.Trim(),
                Warranty = warranty,
                DateAdded = System.DateTime.Now.ToString("MM/dd/yyyy")
            };

            if (Tag is StockItem existing)
            {
                int index = StocksPage.StockList.IndexOf(existing);
                newItem.SerialNumber = existing.SerialNumber;
                newItem.DateAdded = existing.DateAdded;
                StocksPage.StockList[index] = newItem;
                ShowSuccess("Stock item updated successfully!");
            }
            else
            {
                StocksPage.StockList.Add(newItem);
                ShowSuccess("Stock item added successfully!");
                ClearFields();
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