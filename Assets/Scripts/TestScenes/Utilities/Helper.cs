using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.TargetFinders;
using BattleCruisers.TargetFinders.Filters;
using BattleCruisers.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes.Utilities
{
	public class Helper
	{
		// Massive, so buildables build instantly
		private const int NUM_OF_DRONES = 100;
		
		public void InitialiseBuildable(
			Buildable buildable,
			Faction faction = Faction.Blues,
			UIManager uiManager = null,
			ICruiser parentCruiser = null,
			ICruiser enemyCruiser = null,
			BuildableFactory buildableFactory = null,
			ITargetFinderFactory targetFinderFactory = null,
			ITargetFilterFactory filterFactory = null)
		{
			if (parentCruiser == null)
			{
				parentCruiser = CreateCruiser();
			}

			if (enemyCruiser == null)
			{
				enemyCruiser = CreateCruiser();
			}

			if (filterFactory == null)
			{
				filterFactory = new TargetFilterFactory();
			}

			buildable.Initialise(
				faction,
				uiManager,
				parentCruiser,
				enemyCruiser,
				buildableFactory,
				targetFinderFactory,
				filterFactory);
		}

		public ICruiser CreateCruiser(int numOfDrones = NUM_OF_DRONES)
		{
			return new Mock.Cruiser() 
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
		}
	}
}
