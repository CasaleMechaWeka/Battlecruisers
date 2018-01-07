using System;

namespace BattleCruisers.Drones
{
    public interface IDroneManager
    {
        int NumOfDrones { get; set; }

        event EventHandler<DroneNumChangedEventArgs> DroneNumChanged;

        bool CanSupportDroneConsumer(int numOfDronesRequired);
        bool HasDroneConsumer(IDroneConsumer droneConsumer);
        void AddDroneConsumer(IDroneConsumer droneConsumer);
        void RemoveDroneConsumer(IDroneConsumer droneConsumer);
        void ToggleDroneConsumerFocus(IDroneConsumer droneConsumer);
    }
}
