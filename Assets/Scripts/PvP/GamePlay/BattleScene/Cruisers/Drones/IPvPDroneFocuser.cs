using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.Sound;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones
{
    public interface IPvPDroneFocuser
    {
        event EventHandler PlayerTriggeredRepair;
        PrioritisedSoundKey ToggleDroneConsumerFocus(IDroneConsumer droneConsumer, bool isTriggeredByPlayer);
    }
}