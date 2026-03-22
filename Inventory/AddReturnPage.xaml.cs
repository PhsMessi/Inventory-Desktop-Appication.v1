using System;
using System.Windows;
using System.Windows.Controls;

namespace Inventory
{
    public partial class AddReturnPage : Page
    {
        public AddReturnPage()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
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

            ReturnsPage.ReturnList.Add(new ReturnItem
            {
                ItemName = txtItemName.Text.Trim(),
                ReturnedBy = txtReturnedBy.Text.Trim(),
                Quantity = qty,
                Reason = txtReason.Text.Trim(),
                Date = DateTime.Now.ToString("MM/dd/yyyy"),
                Status = status
            });

            txtSuccess.Text = "✅ Return recorded successfully!";
            ClearFields();
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