using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones
{
    public interface IPvPDroneFocuser
    {
        event EventHandler PlayerTriggeredRepair;
        void ToggleDroneConsumerFocus(IPvPDroneConsumer droneConsumer, bool isTriggeredByPlayer);
    }
}