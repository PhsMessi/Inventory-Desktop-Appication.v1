using System;
using System.Windows;
using System.Windows.Controls;

namespace Inventory
{
    public partial class AddRequestPage : Page
    {
        public AddRequestPage()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
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

            RequestsPage.RequestList.Add(new RequestItem
            {
                RequestedItems = txtRequestedItems.Text.Trim(),
                RequestedBy = txtRequestedBy.Text.Trim(),
                Quantity = qty,
                Date = DateTime.Now.ToString("MM/dd/yyyy"),
                Status = status
            });

            txtSuccess.Text = "✅ Request submitted successfully!";
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
            txtRequestedItems.Text = "";
            txtRequestedBy.Text = "";
            txtQuantity.Text = "";
            cmbStatus.SelectedIndex = 0;
        }
    }
}