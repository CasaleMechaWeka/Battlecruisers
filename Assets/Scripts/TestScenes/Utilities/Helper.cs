using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.UI;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes.Utilities
{
	public class Helper
	{
		private readonly int _numOfDrones;

		// Massive, so buildables build instantly
		private const int NUM_OF_DRONES = 100;

		public Helper(int numOfDrones = NUM_OF_DRONES)
		{
			_numOfDrones = numOfDrones;
		}

		public void InitialiseBuildable(
			Buildable buildable,
			Faction faction = Faction.Blues,
			UIManager uiManager = null,
			ICruiser parentCruiser = null,
			ICruiser enemyCruiser = null,
			BuildableFactory buildableFactory = null,
			ITargetsFactory targetsFactory = null)
		{
			if (parentCruiser == null)
			{
				parentCruiser = CreateCruiser(_numOfDrones);
			}

			if (enemyCruiser == null)
			{
				enemyCruiser = CreateCruiser(_numOfDrones);
			}

			if (targetsFactory == null)
			{
				targetsFactory = new TargetsFactory(enemyCruiser);
			}

			buildable.Initialise(
				faction,
				uiManager,
				parentCruiser,
				enemyCruiser,
				buildableFactory,
				targetsFactory);
		}

		private ICruiser CreateCruiser(int numOfDrones)
		{
			IDroneConsumer droneConsumer = Substitute.For<IDroneConsumer>();
			droneConsumer.NumOfDrones = NUM_OF_DRONES;

			IDroneConsumerProvider droneConsumerProvider = Substitute.For<IDroneConsumerProvider>();
			droneConsumerProvider.RequestDroneConsumer(0).ReturnsForAnyArgs(droneConsumer);

			ICruiser cruiser = Substitute.For<ICruiser>();
			cruiser.DroneConsumerProvider.Returns(droneConsumerProvider);

			return cruiser;
		}

		public ITargetsFactory CreateTargetsFactory(GameObject globalTarget)
		{
			// The enemy cruiser is added as a target by the global target finder.
			// So pretend the cruiser game object is the specified target.
			ICruiser enemyCruiser = Substitute.For<ICruiser>();
			enemyCruiser.GameObject.Returns(globalTarget);

			ITargetFinder targetFinder = new GlobalTargetFinder(enemyCruiser);
			ITargetProcessor targetProcessor = new TargetProcessor(targetFinder);
			ITargetsFactory targetsFactory = Substitute.For<ITargetsFactory>();
			targetsFactory.GlobalTargetProcessor.Returns(targetProcessor);

			return targetsFactory;
		}
	}
}
