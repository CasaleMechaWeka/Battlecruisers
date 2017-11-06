using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Explosions;
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

        public void InitialiseBuilding(
            IBuilding building,
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
            IBoostFactory boostFactory = null,
            IBoostProvidersManager boostProvidersManager = null,
            IDamageApplierFactory damageApplierFactory = null,
            Direction parentCruiserDirection = Direction.Right,
            ISlot parentSlot = null,
            IExplosionFactory explosionFactory = null,
            IAccuracyAdjusterFactory accuracyAdjusterFactory = null)
        {
            BuildableInitialisationArgs args
                = new BuildableInitialisationArgs(
                    this,
                    faction,
                    uiManager,
                    parentCruiser,
                    enemyCruiser,
                    aircraftProvider,
                    prefabFactory,
                    targetsFactory,
                    movementControllerFactory,
                    angleCalculatorFactory,
                    targetPositionPredictorFactory,
                    flightPointsProviderFactory,
                    boostFactory,
                    boostProvidersManager,
                    damageApplierFactory,
                    parentCruiserDirection,
                    explosionFactory,
                    accuracyAdjusterFactory);

            InitialiseBuilding(building, args, parentSlot);
        }

        public void InitialiseBuilding(
            IBuilding building,
            BuildableInitialisationArgs initialisationArgs,
            ISlot parentSlot = null)
        {
            building.StaticInitialise();
            building.Initialise(
                initialisationArgs.ParentCruiser,
                initialisationArgs.EnemyCruiser,
                initialisationArgs.UiManager,
                initialisationArgs.FactoryProvider,
                parentSlot ?? CreateParentSlot());
        }

        public void InitialiseUnit(
			IUnit unit,
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
            IBoostFactory boostFactory = null,
            IBoostProvidersManager boostProvidersManager = null,
            IDamageApplierFactory damageApplierFactory = null,
            Direction parentCruiserDirection = Direction.Right,
            IExplosionFactory explosionFactory = null,
            IAccuracyAdjusterFactory accuracyAdjusterFactory = null)
		{
			BuildableInitialisationArgs args
				= new BuildableInitialisationArgs(
					this,
					faction,
					uiManager,
					parentCruiser,
					enemyCruiser,
					aircraftProvider,
					prefabFactory,
					targetsFactory,
					movementControllerFactory,
					angleCalculatorFactory,
					targetPositionPredictorFactory,
					flightPointsProviderFactory,
                    boostFactory,
                    boostProvidersManager,
                    damageApplierFactory,
					parentCruiserDirection,
                    explosionFactory,
                    accuracyAdjusterFactory);

            InitialiseUnit(unit, args);
		}

        public void InitialiseUnit(
            IUnit unit,
            BuildableInitialisationArgs initialisationArgs)
        {
            unit.StaticInitialise();
            unit.Initialise(
                initialisationArgs.ParentCruiser,
                initialisationArgs.EnemyCruiser,
                initialisationArgs.UiManager,
                initialisationArgs.FactoryProvider);
        }

		public ICruiser CreateCruiser(Direction facingDirection, Faction faction)
		{
			IDroneConsumer droneConsumer = Substitute.For<IDroneConsumer>();
            droneConsumer.NumOfDrones = _numOfDrones;
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

		private ISlot CreateParentSlot()
		{
			ISlot parentSlot = Substitute.For<ISlot>();

            ReadOnlyCollection<IBoostProvider> boostProviders = new ReadOnlyCollection<IBoostProvider>(new List<IBoostProvider>());
			parentSlot.BoostProviders.Items.Returns(boostProviders);

			ReadOnlyCollection<ISlot> neighbouringSlots = new ReadOnlyCollection<ISlot>(new List<ISlot>());
			parentSlot.NeighbouringSlots.Returns(neighbouringSlots);

			return parentSlot;
		}

        public IBarrelControllerArgs CreateBarrelControllerArgs(
            BarrelController barrel,
            ITargetFilter targetFilter = null,
            ITargetPositionPredictor targetPositionPredictor = null,
            IAngleCalculator angleCalculator = null,
            IAccuracyAdjuster accuracyAdjuster = null,
            IRotationMovementController rotationMovementController = null,
            IFactoryProvider factoryProvider = null)
        {
            return
                new BarrelControllerArgs(
                    targetFilter ?? Substitute.For<ITargetFilter>(),
                    targetPositionPredictor ?? new DummyTargetPositionpredictor(),
                    angleCalculator ?? new AngleCalculator(),
                    accuracyAdjuster ?? new DummyAccuracyAdjuster(),
                    rotationMovementController ?? new RotationMovementController(new RotationHelper(), barrel.TurretStats.TurretRotateSpeedInDegrees, barrel.transform),
                    factoryProvider ?? new BuildableInitialisationArgs(this).FactoryProvider);
        }

        public IAccuracyAdjusterFactory CreateDummyAccuracyAdjuster()
        {
			IAccuracyAdjusterFactory factory = Substitute.For<IAccuracyAdjusterFactory>();

            IAccuracyAdjuster dummyAccuracyAdjuster = new DummyAccuracyAdjuster();

            factory.CreateVerticalImpactProjectileAdjuster(null, 0, 0).ReturnsForAnyArgs(dummyAccuracyAdjuster);
            factory.CreateHorizontalImpactProjectileAdjuster(null, 0, 0).ReturnsForAnyArgs(dummyAccuracyAdjuster);
            factory.CreateDummyAdjuster().ReturnsForAnyArgs(dummyAccuracyAdjuster);

            return factory;
        }
	}
}
