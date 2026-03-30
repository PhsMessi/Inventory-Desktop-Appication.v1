using System;

namespace Inventory.Models
{
    public class StockEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string SerialNumber { get; set; }
        public string ModelNumber { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public string AddedBy { get; set; }
        public string Warranty { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Today;
        public bool IsSynced { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}