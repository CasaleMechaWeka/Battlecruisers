using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.TargetFinders;
using BattleCruisers.TargetFinders.Filters;
using BattleCruisers.UI;
using NSubstitute;
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

		private ICruiser CreateCruiser(int numOfDrones = NUM_OF_DRONES)
		{
			IDroneConsumer droneConsumer = Substitute.For<IDroneConsumer>();
			droneConsumer.NumOfDrones = NUM_OF_DRONES;

			IDroneConsumerProvider droneConsumerProvider = Substitute.For<IDroneConsumerProvider>();
			droneConsumerProvider.RequestDroneConsumer(0).ReturnsForAnyArgs(droneConsumer);

			ICruiser cruiser = Substitute.For<ICruiser>();
			cruiser.DroneConsumerProvider.Returns(droneConsumerProvider);

			return cruiser;
		}

		public ITargetFinderFactory CreateTargetFinderFactory(IFactionable bomberTarget)
		{
			ITargetFinder targetFinder = Substitute.For<ITargetFinder>();
			targetFinder.FindTarget().Returns(bomberTarget);

			ITargetFinderFactory targetFinderFactory = Substitute.For<ITargetFinderFactory>();
			targetFinderFactory.BomberTargetFinder.Returns(targetFinder);
			return targetFinderFactory;
		}
	}
}
