using System;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones
{
    public interface IPvPDroneManager
    {
        int NumOfDrones { get; set; }
        ReadOnlyObservableCollection<IPvPDroneConsumer> DroneConsumers { get; }

        event EventHandler<PvPDroneNumChangedEventArgs> DroneNumChanged;

        bool CanSupportDroneConsumer(int numOfDronesRequired);
        bool HasDroneConsumer(IPvPDroneConsumer droneConsumer);
        void AddDroneConsumer(IPvPDroneConsumer droneConsumer);
        void RemoveDroneConsumer(IPvPDroneConsumer droneConsumer);
        void ToggleDroneConsumerFocus(IPvPDroneConsumer droneConsumer);
    }
}
