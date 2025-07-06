namespace EquipmentStates.HMI.MachineStateChangeHandler
{
    public interface IMachineEvents
    {
        public event Action<string> onMachineEmergencyStop;
    }
}