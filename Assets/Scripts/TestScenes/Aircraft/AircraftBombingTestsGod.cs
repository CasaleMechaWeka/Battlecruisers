using BattleCruisers.Buildables;
using BattleCruisers.Drones;
using BattleCruisers.TargetFinders;
using BattleCruisers.TestScenes;
using BattleCruisers.Units.Aircraft;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.TestScenes.Aircraft
{
	public class AircraftBombingTestsGod : MonoBehaviour 
	{
		public BomberController bomberToLeft;
		public BomberController bomberToRight;

		public GameObject targetToLeft;
		public GameObject targetToRight;

		void Start() 
		{
			ITargetFinderFactory targetFinderFactoryForBomberOnRight = new Mock.TargetFinderFactory() 
			{
				BomberTargetFinder = new Mock.TargetFinder()
				{
					Target = targetToLeft
				}
			};

			IDroneConsumer droneConsumer = new Mock.DroneConsumer();
			IDroneConsumerProvider droneConsumerProvider = new Mock.DroneConsumerProvider();

			bomberToRight.Initialise(
				Faction.Blues,
				null,
				null,
				null,
				null,
				targetFinderFactoryForBomberOnRight);


		}
	}
}
