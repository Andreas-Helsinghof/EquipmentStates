using EquipmentStates.HMI.Api.Models;
using EquipmentStates.HMI.Api.Services;
using Microsoft.AspNetCore.SignalR;
using EquipmentStates.HMI.Api.Hubs;
// using EquipmentStates.HMI.Api.Hubs; // Already included, but ensure correct reference
using Microsoft.AspNetCore.Mvc;

namespace EquipmentStates.HMI.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentStateController(EquipmentStateService service) : ControllerBase
    {
        private readonly EquipmentStateService _service = service;
        private readonly IHubContext<EquipmentStates.HMI.Api.Hubs.EquipmentStateHub> _hubContext;

        [HttpGet]
        public ActionResult<EquipmentStatus> Get()
        {
            return Ok(_service.GetState());
        }

        [HttpPost]
        public IActionResult Set([FromBody] EquipmentStatus status)
        {
            _service.SetState(status.State, status.OrderId);
            return NoContent();
        }
    }
}
