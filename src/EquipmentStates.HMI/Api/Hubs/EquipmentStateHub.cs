using EquipmentStates.HMI.Api.Models;
using EquipmentStates.HMI.Api.Services;
using Microsoft.AspNetCore.SignalR;

namespace EquipmentStates.HMI.Api.Hubs
{
    public class EquipmentStateHub : Hub
    {
        private readonly EquipmentStateService _service;
        private static IHubContext<EquipmentStateHub>? _hubContext;

        public EquipmentStateHub(EquipmentStateService service, IHubContext<EquipmentStateHub> hubContext)
        {
            _service = service;
            if (_hubContext == null)
            {
                _hubContext = hubContext;
                _service.OnStateChanged += BroadcastStateChanged;
            }
        }

        private static void BroadcastStateChanged(EquipmentStatus state)
        {
            _hubContext?.Clients.All.SendAsync("StateChanged", state);
        }
    }
}
