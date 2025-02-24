using BattleCruisers.Cruisers.Drones;
using System;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones
{
    public interface IPvPDroneManager
    {
        int NumOfDrones { get; set; }
        ReadOnlyObservableCollection<IDroneConsumer> DroneConsumers { get; }

        event EventHandler<DroneNumChangedEventArgs> DroneNumChanged;

        bool CanSupportDroneConsumer(int numOfDronesRequired);
        bool HasDroneConsumer(IDroneConsumer droneConsumer);
        void AddDroneConsumer(IDroneConsumer droneConsumer);
        void RemoveDroneConsumer(IDroneConsumer droneConsumer);
        void ToggleDroneConsumerFocus(IDroneConsumer droneConsumer);
    }
}
