using EquipmentStates.OrdersAPI.Models;
using EquipmentStates.OrdersAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentStates.OrdersAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EquipmentSSEController : ControllerBase
    {
        private readonly EquipmentSSEService _sseService;
        public EquipmentSSEController(EquipmentSSEService sseService)
        {
            _sseService = sseService;
        }

        [HttpPost("RegisterEquipmentState")]
        public IActionResult RegisterEquipmentState([FromBody] RegisterEquipmentStateRequest request)
        {
            if (request.EquipmentId == Guid.Empty || string.IsNullOrWhiteSpace(request.SSEurl))
                return BadRequest("EquipmentId and SSEurl are required.");
            _sseService.RegisterEquipmentSSE(request);
            return Ok();
        }
    }
}
