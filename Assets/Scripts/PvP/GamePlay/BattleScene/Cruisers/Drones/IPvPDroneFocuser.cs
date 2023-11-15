using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones
{
    public interface IPvPDroneFocuser
    {
        event EventHandler PlayerTriggeredRepair;
        PvPPrioritisedSoundKey ToggleDroneConsumerFocus(IPvPDroneConsumer droneConsumer, bool isTriggeredByPlayer);
    }
}