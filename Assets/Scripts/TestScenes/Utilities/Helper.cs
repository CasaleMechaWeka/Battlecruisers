using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.TargetFinders;
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
			ITargetFinderFactory targetFinderFactory = null)
		{
			if (parentCruiser == null)
			{
				parentCruiser = CreateCruiser();
			}

			if (enemyCruiser == null)
			{
				enemyCruiser = CreateCruiser();
			}

			buildable.Initialise(
				faction,
				uiManager,
				parentCruiser,
				enemyCruiser,
				buildableFactory,
				targetFinderFactory);
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

		public ITargetFinderFactory CreateTargetFinderFactory(ITargetFinder targetFinder)
		{
			return new Mock.TargetFinderFactory() 
			{
				BomberTargetFinder = targetFinder
			};
		}
		
		public ITargetFinder CreateTargetFinder(IFactionable target)
		{
			return new Mock.TargetFinder() 
			{
				Target = target
			};
		}

		public IFactionable CreateFactionObject(GameObject gameObject = null)
		{
			return new Mock.FactionObject() 
			{
				GameObject = gameObject
			};
		}
	}
}
