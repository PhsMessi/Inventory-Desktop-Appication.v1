using System;

namespace Inventory.Models
{
    public class ReturnEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ItemName { get; set; }
        public string ReturnedBy { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; }
        public DateTime Date { get; set; } = DateTime.Today;
        public string Status { get; set; } = "Not Yet";
        public bool IsSynced { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}