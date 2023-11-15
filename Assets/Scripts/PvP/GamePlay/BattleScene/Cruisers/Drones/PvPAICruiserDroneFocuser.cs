using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones
{
    public class PvPAICruiserDroneFocuser : IPvPDroneFocuser
    {
        private readonly IPvPDroneManager _droneManager;

#pragma warning disable 67  // Unused event
        // Never the case for AI cruiser
        public event EventHandler PlayerTriggeredRepair;
#pragma warning restore 67  // Unused event

        public PvPAICruiserDroneFocuser(IPvPDroneManager droneManager)
        {
            Assert.IsNotNull(droneManager);
            _droneManager = droneManager;
        }

        public PvPPrioritisedSoundKey ToggleDroneConsumerFocus(IPvPDroneConsumer droneConsumer, bool isTriggeredByPlayer)
        {
            _droneManager.ToggleDroneConsumerFocus(droneConsumer);
            return null;
        }
    }
}