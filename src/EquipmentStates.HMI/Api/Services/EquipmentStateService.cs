
using EquipmentStates.HMI.Api.Models;
using EquipmentStates.HMI.MachineStateChangeHandler;
using System;

namespace EquipmentStates.HMI.Api.Services
{
    public class EquipmentStateService
    {
        public EquipmentStateService(IMachineEvents machineEvents)
        {
            // Subscribe to the emergency stop event
            machineEvents.onMachineEmergencyStop += HandleEmergencyStop;
        }

        private void HandleEmergencyStop(string message)
        {
            SetState((EquipmentProductionState) (((int)_currentStatus.State + 1) % 2) );
        }

        private EquipmentStatus _currentStatus = new EquipmentStatus
        {
            State = EquipmentProductionState.Red,
            OrderId = "N/A"
        };

        // Event for SSE subscribers and hub
        public event Action<EquipmentStatus>? OnStateChanged;

        public EquipmentStatus GetState()
        {
            return _currentStatus;
        }

        public void SetState(EquipmentProductionState state, string orderId)
        {
            Console.WriteLine($"state change {state}, {orderId}");
            _currentStatus.State = state;
            _currentStatus.OrderId = orderId;
            OnStateChanged?.Invoke(_currentStatus);
        }

        public void SetState(EquipmentProductionState state)
        {
            Console.WriteLine($"state change {state}");
            _currentStatus.State = state;
            OnStateChanged?.Invoke(_currentStatus);
        }
    }
}
