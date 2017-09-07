using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.Utils;
using NSubstitute;
using UnityEngine;
using BcUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class Helper
	{
		private readonly int _numOfDrones;

		// Massive, so buildables build instantly
		private const int NUM_OF_DRONES = 2000;

		public Helper(int numOfDrones = NUM_OF_DRONES)
		{
			_numOfDrones = numOfDrones;
		}

		public void InitialiseBuildable(
			IBuildable buildable,
			Faction faction = Faction.Blues,
			IUIManager uiManager = null,
			ICruiser parentCruiser = null,
			ICruiser enemyCruiser = null,
			IAircraftProvider aircraftProvider = null,
			IPrefabFactory prefabFactory = null,
			ITargetsFactory targetsFactory = null,
			IMovementControllerFactory movementControllerFactory = null,
			IAngleCalculatorFactory angleCalculatorFactory = null,
			ITargetPositionPredictorFactory targetPositionPredictorFactory = null,
			IFlightPointsProviderFactory flightPointsProviderFactory = null,
			Direction parentCruiserDirection = Direction.Right)
		{
			buildable.StaticInitialise();

			if (parentCruiser == null)
			{
				parentCruiser = CreateCruiser(_numOfDrones, parentCruiserDirection, faction);
			}

			if (enemyCruiser == null)
			{
				enemyCruiser = CreateCruiser(_numOfDrones, Direction.Left, BcUtils.Helper.GetOppositeFaction(faction));
			}

            if (uiManager == null)
            {
                uiManager = Substitute.For<IUIManager>();
            }

			if (aircraftProvider == null)
			{
				aircraftProvider = CreateAircraftProvider();
			}

			if (targetsFactory == null)
			{
				targetsFactory = new TargetsFactory(enemyCruiser);
			}

			if (angleCalculatorFactory == null)
			{
				angleCalculatorFactory = new AngleCalculatorFactory();
			}

			if (targetPositionPredictorFactory == null)
			{
				targetPositionPredictorFactory = new TargetPositionPredictorFactory();
			}
			
			if (movementControllerFactory == null)
			{
				movementControllerFactory = new MovementControllerFactory(angleCalculatorFactory, targetPositionPredictorFactory);
			}

			if (prefabFactory == null)
			{
				prefabFactory = new PrefabFactory(new PrefabFetcher());
			}

			if (flightPointsProviderFactory == null)
			{
				flightPointsProviderFactory = new FlightPointsProviderFactory();
			}

			IFactoryProvider factoryProvider = CreateFactoryProvider(prefabFactory, targetsFactory, movementControllerFactory, 
				angleCalculatorFactory, targetPositionPredictorFactory, aircraftProvider, flightPointsProviderFactory);

			buildable.Initialise(
				parentCruiser,
				enemyCruiser,
				uiManager,
				factoryProvider);
		}

		private ICruiser CreateCruiser(int numOfDrones, Direction facingDirection, Faction faction)
		{
			IDroneConsumer droneConsumer = Substitute.For<IDroneConsumer>();
            droneConsumer.NumOfDrones = numOfDrones;
			droneConsumer.State.Returns(DroneConsumerState.Active);

			IDroneConsumerProvider droneConsumerProvider = Substitute.For<IDroneConsumerProvider>();
			droneConsumerProvider.RequestDroneConsumer(-99, true).ReturnsForAnyArgs(callInfo =>
			{
				droneConsumer.NumOfDronesRequired.Returns(callInfo.Arg<int>());
				return droneConsumer;
			});

			ICruiser cruiser = Substitute.For<ICruiser>();
			cruiser.DroneConsumerProvider.Returns(droneConsumerProvider);
			cruiser.Direction.Returns(facingDirection);
			cruiser.AttackCapabilities.Returns(new List<TargetType>());
			cruiser.Faction.Returns(faction);

			return cruiser;
		}

		public ICruiser CreateCruiser(GameObject target)
		{
			ICruiser enemyCruiser = Substitute.For<ICruiser>();
			enemyCruiser.GameObject.Returns(target);
			enemyCruiser.Position.Returns(x => (Vector2)target.transform.position);
			return enemyCruiser;
		}

		/// <summary>
		/// Target processors only assign the specified target once, and then chill forever.
		/// </summary>
		public ITargetsFactory CreateTargetsFactory(GameObject globalTarget, ITargetFilter targetFilter = null, IExactMatchTargetFilter exactMatchTargetFilter = null)
		{
			// The enemy cruiser is added as a target by the global target finder.
			// So pretend the cruiser game object is the specified target.
			ICruiser enemyCruiser = Substitute.For<ICruiser>();
			enemyCruiser.GameObject.Returns(globalTarget);
			enemyCruiser.Position.Returns(x => (Vector2)globalTarget.transform.position);

			ITargetFinder targetFinder = new GlobalTargetFinder(enemyCruiser);
			ITargetProcessor targetProcessor = new TargetProcessor(targetFinder, new EqualTargetRanker());
			ITargetsFactory targetsFactory = Substitute.For<ITargetsFactory>();

            if (exactMatchTargetFilter == null)
            {
                exactMatchTargetFilter = new ExactMatchTargetFilter();
            }

			targetsFactory.BomberTargetProcessor.Returns(targetProcessor);
			targetsFactory.OffensiveBuildableTargetProcessor.Returns(targetProcessor);
			targetsFactory.CreateRangedTargetFinder(null, null).ReturnsForAnyArgs(targetFinder);
			targetsFactory.CreateTargetProcessor(null, null).ReturnsForAnyArgs(targetProcessor);
			targetsFactory.CreateTargetFilter(Faction.Reds, new List<TargetType>()).ReturnsForAnyArgs(targetFilter);
			targetsFactory.CreateExactMatchTargetFilter().Returns(exactMatchTargetFilter);
			targetsFactory.CreateExactMatchTargetFilter(null).ReturnsForAnyArgs(exactMatchTargetFilter);
            targetsFactory.CreateDummyTargetFilter(true).ReturnsForAnyArgs(new DummyTargetFilter(isMatchResult: true));

			return targetsFactory;
		}

		public IFactoryProvider CreateFactoryProvider(
			IPrefabFactory prefabFactory, 
			ITargetsFactory targetsFactory, 
			IMovementControllerFactory movementControllerFactory,
			IAngleCalculatorFactory angleCalculatorFactory,
			ITargetPositionPredictorFactory targetPositionControllerFactory,
			IAircraftProvider aircraftProvider,
			IFlightPointsProviderFactory flightPointsProviderFactory)
		{
			IFactoryProvider factoryProvider = Substitute.For<IFactoryProvider>();

			factoryProvider.PrefabFactory.Returns(prefabFactory);
			factoryProvider.TargetsFactory.Returns(targetsFactory);
			factoryProvider.MovementControllerFactory.Returns(movementControllerFactory);
			factoryProvider.AngleCalculatorFactory.Returns(angleCalculatorFactory);
			factoryProvider.TargetPositionPredictorFactory.Returns(targetPositionControllerFactory);
			factoryProvider.AircraftProvider.Returns(aircraftProvider);
			factoryProvider.FlightPointsProviderFactory.Returns(flightPointsProviderFactory);

			return factoryProvider;
		}

		public IAircraftProvider CreateAircraftProvider(
			IList<Vector2> bomberPatrolPoints = null,
            IList<Vector2> gunshipPatrolPoints = null,
			IList<Vector2> fighterPatrolPoints = null,
			IList<Vector2> deathstarPatrolPoints = null,
            IList<Vector2> spySatellitePatrolPoints = null,
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

            if (gunshipPatrolPoints == null)
			{
				gunshipPatrolPoints = new List<Vector2>()
				{
					new Vector2(0, 1),
					new Vector2(0, 2)
				};
			}
            provider.FindGunshipPatrolPoints(0).ReturnsForAnyArgs(gunshipPatrolPoints);

			if (fighterPatrolPoints == null)
			{
				fighterPatrolPoints = new List<Vector2>() 
				{
					new Vector2(0, 1),
					new Vector2(0, 2)
				};
			}
			provider.FindFighterPatrolPoints(0).ReturnsForAnyArgs(fighterPatrolPoints);

			if (deathstarPatrolPoints == null)
			{
				deathstarPatrolPoints = new List<Vector2>() 
				{
					new Vector2(0, 1),
					new Vector2(0, 2),
					new Vector2(0, 3),
					new Vector2(0, 4)
				};
			}
			provider.FindDeathstarPatrolPoints(default(Vector2), 0).ReturnsForAnyArgs(deathstarPatrolPoints);

            if (spySatellitePatrolPoints == null)
            {
                spySatellitePatrolPoints = new List<Vector2>()
                {
					new Vector2(0, 1),
					new Vector2(0, 2),
					new Vector2(0, 3)
                };
            }
            provider.FindSpySatellitePatrolPoints(default(Vector2), 0).ReturnsForAnyArgs(spySatellitePatrolPoints);

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
