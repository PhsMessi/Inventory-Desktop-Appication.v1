using System;

namespace Inventory.Models
{
    public class RequestEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string RequestedItems { get; set; }
        public string RequestedBy { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; } = DateTime.Today;
        public string Status { get; set; } = "Pending";
        public bool IsSynced { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}