using Inventory.Data;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace Inventory
{
    public partial class RequestDetailWindow : Window
    {
        private readonly RequestItem _item;
        private readonly DatabaseService _db = new DatabaseService();
        public bool WasApproved { get; private set; } = false;

        public RequestDetailWindow(RequestItem item)
        {
            InitializeComponent();
            _item = item;
            LoadDetails();
        }

        private void LoadDetails()
        {
            txtItems.Text = _item.RequestedItems;
            txtRequestedBy.Text = _item.RequestedBy;
            txtQuantity.Text = _item.Quantity.ToString();
            txtDate.Text = _item.Date;

            // Status badge
            if (_item.Status == "Completed")
            {
                statusBadge.Background = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#DCFCE7"));
                txtStatusBadge.Text = "✅ Completed";
                txtStatusBadge.Foreground = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#16A34A"));
                btnApprove.IsEnabled = false;
                btnApprove.Background = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#94A3B8"));
                btnApprove.Content = "Already Approved";
            }
            else
            {
                statusBadge.Background = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FEF9C3"));
                txtStatusBadge.Text = "⏳ Pending";
                txtStatusBadge.Foreground = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#CA8A04"));
            }
        }

        private async void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            btnApprove.IsEnabled = false;
            btnApprove.Content = "Approving...";

            try
            {
                // Update local DB
                using var db = new AppDbContext();
                var entity = await db.Requests.FindAsync(_item.Id);
                if (entity != null)
                {
                    entity.Status = "Completed";
                    entity.IsSynced = false;
                    await db.SaveChangesAsync();
                }

                // Update Supabase directly
                await UpdateSupabaseStatusAsync(_item.Id.ToString(), "Completed");

                WasApproved = true;
                MessageBox.Show("Request has been approved successfully!", "Approved ✅",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error approving request: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                btnApprove.IsEnabled = true;
                btnApprove.Content = "✅ Approve Request";
            }
        }

        //private async System.Threading.Tasks.Task UpdateSupabaseStatusAsync(string id, string status)
        //{
        //    using var http = new HttpClient();
        //    http.DefaultRequestHeaders.Add("apikey", App.SupabaseKey);
        //    http.DefaultRequestHeaders.Add("Authorization", $"Bearer {App.SupabaseKey}");

        //    var payload = JsonSerializer.Serialize(new { status = status, is_synced = true });
        //    var content = new StringContent(payload, Encoding.UTF8, "application/json");

        //    await http.PatchAsync(
        //        $"{App.SupabaseUrl}/rest/v1/requests?id=eq.{id}",
        //        content);
        //}

        private async System.Threading.Tasks.Task UpdateSupabaseStatusAsync(string id, string status)
        {
            try
            {
                using var http = new HttpClient();
                http.DefaultRequestHeaders.Add("apikey", App.SupabaseKey);
                http.DefaultRequestHeaders.Add("Authorization", $"Bearer {App.SupabaseKey}");
                http.DefaultRequestHeaders.Add("Prefer", "return=minimal");

                var payload = JsonSerializer.Serialize(new
                {
                    status = status,
                    is_synced = true
                });

                var content = new StringContent(payload, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Patch,
                    $"{App.SupabaseUrl}/rest/v1/requests?id=eq.{id}")
                {
                    Content = content
                };

                var response = await http.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Supabase error: {error}", "Sync Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update cloud: {ex.Message}", "Sync Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}