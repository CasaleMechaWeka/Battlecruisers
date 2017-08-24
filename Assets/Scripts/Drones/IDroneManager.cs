using System;
using System.Collections.ObjectModel;

namespace BattleCruisers.Drones
{
    public interface IDroneManager
    {
        int NumOfDrones { get; set; }
        // Unity does not have the ReadOnlyObservableCollection<T> class, so
        // I have to implement the parts I want myself :)
        ReadOnlyCollection<IDroneConsumer> DroneConsumers { get; }

        // Emitted every time a drone consumer is added or removed.
        event EventHandler DroneConsumersChanged;
        event EventHandler<DroneNumChangedEventArgs> DroneNumChanged;

        bool CanSupportDroneConsumer(int numOfDronesRequired);
        void AddDroneConsumer(IDroneConsumer droneConsumer);
        void RemoveDroneConsumer(IDroneConsumer droneConsumer);
        void ToggleDroneConsumerFocus(IDroneConsumer droneConsumer);
    }
}
