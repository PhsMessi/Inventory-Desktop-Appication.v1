using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Data
{
    public class DatabaseService
    {
        // ===== STOCKS =====
        public async Task<List<StockEntity>> GetAllStocksAsync()
        {
            using var db = new AppDbContext();
            return await db.Stocks.OrderByDescending(s => s.CreatedAt).ToListAsync();
        }

        public async Task AddStockAsync(StockEntity stock)
        {
            using var db = new AppDbContext();
            db.Stocks.Add(stock);
            await db.SaveChangesAsync();
        }

        public async Task UpdateStockAsync(StockEntity stock)
        {
            using var db = new AppDbContext();
            db.Stocks.Update(stock);
            await db.SaveChangesAsync();
        }

        public async Task DeleteStockAsync(StockEntity stock)
        {
            using var db = new AppDbContext();
            db.Stocks.Remove(stock);
            await db.SaveChangesAsync();
        }

        public async Task<List<StockEntity>> GetUnsyncedStocksAsync()
        {
            using var db = new AppDbContext();
            return await db.Stocks.Where(s => !s.IsSynced).ToListAsync();
        }

        // ===== REQUESTS =====
        public async Task<List<RequestEntity>> GetAllRequestsAsync()
        {
            using var db = new AppDbContext();
            return await db.Requests.OrderByDescending(r => r.CreatedAt).ToListAsync();
        }

        public async Task AddRequestAsync(RequestEntity request)
        {
            using var db = new AppDbContext();
            db.Requests.Add(request);
            await db.SaveChangesAsync();
        }

        public async Task UpdateRequestAsync(RequestEntity request)
        {
            using var db = new AppDbContext();
            db.Requests.Update(request);
            await db.SaveChangesAsync();
        }

        public async Task<List<RequestEntity>> GetUnsyncedRequestsAsync()
        {
            using var db = new AppDbContext();
            return await db.Requests.Where(r => !r.IsSynced).ToListAsync();
        }

        // ===== RETURNS =====
        public async Task<List<ReturnEntity>> GetAllReturnsAsync()
        {
            using var db = new AppDbContext();
            return await db.Returns.OrderByDescending(r => r.CreatedAt).ToListAsync();
        }

        public async Task AddReturnAsync(ReturnEntity returnItem)
        {
            using var db = new AppDbContext();
            db.Returns.Add(returnItem);
            await db.SaveChangesAsync();
        }

        public async Task UpdateReturnAsync(ReturnEntity returnItem)
        {
            using var db = new AppDbContext();
            db.Returns.Update(returnItem);
            await db.SaveChangesAsync();
        }

        public async Task<List<ReturnEntity>> GetUnsyncedReturnsAsync()
        {
            using var db = new AppDbContext();
            return await db.Returns.Where(r => !r.IsSynced).ToListAsync();
        }
    }
}