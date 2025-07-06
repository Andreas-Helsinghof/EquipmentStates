using EquipmentStates.OrdersAPI.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace EquipmentStates.OrdersAPI.Services
{
    public class OrdersService
    {
        private static readonly List<OrderDetails> Orders = new()
        {
            new OrderDetails {
                OrderId = Guid.NewGuid(),
                EquipmentId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Description = "Order 1",
                ScheduledTime = DateTime.UtcNow.AddHours(1),
                Parts = new List<OrderPart>
                {
                    new OrderPart { PartId = Guid.NewGuid(), PartName = "MarioWheel", Quantity = 5 },
                }
            },
            new OrderDetails {
                OrderId = Guid.NewGuid(),
                EquipmentId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Description = "Order 2",
                ScheduledTime = DateTime.UtcNow.AddHours(2),
                Parts = new List<OrderPart>
                {
                    new OrderPart { PartId = Guid.NewGuid(), PartName = "FlowerPedal", Quantity = 15 },
                }
            },
            new OrderDetails {
                OrderId = Guid.NewGuid(),
                EquipmentId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Description = "Order 3",
                ScheduledTime = DateTime.UtcNow.AddHours(3),
                Parts = new List<OrderPart>
                {
                    new OrderPart { PartId = Guid.NewGuid(), PartName = "TransparantBall", Quantity = 50 }
                }
            }
        };

        public OrderDetails? GetOrderDetails(Guid orderId) => Orders.Find(o => o.OrderId == orderId);

        public IEnumerable<OrderDetails> GetOrdersForEquipment(Guid equipmentId) => Orders.FindAll(o => o.EquipmentId == equipmentId);
    }
}
