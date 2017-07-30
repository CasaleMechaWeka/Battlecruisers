using UnityEngine.Assertions;

namespace BattleCruisers.Drones
{
    public interface IDroneConsumerProvider
	{
		IDroneConsumer RequestDroneConsumer(int numOfDronesRequired);
		void ActivateDroneConsumer(IDroneConsumer droneConsumer);
		void ReleaseDroneConsumer(IDroneConsumer droneConsumer);
	}

	public class DroneConsumerProvider : IDroneConsumerProvider
	{
		private IDroneManager _droneManager;

		public DroneConsumerProvider(IDroneManager droneManager)
		{
			_droneManager = droneManager;
		}

		public IDroneConsumer RequestDroneConsumer(int numOfDronesRequired)
		{
			return new DroneConsumer(numOfDronesRequired);
		}

		public void ActivateDroneConsumer(IDroneConsumer droneConsumer)
		{
			Assert.IsTrue(_droneManager.CanSupportDroneConsumer(droneConsumer.NumOfDronesRequired));
			_droneManager.AddDroneConsumer(droneConsumer);
		}

		public void ReleaseDroneConsumer(IDroneConsumer droneConsumer)
		{
			_droneManager.RemoveDroneConsumer(droneConsumer);
		}
	}
}
