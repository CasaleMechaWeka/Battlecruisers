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

		public GameObject target;

		// Massive, so bombers built instantly
		private const int NUM_OF_DRONES = 100;

		void Start() 
		{
			ICruiser parentCruiser = new Mock.Cruiser() 
			{
				DroneConsumerProvider = new Mock.DroneConsumerProvider() 
				{
					DroneConsumer = new Mock.DroneConsumer() 
					{
						State = DroneConsumerState.Active,
						NumOfDrones = NUM_OF_DRONES
					}
				}
			};

			ITargetFinderFactory targetFinderFactory = new Mock.TargetFinderFactory() 
			{
				BomberTargetFinder = new Mock.TargetFinder()
				{
					Target = target
				}
			};

			bomberToRight.Initialise(
				Faction.Blues,
				null,
				parentCruiser,
				null,
				null,
				targetFinderFactory);
			bomberToRight.StartConstruction();

			bomberToLeft.Initialise(
				Faction.Blues,
				null,
				parentCruiser,
				null,
				null,
				targetFinderFactory);
			bomberToLeft.StartConstruction();
		}
	}
}
