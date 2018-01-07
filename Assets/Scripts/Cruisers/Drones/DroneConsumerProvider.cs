using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones
{
	public class DroneConsumerProvider : IDroneConsumerProvider
	{
		private IDroneManager _droneManager;

		public DroneConsumerProvider(IDroneManager droneManager)
		{
            Assert.IsNotNull(droneManager);
			_droneManager = droneManager;
		}

		public IDroneConsumer RequestDroneConsumer(int numOfDronesRequired, bool isHighPriority)
		{
            return new DroneConsumer(numOfDronesRequired, isHighPriority);
		}

		public void ActivateDroneConsumer(IDroneConsumer droneConsumer)
		{
            Logging.Log(Tags.DRONE_CONSUMER_PROVIDER, "ActivateDroneConsumer()");

			Assert.IsTrue(_droneManager.CanSupportDroneConsumer(droneConsumer.NumOfDronesRequired));
            Assert.IsFalse(_droneManager.HasDroneConsumer(droneConsumer));

			_droneManager.AddDroneConsumer(droneConsumer);
		}

		public void ReleaseDroneConsumer(IDroneConsumer droneConsumer)
		{
            Logging.Log(Tags.DRONE_CONSUMER_PROVIDER, "ReleaseDroneConsumer()");

            if (_droneManager.HasDroneConsumer(droneConsumer))
            {
                _droneManager.RemoveDroneConsumer(droneConsumer);
			}
		}
	}
}
