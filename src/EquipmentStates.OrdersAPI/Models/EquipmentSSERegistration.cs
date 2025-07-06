using System;

namespace EquipmentStates.OrdersAPI.Models
{
    public class EquipmentSSERegistration
    {
        public Guid EquipmentId { get; set; }
        public string SSEurl { get; set; } = string.Empty;
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    }
}
