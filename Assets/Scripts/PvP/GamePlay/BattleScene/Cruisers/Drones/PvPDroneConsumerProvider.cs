using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones
{
    public class PvPDroneConsumerProvider : IPvPDroneConsumerProvider
    {
        private IPvPDroneManager _droneManager;

        public PvPDroneConsumerProvider(IPvPDroneManager droneManager)
        {
            Assert.IsNotNull(droneManager);
            _droneManager = droneManager;
        }

        public IPvPDroneConsumer RequestDroneConsumer(int numOfDronesRequired)
        {
            return new PvPDroneConsumer(numOfDronesRequired, _droneManager);
        }

        public void ActivateDroneConsumer(IPvPDroneConsumer droneConsumer)
        {
            // Logging.LogMethod(Tags.DRONE_CONSUMER_PROVIDER);

            Assert.IsTrue(_droneManager.CanSupportDroneConsumer(droneConsumer.NumOfDronesRequired));
            Assert.IsFalse(_droneManager.HasDroneConsumer(droneConsumer));

            _droneManager.AddDroneConsumer(droneConsumer);
        }

        public void ReleaseDroneConsumer(IPvPDroneConsumer droneConsumer)
        {
            // Logging.LogMethod(Tags.DRONE_CONSUMER_PROVIDER);

            if (_droneManager.HasDroneConsumer(droneConsumer))
            {
                _droneManager.RemoveDroneConsumer(droneConsumer);
            }
        }
    }
}
