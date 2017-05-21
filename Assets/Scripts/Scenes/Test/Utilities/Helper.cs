using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.UI;
using BattleCruisers.Units.Aircraft;
using BattleCruisers.Units.Aircraft.Providers;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Utilities
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
			IPrefabFactory prefabFactory = null,
			ITargetsFactory targetsFactory = null,
			IAircraftProvider aircraftProvider = null,
			Direction parentCruiserDirection = Direction.Right)
		{
			if (parentCruiser == null)
			{
				parentCruiser = CreateCruiser(_numOfDrones, parentCruiserDirection);
			}

			if (enemyCruiser == null)
			{
				enemyCruiser = CreateCruiser(_numOfDrones, Direction.Left);
			}

			if (targetsFactory == null)
			{
				targetsFactory = new TargetsFactory(enemyCruiser);
			}

			if (aircraftProvider == null)
			{
				aircraftProvider = CreateAircraftProvider();
			}

			buildable.Initialise(
				faction,
				uiManager,
				parentCruiser,
				enemyCruiser,
				prefabFactory,
				targetsFactory,
				aircraftProvider);
		}

		private ICruiser CreateCruiser(int numOfDrones, Direction facingDirection)
		{
			IDroneConsumer droneConsumer = Substitute.For<IDroneConsumer>();
			droneConsumer.NumOfDrones = NUM_OF_DRONES;
			droneConsumer.State.Returns(DroneConsumerState.Active);

			IDroneConsumerProvider droneConsumerProvider = Substitute.For<IDroneConsumerProvider>();
			droneConsumerProvider.RequestDroneConsumer(0).ReturnsForAnyArgs(callInfo =>
			{
				droneConsumer.NumOfDronesRequired.Returns(callInfo.Arg<int>());
				return droneConsumer;
			});

			ICruiser cruiser = Substitute.For<ICruiser>();
			cruiser.DroneConsumerProvider.Returns(droneConsumerProvider);
			cruiser.Direction.Returns(facingDirection);

			return cruiser;
		}

		/// <summary>
		/// Target processors only assign the specified target once, and then chill forever.
		/// </summary>
		public ITargetsFactory CreateTargetsFactory(GameObject globalTarget)
		{
			// The enemy cruiser is added as a target by the global target finder.
			// So pretend the cruiser game object is the specified target.
			ICruiser enemyCruiser = Substitute.For<ICruiser>();
			enemyCruiser.GameObject.Returns(globalTarget);

			ITargetFinder targetFinder = new GlobalTargetFinder(enemyCruiser);
			ITargetProcessor targetProcessor = new TargetProcessor(targetFinder, new EqualTargetRanker());
			ITargetsFactory targetsFactory = Substitute.For<ITargetsFactory>();

			targetsFactory.BomberTargetProcessor.Returns(targetProcessor);
			targetsFactory.OffensiveTurretTargetProcessor.Returns(targetProcessor);
			targetsFactory.CreateRangedTargetFinder(null).ReturnsForAnyArgs(targetFinder);
			targetsFactory.CreateTargetProcessor(null, null).ReturnsForAnyArgs(targetProcessor);

			return targetsFactory;
		}

		/// <summary>
		/// Target processors never assign any targets to any target consumers.
		/// </summary>
		public ITargetsFactory CreateTargetsFactory()
		{
			return Substitute.For<ITargetsFactory>();
		}

		public IAircraftProvider CreateAircraftProvider(
			IList<Vector2> bomberPatrolPoints = null,
			IList<Vector2> fighterPatrolPoints = null,
			SafeZone fighterSafeZone = null)
		{
			IAircraftProvider provider = Substitute.For<IAircraftProvider>();

			if (bomberPatrolPoints == null)
			{
				bomberPatrolPoints = new List<Vector2>() 
				{
					new Vector2(0, 1),
					new Vector2(0, 2)
				};
			}
			provider.FindBomberPatrolPoints(0).ReturnsForAnyArgs(bomberPatrolPoints);

			if (fighterPatrolPoints == null)
			{
				fighterPatrolPoints = new List<Vector2>() 
				{
					new Vector2(0, 1),
					new Vector2(0, 2)
				};
			}
			provider.FindFighterPatrolPoints(0).ReturnsForAnyArgs(fighterPatrolPoints);

			if (fighterSafeZone == null)
			{
				fighterSafeZone = new SafeZone(
					minX: float.MinValue,
					maxX: float.MaxValue,
					minY: float.MinValue,
					maxY: float.MaxValue);
			}
			provider.FighterSafeZone.Returns(fighterSafeZone);
			
			return provider;
		}
	}
}
