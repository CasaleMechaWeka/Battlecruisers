using BattleCruisers.Cruisers.Drones;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones
{
    public class PvPDroneConsumerProvider : IPvPDroneConsumerProvider
    {
        private IDroneManager _droneManager;

        public PvPDroneConsumerProvider(IDroneManager droneManager)
        {
            Assert.IsNotNull(droneManager);
            _droneManager = droneManager;
        }

        public IDroneConsumer RequestDroneConsumer(int numOfDronesRequired)
        {
            return new PvPDroneConsumer(numOfDronesRequired, _droneManager);
        }

        public void ActivateDroneConsumer(IDroneConsumer droneConsumer)
        {
            // Logging.LogMethod(Tags.DRONE_CONSUMER_PROVIDER);

            Assert.IsTrue(_droneManager.CanSupportDroneConsumer(droneConsumer.NumOfDronesRequired));
            Assert.IsFalse(_droneManager.HasDroneConsumer(droneConsumer));

            _droneManager.AddDroneConsumer(droneConsumer);
        }

        public void ReleaseDroneConsumer(IDroneConsumer droneConsumer)
        {
            // Logging.LogMethod(Tags.DRONE_CONSUMER_PROVIDER);

            if (_droneManager.HasDroneConsumer(droneConsumer))
            {
                _droneManager.RemoveDroneConsumer(droneConsumer);
            }
        }
    }
}
