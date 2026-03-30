using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<StockEntity> Stocks { get; set; }
        public DbSet<RequestEntity> Requests { get; set; }
        public DbSet<ReturnEntity> Returns { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(App.LocalConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Stocks table mapping
            modelBuilder.Entity<StockEntity>(entity =>
            {
                entity.ToTable("stocks");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.SerialNumber).HasColumnName("serial_number");
                entity.Property(e => e.ModelNumber).HasColumnName("model_number");
                entity.Property(e => e.ProductName).HasColumnName("product_name");
                entity.Property(e => e.Category).HasColumnName("category");
                entity.Property(e => e.AddedBy).HasColumnName("added_by");
                entity.Property(e => e.Warranty).HasColumnName("warranty");
                entity.Property(e => e.DateAdded).HasColumnName("date_added");
                entity.Property(e => e.IsSynced).HasColumnName("is_synced");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            });

            // Requests table mapping
            modelBuilder.Entity<RequestEntity>(entity =>
            {
                entity.ToTable("requests");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.RequestedItems).HasColumnName("requested_items");
                entity.Property(e => e.RequestedBy).HasColumnName("requested_by");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
                entity.Property(e => e.Date).HasColumnName("date");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.IsSynced).HasColumnName("is_synced");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            });

            // Returns table mapping
            modelBuilder.Entity<ReturnEntity>(entity =>
            {
                entity.ToTable("returns");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ItemName).HasColumnName("item_name");
                entity.Property(e => e.ReturnedBy).HasColumnName("returned_by");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
                entity.Property(e => e.Reason).HasColumnName("reason");
                entity.Property(e => e.Date).HasColumnName("date");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.IsSynced).HasColumnName("is_synced");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            });
        }
    }
}