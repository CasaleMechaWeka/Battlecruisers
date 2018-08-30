using BattleCruisers.Utils.DataStrctures;
using System;

namespace BattleCruisers.Cruisers.Drones
{
    public interface IDroneManager
    {
        int NumOfDrones { get; set; }
        IReadonlyObservableCollection<IDroneConsumer> DroneConsumers { get; }

        event EventHandler<DroneNumChangedEventArgs> DroneNumChanged;

        bool CanSupportDroneConsumer(int numOfDronesRequired);
        bool HasDroneConsumer(IDroneConsumer droneConsumer);
        void AddDroneConsumer(IDroneConsumer droneConsumer);
        void RemoveDroneConsumer(IDroneConsumer droneConsumer);
        void ToggleDroneConsumerFocus(IDroneConsumer droneConsumer);
    }
}
