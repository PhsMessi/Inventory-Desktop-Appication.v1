using Inventory.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Data
{
    public class SupabaseSyncService
    {
        private readonly HttpClient _http;
        private readonly DatabaseService _db;
        private readonly string _baseUrl;
        private readonly string _apiKey;

        public SupabaseSyncService()
        {
            _http = new HttpClient();
            _db = new DatabaseService();
            _baseUrl = App.SupabaseUrl;
            _apiKey = App.SupabaseKey;

            // Set default headers
            _http.DefaultRequestHeaders.Add("apikey", _apiKey);
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        // ===== CHECK INTERNET =====
        //public async Task<bool> IsOnlineAsync()
        //{
        //    try
        //    {
        //        using var client = new HttpClient();
        //        client.Timeout = TimeSpan.FromSeconds(5);
        //        var response = await client.GetAsync("https://www.google.com");
        //        return response.IsSuccessStatusCode;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public async Task<bool> IsOnlineAsync()
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(10);
                // Use Supabase directly instead of Google for faster response
                var response = await client.GetAsync($"{_baseUrl}/rest/v1/");
                return response.IsSuccessStatusCode ||
                       response.StatusCode == System.Net.HttpStatusCode.Unauthorized;
            }
            catch
            {
                return false;
            }
        }

        // ===== SYNC ALL =====
        public async Task<SyncResult> SyncAllAsync()
        {
            var result = new SyncResult();

            if (!await IsOnlineAsync())
            {
                result.Message = "Offline — sync skipped";
                result.IsOnline = false;
                return result;
            }

            result.IsOnline = true;

            try
            {
                result.StocksSynced = await SyncStocksAsync();
                result.RequestsSynced = await SyncRequestsAsync();
                result.ReturnsSynced = await SyncReturnsAsync();
                result.Message = $"Synced ✅ Stocks: {result.StocksSynced} | Requests: {result.RequestsSynced} | Returns: {result.ReturnsSynced}";
            }
            catch (Exception ex)
            {
                result.Message = $"Sync error: {ex.Message}";
            }

            return result;
        }

        // ===== PULL ALL FROM SUPABASE =====
        public async Task<SyncResult> PullAllAsync()
        {
            var result = new SyncResult();

            if (!await IsOnlineAsync())
            {
                result.Message = "Offline — pull skipped";
                result.IsOnline = false;
                return result;
            }

            result.IsOnline = true;

            try
            {
                result.RequestsSynced = await PullRequestsAsync();
                result.ReturnsSynced = await PullReturnsAsync();
                result.Message = $"Pulled ✅ Requests: {result.RequestsSynced} | Returns: {result.ReturnsSynced}";
            }
            catch (Exception ex)
            {
                result.Message = $"Pull error: {ex.Message}";
            }

            return result;
        }

        // ===== PULL REQUESTS FROM SUPABASE =====
        private async Task<int> PullRequestsAsync()
        {
            var url = $"{_baseUrl}/rest/v1/requests?select=*&order=created_at.desc";
            var response = await _http.GetAsync(url);

            if (!response.IsSuccessStatusCode) return 0;

            var json = await response.Content.ReadAsStringAsync();
            var items = System.Text.Json.JsonSerializer.Deserialize<List<System.Text.Json.JsonElement>>(json);

            if (items == null) return 0;

            int count = 0;
            using var db = new AppDbContext();

            foreach (var item in items)
            {
                var id = Guid.Parse(item.GetProperty("id").GetString());

                // Check if already exists locally
                var exists = await db.Requests.FindAsync(id);
                if (exists != null) continue;

                // Insert new record from Supabase
                var entity = new Inventory.Models.RequestEntity
                {
                    Id = id,
                    RequestedItems = item.GetProperty("requested_items").GetString(),
                    RequestedBy = item.GetProperty("requested_by").GetString(),
                    Quantity = item.GetProperty("quantity").GetInt32(),
                    Date = DateTime.Parse(item.GetProperty("date").GetString()),
                    Status = item.GetProperty("status").GetString(),
                    IsSynced = true,
                    CreatedAt = DateTime.Parse(item.GetProperty("created_at").GetString())
                };

                db.Requests.Add(entity);
                count++;
            }

            await db.SaveChangesAsync();
            return count;
        }

        // ===== PULL RETURNS FROM SUPABASE =====
        private async Task<int> PullReturnsAsync()
        {
            var url = $"{_baseUrl}/rest/v1/returns?select=*&order=created_at.desc";
            var response = await _http.GetAsync(url);

            if (!response.IsSuccessStatusCode) return 0;

            var json = await response.Content.ReadAsStringAsync();
            var items = System.Text.Json.JsonSerializer.Deserialize<List<System.Text.Json.JsonElement>>(json);

            if (items == null) return 0;

            int count = 0;
            using var db = new AppDbContext();

            foreach (var item in items)
            {
                var id = Guid.Parse(item.GetProperty("id").GetString());

                var exists = await db.Returns.FindAsync(id);
                if (exists != null) continue;

                var entity = new Inventory.Models.ReturnEntity
                {
                    Id = id,
                    ItemName = item.GetProperty("item_name").GetString(),
                    ReturnedBy = item.GetProperty("returned_by").GetString(),
                    Quantity = item.GetProperty("quantity").GetInt32(),
                    Reason = item.GetProperty("reason").GetString(),
                    Date = DateTime.Parse(item.GetProperty("date").GetString()),
                    Status = item.GetProperty("status").GetString(),
                    IsSynced = true,
                    CreatedAt = DateTime.Parse(item.GetProperty("created_at").GetString())
                };

                db.Returns.Add(entity);
                count++;
            }

            await db.SaveChangesAsync();
            return count;
        }


        // ===== SYNC STOCKS =====
        private async Task<int> SyncStocksAsync()
        {
            var unsynced = await _db.GetUnsyncedStocksAsync();
            int count = 0;

            foreach (var stock in unsynced)
            {
                var payload = new
                {
                    id = stock.Id.ToString(),
                    serial_number = stock.SerialNumber,
                    model_number = stock.ModelNumber,
                    product_name = stock.ProductName,
                    category = stock.Category,
                    added_by = stock.AddedBy,
                    warranty = stock.Warranty,
                    date_added = stock.DateAdded.ToString("yyyy-MM-dd"),
                    is_synced = true
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                content.Headers.Add("Prefer", "resolution=merge-duplicates");

                var response = await _http.PostAsync(
                    $"{_baseUrl}/rest/v1/stocks?on_conflict=id",
                    content);

                if (response.IsSuccessStatusCode)
                {
                    // Mark as synced in local DB
                    using var db = new AppDbContext();
                    var entity = await db.Stocks.FindAsync(stock.Id);
                    if (entity != null)
                    {
                        entity.IsSynced = true;
                        await db.SaveChangesAsync();
                    }
                    count++;
                }
            }

            return count;
        }

        // ===== SYNC REQUESTS =====
        private async Task<int> SyncRequestsAsync()
        {
            var unsynced = await _db.GetUnsyncedRequestsAsync();
            int count = 0;

            foreach (var request in unsynced)
            {
                var payload = new
                {
                    id = request.Id.ToString(),
                    requested_items = request.RequestedItems,
                    requested_by = request.RequestedBy,
                    quantity = request.Quantity,
                    date = request.Date.ToString("yyyy-MM-dd"),
                    status = request.Status,
                    is_synced = true
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                content.Headers.Add("Prefer", "resolution=merge-duplicates");

                var response = await _http.PostAsync(
                    $"{_baseUrl}/rest/v1/requests?on_conflict=id",
                    content);

                if (response.IsSuccessStatusCode)
                {
                    using var db = new AppDbContext();
                    var entity = await db.Requests.FindAsync(request.Id);
                    if (entity != null)
                    {
                        entity.IsSynced = true;
                        await db.SaveChangesAsync();
                    }
                    count++;
                }
            }

            return count;
        }

        // ===== SYNC RETURNS =====
        private async Task<int> SyncReturnsAsync()
        {
            var unsynced = await _db.GetUnsyncedReturnsAsync();
            int count = 0;

            foreach (var ret in unsynced)
            {
                var payload = new
                {
                    id = ret.Id.ToString(),
                    item_name = ret.ItemName,
                    returned_by = ret.ReturnedBy,
                    quantity = ret.Quantity,
                    reason = ret.Reason,
                    date = ret.Date.ToString("yyyy-MM-dd"),
                    status = ret.Status,
                    is_synced = true
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                content.Headers.Add("Prefer", "resolution=merge-duplicates");

                var response = await _http.PostAsync(
                    $"{_baseUrl}/rest/v1/returns?on_conflict=id",
                    content);

                if (response.IsSuccessStatusCode)
                {
                    using var db = new AppDbContext();
                    var entity = await db.Returns.FindAsync(ret.Id);
                    if (entity != null)
                    {
                        entity.IsSynced = true;
                        await db.SaveChangesAsync();
                    }
                    count++;
                }
            }

            return count;
        }
    }

    // ===== SYNC RESULT MODEL =====
    public class SyncResult
    {
        public bool IsOnline { get; set; }
        public int StocksSynced { get; set; }
        public int RequestsSynced { get; set; }
        public int ReturnsSynced { get; set; }
        public string Message { get; set; }
    }
}