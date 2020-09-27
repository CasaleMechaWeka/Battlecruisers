using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Buildables.Pools;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Helpers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityCommon.PlatformAbstractions;
using UnityCommon.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class Helper
	{
        private readonly int _numOfDrones;
        private readonly float _buildSpeedMultiplier;
        
        public IDeferrer Deferrer { get; }
        public IDeferrer RealTimeDeferrer { get; }

        public IUpdaterProvider UpdaterProvider { get; }

        private IPrefabFactory _prefabFactory;
        public IPrefabFactory PrefabFactory
        {
            get
            {
                Assert.IsNotNull(_prefabFactory);
                return _prefabFactory;
            }
        }

        public Helper(
            int numOfDrones, 
            float buildSpeedMultiplier,
            IDeferrer deferrer,
            IDeferrer realTimeDeferrer,
            IUpdaterProvider updaterProvider,
            IPrefabFactory prefabFactory)
		{
            _numOfDrones = numOfDrones;
            _buildSpeedMultiplier = buildSpeedMultiplier;
            Deferrer = deferrer;
            RealTimeDeferrer = realTimeDeferrer;
            UpdaterProvider = updaterProvider;
            _prefabFactory = prefabFactory;
		}

        public Helper(
            Helper helper,
            int? numOfDrones = null,
            float? buildSpeedMultiplier = null,
            IDeferrer deferrer = null,
            IUpdaterProvider updaterProvider = null,
            IPrefabFactory prefabFactory = null)
        {
            _numOfDrones = numOfDrones ?? helper._numOfDrones;
            _buildSpeedMultiplier = buildSpeedMultiplier ?? helper._buildSpeedMultiplier;
            Deferrer = deferrer ?? helper.Deferrer;
            UpdaterProvider = updaterProvider ?? helper.UpdaterProvider;
            _prefabFactory = prefabFactory ?? helper._prefabFactory;
        }

        public void InitialiseBuilding(
            IBuilding building,
            Faction faction = Faction.Blues,
            IUIManager uiManager = null,
            ICruiser parentCruiser = null,
            ICruiser enemyCruiser = null,
            IAircraftProvider aircraftProvider = null,
            ITargetFactories targetFactories = null,
            IMovementControllerFactory movementControllerFactory = null,
            IAngleCalculatorFactory angleCalculatorFactory = null,
            ITargetPositionPredictorFactory targetPositionPredictorFactory = null,
            IFlightPointsProviderFactory flightPointsProviderFactory = null,
            IBoostFactory boostFactory = null,
            IGlobalBoostProviders globalBoostProviders = null,
            IDamageApplierFactory damageApplierFactory = null,
            Direction parentCruiserDirection = Direction.Right,
            ISlot parentSlot = null,
            IAccuracyAdjusterFactory accuracyAdjusterFactory = null,
            ITargetPositionValidatorFactory targetPositionValidatorFactory = null,
            IUserChosenTargetManager userChosenTargetManager = null)
        {
            BuildableInitialisationArgs args
                = new BuildableInitialisationArgs(
                    this,
                    faction,
                    uiManager,
                    parentCruiser,
                    enemyCruiser,
                    aircraftProvider,
                    targetFactories,
                    movementControllerFactory,
                    angleCalculatorFactory,
                    targetPositionPredictorFactory,
                    flightPointsProviderFactory,
                    boostFactory,
                    globalBoostProviders,
                    damageApplierFactory,
                    parentCruiserDirection,
                    accuracyAdjusterFactory,
                    targetPositionValidatorFactory,
                    deferrer: Deferrer,
                    updaterProvider: UpdaterProvider,
                    userChosenTargetManager: userChosenTargetManager);

            InitialiseBuilding(building, args, parentSlot);
        }

        public void InitialiseBuilding(
            IBuilding building,
            BuildableInitialisationArgs initialisationArgs,
            ISlot parentSlot = null)
        {
            BuildingWrapper buildingWrapper = building.GameObject.GetComponentInInactiveParent<BuildingWrapper>();
            HealthBarController healthBar = buildingWrapper.GetComponentInChildren<HealthBarController>(includeInactive: true);
            building.StaticInitialise(buildingWrapper.gameObject, healthBar);
            building.Initialise(initialisationArgs.UiManager, initialisationArgs.FactoryProvider);
            building.Activate(
                new BuildingActivationArgs(
                    initialisationArgs.ParentCruiser,
                    initialisationArgs.EnemyCruiser,
                    initialisationArgs.CruiserSpecificFactories,
                    parentSlot ?? CreateParentSlot(),
                    Substitute.For<IDoubleClickHandler<IBuilding>>()));
        }

        public void InitialiseUnit(
			IUnit unit,
			Faction faction = Faction.Blues,
			IUIManager uiManager = null,
			ICruiser parentCruiser = null,
			ICruiser enemyCruiser = null,
			IAircraftProvider aircraftProvider = null,
            ITargetFactories targetFactories = null,
            IMovementControllerFactory movementControllerFactory = null,
			IAngleCalculatorFactory angleCalculatorFactory = null,
			ITargetPositionPredictorFactory targetPositionPredictorFactory = null,
			IFlightPointsProviderFactory flightPointsProviderFactory = null,
            IBoostFactory boostFactory = null,
            IGlobalBoostProviders globalBoostProviders = null,
            IDamageApplierFactory damageApplierFactory = null,
            Direction parentCruiserDirection = Direction.Right,
            IAccuracyAdjusterFactory accuracyAdjusterFactory = null,
            IUserChosenTargetManager userChosenTargetManager = null,
            bool showDroneFeedback = false)
		{
            BuildableInitialisationArgs args
                = new BuildableInitialisationArgs(
                    this,
                    faction,
                    uiManager,
                    parentCruiser,
                    enemyCruiser,
                    aircraftProvider,
                    targetFactories,
                    movementControllerFactory,
                    angleCalculatorFactory,
                    targetPositionPredictorFactory,
                    flightPointsProviderFactory,
                    boostFactory,
                    globalBoostProviders,
                    damageApplierFactory,
                    parentCruiserDirection,
                    accuracyAdjusterFactory,
                    userChosenTargetManager: userChosenTargetManager,
                    deferrer: Deferrer,
                    updaterProvider: UpdaterProvider,
                    showDroneFeedback: showDroneFeedback);

            InitialiseUnit(unit, args);
		}

        public void InitialiseUnit(
            IUnit unit,
            BuildableInitialisationArgs initialisationArgs)
        {
            UnitWrapper unitWrapper = unit.GameObject.GetComponentInInactiveParent<UnitWrapper>();
            HealthBarController healthBar = unitWrapper.GetComponentInChildren<HealthBarController>(includeInactive: true);
            unit.StaticInitialise(unitWrapper.gameObject, healthBar);
            unit.Initialise(initialisationArgs.UiManager, initialisationArgs.FactoryProvider);
            unit.Activate(
                new BuildableActivationArgs(
                    initialisationArgs.ParentCruiser,
                    initialisationArgs.EnemyCruiser,
                    initialisationArgs.CruiserSpecificFactories));
        }

		public ICruiser CreateCruiser(Direction facingDirection, Faction faction)
        {
            IDroneConsumerProvider droneConsumerProvider = CreateDroneConsumerProvider();

            IBuildProgressCalculator buildProgressCalculator = new LinearCalculator(_buildSpeedMultiplier);

            ICruiser cruiser = Substitute.For<ICruiser>();
            cruiser.DroneConsumerProvider.Returns(droneConsumerProvider);
            cruiser.Direction.Returns(facingDirection);
            cruiser.AttackCapabilities.Returns(new ReadOnlyCollection<TargetType>(new List<TargetType>()));
            cruiser.Faction.Returns(faction);
            cruiser.BuildProgressCalculator.Returns(buildProgressCalculator);
            cruiser.Size.Returns(new Vector2(5, 2));
            IUnitTargets unitTargets = new UnitTargets(cruiser.UnitMonitor);

            cruiser.UnitTargets.Returns(unitTargets);

            float xPosition = facingDirection == Direction.Right ? -35 : 35;
            cruiser.Position.Returns(new Vector2(xPosition, 0));

            return cruiser;
        }

        public ICruiser CreateCruiser(GameObject target)
		{
			ICruiser enemyCruiser = Substitute.For<ICruiser>();
			enemyCruiser.GameObject.Returns(target);
			enemyCruiser.Position.Returns(x => (Vector2)target.transform.position);
            enemyCruiser.AttackCapabilities.Returns(new ReadOnlyCollection<TargetType>(new List<TargetType>()));
			return enemyCruiser;
        }

        public ITargetFactories CreateTargetFactories(
            GameObject globalTarget, 
            ICruiser parentCruiser = null,
            ICruiser enemyCruiser = null,
            IUpdaterProvider updaterProvider = null,
            ITargetFilter targetFilter = null, 
            IExactMatchTargetFilter exactMatchTargetFilter = null)
        {
            // The enemy cruiser is added as a target by the global target finder.
            // So pretend the cruiser game object is the specified target.
            ICruiser dummyEnemyCruiser = Substitute.For<ICruiser>();
            dummyEnemyCruiser.GameObject.Returns(globalTarget);
            dummyEnemyCruiser.Position.Returns(x => (Vector2)globalTarget.transform.position);

            GlobalTargetFinder targetFinder = new GlobalTargetFinder(dummyEnemyCruiser);
            IRankedTargetTracker targetTracker = new RankedTargetTracker(targetFinder, new EqualTargetRanker());
            ITargetProcessor targetProcessor = new TargetProcessor(targetTracker);
            ITargetFactories targetFactories = Substitute.For<ITargetFactories>();
            ITargetFactoriesProvider targetFactoriesProvider = Substitute.For<ITargetFactoriesProvider>();
            targetFactories.TargetFactoriesProvider.Returns(targetFactoriesProvider);
            targetFinder.EmitCruiserAsGlobalTarget();

            if (exactMatchTargetFilter == null)
            {
                exactMatchTargetFilter = new ExactMatchTargetFilter();
            }

            // Processors
            targetFactories.TargetProcessorFactory.BomberTargetProcessor.Returns(targetProcessor);
            targetFactories.TargetProcessorFactory.OffensiveBuildableTargetProcessor.Returns(targetProcessor);
            targetFactories.TargetProcessorFactory.CreateTargetProcessor(null).ReturnsForAnyArgs(targetProcessor);

            // Finders
            targetFactoriesProvider.FinderFactory.CreateRangedTargetFinder(null, null).ReturnsForAnyArgs(targetFinder);

            // Trackers
            targetFactories.TargetTrackerFactory.CreateRankedTargetTracker(null, null).ReturnsForAnyArgs(targetTracker);

            // Detector
            if (updaterProvider != null)
            {
                ITargetDetectorFactory targetDetectorFactory = new TargetDetectorFactory(enemyCruiser.UnitTargets, parentCruiser.UnitTargets, updaterProvider);
                targetFactories.TargetDetectorFactory.Returns(targetDetectorFactory);
            }
            
            // Filters
            targetFactoriesProvider.FilterFactory.CreateExactMatchTargetFilter().Returns(exactMatchTargetFilter);
            targetFactoriesProvider.FilterFactory.CreateExactMatchTargetFilter(null).ReturnsForAnyArgs(exactMatchTargetFilter);
            targetFactoriesProvider.FilterFactory.CreateDummyTargetFilter(true).ReturnsForAnyArgs(new DummyTargetFilter(isMatchResult: true));

            if (targetFilter != null)
            {
                targetFactoriesProvider.FilterFactory.CreateTargetFilter(default, null).ReturnsForAnyArgs(targetFilter);
            }
            else
            {
                SetupCreateTargetFilter(targetFactoriesProvider.FilterFactory);
            }

            return targetFactories;
        }

        // Copy real filter factory behaviour
        private void SetupCreateTargetFilter(ITargetFilterFactory filterFactory)
        {
            filterFactory
                .CreateTargetFilter(default, null)
                .ReturnsForAnyArgs(arg => new FactionAndTargetTypeFilter((Faction)arg.Args()[0], (IList<TargetType>)arg.Args()[1]));
        }

        public IAircraftProvider CreateAircraftProvider(
			IList<Vector2> bomberPatrolPoints = null,
            IList<Vector2> gunshipPatrolPoints = null,
			IList<Vector2> fighterPatrolPoints = null,
			IList<Vector2> deathstarPatrolPoints = null,
            IList<Vector2> spySatellitePatrolPoints = null,
			Rectangle fighterSafeZone = null)
		{
			IAircraftProvider provider = Substitute.For<IAircraftProvider>();

			if (bomberPatrolPoints == null)
			{
				bomberPatrolPoints = new List<Vector2>() 
				{
					new Vector2(0, 1),
					new Vector2(0, 4)
				};
			}
			provider.FindBomberPatrolPoints(0).ReturnsForAnyArgs(bomberPatrolPoints);

            if (gunshipPatrolPoints == null)
			{
				gunshipPatrolPoints = new List<Vector2>()
				{
					new Vector2(0, 1),
					new Vector2(0, 4)
				};
			}
            provider.FindGunshipPatrolPoints(0).ReturnsForAnyArgs(gunshipPatrolPoints);

			if (fighterPatrolPoints == null)
			{
				fighterPatrolPoints = new List<Vector2>() 
				{
                    new Vector2(0, 0),  
					new Vector2(3, 10)
				};
			}
			provider.FindFighterPatrolPoints(0).ReturnsForAnyArgs(fighterPatrolPoints);

			if (deathstarPatrolPoints == null)
			{
				deathstarPatrolPoints = new List<Vector2>() 
				{
					new Vector2(0, 10),
					new Vector2(0, 20),
					new Vector2(0, 30),
					new Vector2(0, 40)
				};
			}
			provider.FindDeathstarPatrolPoints(default, 0).ReturnsForAnyArgs(deathstarPatrolPoints);

            if (spySatellitePatrolPoints == null)
            {
                spySatellitePatrolPoints = new List<Vector2>()
                {
					new Vector2(0, 10),
					new Vector2(0, 20),
					new Vector2(0, 30)
                };
            }
            provider.FindSpySatellitePatrolPoints(default, 0).ReturnsForAnyArgs(spySatellitePatrolPoints);

			if (fighterSafeZone == null)
			{
				fighterSafeZone = new Rectangle(
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

            ObservableCollection<IBoostProvider> boostProviders = new ObservableCollection<IBoostProvider>();
			parentSlot.BoostProviders.Returns(boostProviders);

			ReadOnlyCollection<ISlot> neighbouringSlots = new ReadOnlyCollection<ISlot>(new List<ISlot>());
			parentSlot.NeighbouringSlots.Returns(neighbouringSlots);

			return parentSlot;
		}

        public IBarrelControllerArgs CreateBarrelControllerArgs(
            BarrelController barrel,
            IUpdater updater,
            ITargetFilter targetFilter = null,
            ITargetPositionPredictor targetPositionPredictor = null,
            IAngleCalculator angleCalculator = null,
            IAccuracyAdjuster accuracyAdjuster = null,
            IRotationMovementController rotationMovementController = null,
            ITargetPositionValidator targetPositionValidator = null,
            IAngleLimiter angleLimiter = null,
            IFactoryProvider factoryProvider = null,
            ICruiserSpecificFactories cruiserSpecificFactories = null,
            ITarget parent = null,
            ISoundKey firingSound = null,
            ObservableCollection<IBoostProvider> localBoostProviders = null,
            IAttackablePositionFinder attackablePositionFinder = null)
        {
            BuildableInitialisationArgs initialisationArgs = new BuildableInitialisationArgs(this, deferrer: Deferrer);

            return
                new BarrelControllerArgs(
                    updater,
                    targetFilter ?? Substitute.For<ITargetFilter>(),
                    targetPositionPredictor ?? new DummyTargetPositionpredictor(),
                    angleCalculator ?? new AngleCalculator(new AngleHelper()),
                    attackablePositionFinder ?? new DummyPositionFinder(),
                    accuracyAdjuster ?? new DummyAccuracyAdjuster(),
                    rotationMovementController ?? CreateRotationMovementController(barrel, updater),
                    targetPositionValidator ?? new DummyPositionValidator(),
                    angleLimiter ?? new DummyAngleLimiter(),
                    factoryProvider ?? initialisationArgs.FactoryProvider,
                    cruiserSpecificFactories ?? initialisationArgs.CruiserSpecificFactories,
                    parent ?? Substitute.For<ITarget>(),
                    localBoostProviders ?? new ObservableCollection<IBoostProvider>(),
                    new ObservableCollection<IBoostProvider>(),
                    firingSound ?? SoundKeys.Firing.BigCannon);
        }

        private IRotationMovementController CreateRotationMovementController(BarrelController barrel, IUpdater updater)
        {
            return
                new RotationMovementController(
                    new RotationHelper(),
                    new TransformBC(barrel.transform),
                    updater,
                    barrel.TurretStats.TurretRotateSpeedInDegrees);
        }

        public IAccuracyAdjusterFactory CreateDummyAccuracyAdjuster()
        {
			IAccuracyAdjusterFactory factory = Substitute.For<IAccuracyAdjusterFactory>();

            IAccuracyAdjuster dummyAccuracyAdjuster = new DummyAccuracyAdjuster();

            factory.CreateVerticalImpactProjectileAdjuster(null, null).ReturnsForAnyArgs(dummyAccuracyAdjuster);
            factory.CreateHorizontalImpactProjectileAdjuster(null, null).ReturnsForAnyArgs(dummyAccuracyAdjuster);
            factory.CreateDummyAdjuster().ReturnsForAnyArgs(dummyAccuracyAdjuster);

            return factory;
        }

        public static IGlobalBoostProviders CreateGlobalBoostProviders()
        {
            IGlobalBoostProviders globalBoostProviders = Substitute.For<IGlobalBoostProviders>();

            ObservableCollection<IBoostProvider> aircraftBoostProviders = Substitute.For<ObservableCollection<IBoostProvider>>();
            globalBoostProviders.AircraftBoostProviders.Returns(aircraftBoostProviders);

            ObservableCollection<IBoostProvider> turretAccuracyBoostProviders = Substitute.For<ObservableCollection<IBoostProvider>>();
            globalBoostProviders.TurretAccuracyBoostProviders.Returns(turretAccuracyBoostProviders);

            ObservableCollection<IBoostProvider> offenseFireRateBoostProviders = Substitute.For<ObservableCollection<IBoostProvider>>();
            globalBoostProviders.OffenseFireRateBoostProviders.Returns(offenseFireRateBoostProviders);

            ObservableCollection<IBoostProvider> defenseFireRateBoostProviders = Substitute.For<ObservableCollection<IBoostProvider>>();
            globalBoostProviders.DefenseFireRateBoostProviders.Returns(defenseFireRateBoostProviders);

            return globalBoostProviders;
        }

        // So UnitTargets knows about ships, and ManualProximityTargetProcessor works.
        public static void SetupFactoryForUnitMonitor(IFactory factory, ICruiser parentCruiser)
        {
            factory.UnitStarted += (sender, e) => SetupUnitForUnitMonitor(e.StartedUnit, parentCruiser);
        }

        // So UnitTargets knows about ships, and ManualProximityTargetProcessor works.
        public static void SetupUnitForUnitMonitor(IUnit unit, ICruiser parentCruiser)
        {
            parentCruiser.UnitMonitor.UnitStarted += Raise.EventWith(new UnitStartedEventArgs(unit));

            void destroyedHandler(object sender, DestroyedEventArgs e)
            {
                unit.Destroyed -= destroyedHandler;
                parentCruiser.UnitMonitor.UnitDestroyed += Raise.EventWith(new UnitDestroyedEventArgs(unit));
            }

            unit.Destroyed += destroyedHandler;
        }

        public void SetupCruiser(Cruiser cruiser)
        {
            ICruiserSpecificFactories cruiserSpecificFactories = Substitute.For<ICruiserSpecificFactories>();
            GlobalBoostProviders globalBoostProviders = new GlobalBoostProviders();
            cruiserSpecificFactories.GlobalBoostProviders.Returns(globalBoostProviders);
            TurretStatsFactory turretStatsFactory 
                = new TurretStatsFactory(
                    new BoostFactory(), 
                    globalBoostProviders);
            cruiserSpecificFactories.TurretStatsFactory.Returns(turretStatsFactory);

            BuildableInitialisationArgs initialisationArgs = new BuildableInitialisationArgs(this, deferrer: Deferrer);

            ICruiserArgs cruiserArgs
                = new CruiserArgs(
                    Faction.Reds,
                    enemyCruiser: Substitute.For<ICruiser>(),
                    uiManager: Substitute.For<IUIManager>(),
                    droneManager: new DroneManager(),
                    droneFocuser: Substitute.For<IDroneFocuser>(),
                    droneConsumerProvider: CreateDroneConsumerProvider(),
                    factoryProvider: initialisationArgs.FactoryProvider,
                    cruiserSpecificFactories: cruiserSpecificFactories,
                    facingDirection: Direction.Right,
                    repairManager: Substitute.For<IRepairManager>(),
                    fogStrength: BattleCruisers.Cruisers.Fog.FogStrength.Weak,
                    helper: Substitute.For<ICruiserHelper>(),
                    highlightableFilter: Substitute.For<ISlotFilter>(),
                    buildProgressCalculator: new LinearCalculator(_buildSpeedMultiplier),
                    buildingDoubleClickHandler: Substitute.For<IDoubleClickHandler<IBuilding>>(),
                    cruiserDoubleClickHandler: Substitute.For<IDoubleClickHandler<ICruiser>>(),
                    fogOfWarManager: Substitute.For<IManagedDisposable>(),
                    parentCruiserHasActiveDrones: Substitute.For<IBroadcastingProperty<bool>>());

            cruiser.StaticInitialise();
            cruiser.Initialise(cruiserArgs);
        }

        private IDroneConsumerProvider CreateDroneConsumerProvider()
        {
            IDroneConsumer droneConsumer = Substitute.For<IDroneConsumer>();
            droneConsumer.NumOfDrones = _numOfDrones;
            droneConsumer.State.Returns(DroneConsumerState.Active);

            IDroneConsumerProvider droneConsumerProvider = Substitute.For<IDroneConsumerProvider>();
            droneConsumerProvider.RequestDroneConsumer(default).ReturnsForAnyArgs(callInfo =>
            {
                droneConsumer.NumOfDronesRequired.Returns(callInfo.Arg<int>());
                return droneConsumer;
            });
            return droneConsumerProvider;
        }

        public BuildableInitialisationArgs CreateBuildableInitialisationArgs()
        {
            return
                new BuildableInitialisationArgs(
                    this,
                    deferrer: Deferrer,
                    realTimeDeferrer: RealTimeDeferrer);
        }
    }
}
