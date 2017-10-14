using UnityEngine.Assertions;

namespace BattleCruisers.Drones
{
	public class DroneConsumerProvider : IDroneConsumerProvider
	{
		private IDroneManager _droneManager;

		public DroneConsumerProvider(IDroneManager droneManager)
		{
			_droneManager = droneManager;
		}

		public IDroneConsumer RequestDroneConsumer(int numOfDronesRequired, bool isHighPriority)
		{
            return new DroneConsumer(numOfDronesRequired, isHighPriority);
		}

		public void ActivateDroneConsumer(IDroneConsumer droneConsumer)
		{
			Assert.IsTrue(_droneManager.CanSupportDroneConsumer(droneConsumer.NumOfDronesRequired));
            Assert.IsFalse(_droneManager.HasDroneConsumer(droneConsumer));

			_droneManager.AddDroneConsumer(droneConsumer);
		}

		public void ReleaseDroneConsumer(IDroneConsumer droneConsumer)
		{
            if (_droneManager.HasDroneConsumer(droneConsumer))
            {
                _droneManager.RemoveDroneConsumer(droneConsumer);
			}
		}
	}
}
