using BattleCruisers.Drones;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes.Mock
{
	public class DroneConsumerProvider : IDroneConsumerProvider
	{
		public IDroneConsumer DroneConsumer { get; set; }

		public IDroneConsumer RequestDroneConsumer(int numOfDronesRequired)
		{
			return DroneConsumer;
		}

		public void ActivateDroneConsumer(IDroneConsumer droneConsumer)
		{
		}

		public void ReleaseDroneConsumer(IDroneConsumer droneConsumer)
		{
		}
	}
}
