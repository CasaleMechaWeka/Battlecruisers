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

		public IDroneConsumer RequestDroneConsumer(int numOfDronesRequired)
		{
            return new DroneConsumer(numOfDronesRequired, _droneManager);
		}

		public void ActivateDroneConsumer(IDroneConsumer droneConsumer)
		{
            Logging.LogMethod(Tags.DRONE_CONSUMER_PROVIDER);

			Assert.IsTrue(_droneManager.CanSupportDroneConsumer(droneConsumer.NumOfDronesRequired));
            Assert.IsFalse(_droneManager.HasDroneConsumer(droneConsumer));

			_droneManager.AddDroneConsumer(droneConsumer);
		}

		public void ReleaseDroneConsumer(IDroneConsumer droneConsumer)
		{
            Logging.LogMethod(Tags.DRONE_CONSUMER_PROVIDER);

            if (_droneManager.HasDroneConsumer(droneConsumer))
            {
                _droneManager.RemoveDroneConsumer(droneConsumer);
			}
		}
	}
}
