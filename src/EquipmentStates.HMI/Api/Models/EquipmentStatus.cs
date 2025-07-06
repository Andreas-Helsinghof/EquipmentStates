namespace EquipmentStates.HMI.Api.Models
{
    public enum EquipmentProductionState
    {
        Red,    // Standing still
        Yellow, // Starting up/winding down
        Green   // Producing normally
    }

    public class EquipmentStatus
    {
        public EquipmentProductionState State { get; set; }
        public string OrderId { get; set; }
    }
}
