using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace EquipmentStates.OrdersAPI.Models
{
    public class OrderPart
    {
        public Guid PartId { get; set; }
        public string PartName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }

    public class OrderDetails
    {
        public Guid OrderId { get; set; }
        public Guid EquipmentId { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<OrderPart> Parts { get; set; }
        public DateTime ScheduledTime { get; set; }
    }

    public class RegisterEquipmentStateRequest
    {
        public Guid EquipmentId { get; set; }
        public string SSEurl { get; set; } = string.Empty;
    }

}
