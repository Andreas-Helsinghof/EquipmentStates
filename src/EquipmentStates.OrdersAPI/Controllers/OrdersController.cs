using EquipmentStates.OrdersAPI.Models;
using EquipmentStates.OrdersAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentStates.OrdersAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersService _ordersService;
        public OrdersController(OrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [HttpGet("OrderDetails")]
        public ActionResult<OrderDetails> GetOrderDetails([FromQuery] Guid orderId)
        {
            var order = _ordersService.GetOrderDetails(orderId);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpGet("Orders")]
        public ActionResult<IEnumerable<OrderDetails>> GetOrders([FromQuery] Guid equipmentId)
        {
            var orders = _ordersService.GetOrdersForEquipment(equipmentId);
            return Ok(orders);
        }

    }
}
