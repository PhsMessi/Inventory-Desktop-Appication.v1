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

        // Constructor for Edit mode
        public AddStockPage(StockItem existing)
        {
            InitializeComponent();
            txtItemName.Text = existing.ItemName;
            txtCategory.Text = existing.Category;
            txtQuantity.Text = existing.Quantity.ToString();
            txtDescription.Text = existing.Description;
            btnSave.Content = "✅ Update Stock";
            // Store reference for editing
            Tag = existing;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            txtError.Text = "";
            txtSuccess.Text = "";

            if (string.IsNullOrWhiteSpace(txtItemName.Text) ||
                string.IsNullOrWhiteSpace(txtCategory.Text) ||
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

            var newItem = new StockItem
            {
                SerialNumber = StocksPage.StockList.Count + 1,
                ItemName = txtItemName.Text.Trim(),
                Category = txtCategory.Text.Trim(),
                Quantity = qty,
                Description = txtDescription.Text.Trim(),
                DateAdded = System.DateTime.Now.ToString("MM/dd/yyyy")
            };

            if (Tag is StockItem existing)
            {
                int index = StocksPage.StockList.IndexOf(existing);
                newItem.SerialNumber = existing.SerialNumber;
                StocksPage.StockList[index] = newItem;
                txtSuccess.Text = "✅ Stock item updated successfully!";
            }
            else
            {
                StocksPage.StockList.Add(newItem);
                txtSuccess.Text = "✅ Stock item added successfully!";
                ClearFields();
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
            txtCategory.Text = "";
            txtQuantity.Text = "";
            txtDescription.Text = "";
        }
    }
}
