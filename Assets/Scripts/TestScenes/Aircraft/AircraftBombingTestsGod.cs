using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
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

		// Massive, so bombers built instantly
		private const int NUM_OF_DRONES = 100;

		void Start() 
		{
			ITargetFinderFactory targetFinderFactoryForBomberOnRight = new Mock.TargetFinderFactory() 
			{
				BomberTargetFinder = new Mock.TargetFinder()
				{
					Target = targetToLeft
				}
			};

			IDroneConsumer droneConsumer = new Mock.DroneConsumer() 
			{
				State = DroneConsumerState.Active,
				NumOfDrones = NUM_OF_DRONES
			};
			IDroneConsumerProvider droneConsumerProvider = new Mock.DroneConsumerProvider() 
			{
				DroneConsumer = droneConsumer
			};
			ICruiser parentCruiser = new Mock.Cruiser() 
			{
				DroneConsumerProvider = droneConsumerProvider
			};

			bomberToRight.Initialise(
				Faction.Blues,
				null,
				parentCruiser,
				null,
				null,
				targetFinderFactoryForBomberOnRight);

			bomberToRight.StartConstruction();
		}
	}
}
