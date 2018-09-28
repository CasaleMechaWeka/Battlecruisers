using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones
{
    public class SimpleDroneFocuser : IDroneFocuser
    {
        private readonly IDroneManager _droneManager;

        public SimpleDroneFocuser(IDroneManager droneManager)
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