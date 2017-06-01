using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.Units.Aircraft;
using BattleCruisers.Units.Aircraft.Providers;
using BattleCruisers.Utils;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BcUtils = BattleCruisers.Utils;

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
			IAircraftProvider aircraftProvider = null,
			IPrefabFactory prefabFactory = null,
			ITargetsFactory targetsFactory = null,
			IMovementControllerFactory movementControllerFactory = null,
			IAngleCalculatorFactory angleCalculatorFactory = null,
			ITargetPositionPredictorFactory targetPositionPredictorFactory = null,
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
			
			if (aircraftProvider == null)
			{
				aircraftProvider = CreateAircraftProvider();
			}

			if (targetsFactory == null)
			{
				targetsFactory = new TargetsFactory(enemyCruiser);
			}

			if (movementControllerFactory == null)
			{
				movementControllerFactory = new MovementControllerFactory();
			}

			if (angleCalculatorFactory == null)
			{
				angleCalculatorFactory = new AngleCalculatorFactory();
			}

			if (targetPositionPredictorFactory == null)
			{
				targetPositionPredictorFactory = new TargetPositionPredictorFactory();
			}

			IFactoryProvider factoryProvider = CreateFactoryProvider(prefabFactory, targetsFactory, 
				movementControllerFactory, angleCalculatorFactory, targetPositionPredictorFactory);

			buildable.Initialise(
				faction,
				uiManager,
				parentCruiser,
				enemyCruiser,
				factoryProvider,
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
			cruiser.AttackCapabilities.Returns(new List<TargetType>());

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
			enemyCruiser.Position.Returns(x => (Vector2)globalTarget.transform.position);

			ITargetFinder targetFinder = new GlobalTargetFinder(enemyCruiser);
			ITargetProcessor targetProcessor = new TargetProcessor(targetFinder, new EqualTargetRanker());
			ITargetsFactory targetsFactory = Substitute.For<ITargetsFactory>();

			targetsFactory.BomberTargetProcessor.Returns(targetProcessor);
			targetsFactory.OffensiveTurretTargetProcessor.Returns(targetProcessor);
			targetsFactory.CreateRangedTargetFinder(null, null).ReturnsForAnyArgs(targetFinder);
			targetsFactory.CreateTargetProcessor(null, null).ReturnsForAnyArgs(targetProcessor);

			return targetsFactory;
		}

		public IFactoryProvider CreateFactoryProvider(
			IPrefabFactory prefabFactory, 
			ITargetsFactory targetsFactory, 
			IMovementControllerFactory movementControllerFactory,
			IAngleCalculatorFactory angleCalculatorFactory,
			ITargetPositionPredictorFactory targetPositionControllerFactory)
		{
			IFactoryProvider factoryProvider = Substitute.For<IFactoryProvider>();

			factoryProvider.PrefabFactory.Returns(prefabFactory);
			factoryProvider.TargetsFactory.Returns(targetsFactory);
			factoryProvider.MovementControllerFactory.Returns(movementControllerFactory);
			factoryProvider.AngleCalculatorFactory.Returns(angleCalculatorFactory);
			factoryProvider.TargetPositionPredictorFactory.Returns(targetPositionControllerFactory);

			return factoryProvider;
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
