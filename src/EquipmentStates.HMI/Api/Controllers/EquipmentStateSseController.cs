using EquipmentStates.HMI.Api.Models;
using EquipmentStates.HMI.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace EquipmentStates.HMI.Api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EquipmentStateSseController : ControllerBase
    {
        private readonly EquipmentStateService _service;
        private static readonly List<HttpResponse> _subscribers = new();

        public EquipmentStateSseController(EquipmentStateService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task Stream()
        {
            Response.Headers["Content-Type"] = "text/event-stream";
            Response.Headers["Cache-Control"] = "no-cache";
            Response.Headers["Connection"] = "keep-alive";
            Response.Headers["Access-Control-Allow-Origin"] = "*";

            void Handler(EquipmentStatus state)
            {
                var json = System.Text.Json.JsonSerializer.Serialize(state);
                var data = $"{json}\n\n";
                var bytes = Encoding.UTF8.GetBytes(data);
                Response.Body.WriteAsync(bytes, 0, bytes.Length);
                Response.Body.FlushAsync();
            }

            _service.OnStateChanged += Handler;

            // Send initial state
            Handler(_service.GetState());

            try
            {
                // Keep the connection open
                await Task.Delay(-1, HttpContext.RequestAborted);
            }
            catch (TaskCanceledException) { }
            finally
            {
                _service.OnStateChanged -= Handler;
            }
        }
    }
}
