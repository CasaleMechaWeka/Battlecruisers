using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones
{
    public class AICruiserDroneFocuser : IDroneFocuser
    {
        private readonly IDroneManager _droneManager;

        // Never the case for AI cruiser
        public event EventHandler PlayerTriggeredRepair;

        public AICruiserDroneFocuser(IDroneManager droneManager)
        {
            Assert.IsNotNull(droneManager);
            _droneManager = droneManager;
        }

        public void ToggleDroneConsumerFocus(IDroneConsumer droneConsumer, bool isTriggeredByPlayer)
        {
            _droneManager.ToggleDroneConsumerFocus(droneConsumer);
        }
    }
}