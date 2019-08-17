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
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.Manager;
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
using System.Collections.Specialized;
using UnityCommon.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class Helper
	{
        private readonly int _numOfDrones;
        private readonly float _buildSpeedMultiplier;
        private readonly IDeferrer _deferrer;
        private readonly IUpdaterProvider _updaterProvider;

        private const int DEFAULT_NUM_OF_DRONES = 10;

        public Helper(
            int numOfDrones = DEFAULT_NUM_OF_DRONES, 
            float buildSpeedMultiplier = BuildSpeedMultipliers.VERY_FAST,
            IDeferrer deferrer = null,
            IUpdaterProvider updaterProvider = null)
		{
            _numOfDrones = numOfDrones;
            _buildSpeedMultiplier = buildSpeedMultiplier;
            _deferrer = deferrer;
            _updaterProvider = updaterProvider;
		}

        public void InitialiseBuilding(
            IBuilding building,
            Faction faction = Faction.Blues,
            IUIManager uiManager = null,
            ICruiser parentCruiser = null,
            ICruiser enemyCruiser = null,
            IAircraftProvider aircraftProvider = null,
            IPrefabFactory prefabFactory = null,
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
            ITargetPositionValidatorFactory targetPositionValidatorFactory = null)
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
                    deferrer: _deferrer,
                    updaterProvider: _updaterProvider);

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
                initialisationArgs.CruiserSpecificFactories,
                parentSlot ?? CreateParentSlot(),
                Substitute.For<IDoubleClickHandler<IBuilding>>());
        }

        public void InitialiseUnit(
			IUnit unit,
			Faction faction = Faction.Blues,
			IUIManager uiManager = null,
			ICruiser parentCruiser = null,
			ICruiser enemyCruiser = null,
			IAircraftProvider aircraftProvider = null,
			IPrefabFactory prefabFactory = null,
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
					prefabFactory,
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
                    updaterProvider: _updaterProvider);

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
                initialisationArgs.FactoryProvider,
                initialisationArgs.CruiserSpecificFactories);
        }

		public ICruiser CreateCruiser(Direction facingDirection, Faction faction)
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
            ITargetFilter targetFilter = null, 
            IExactMatchTargetFilter exactMatchTargetFilter = null)
        {
            // The enemy cruiser is added as a target by the global target finder.
            // So pretend the cruiser game object is the specified target.
            ICruiser enemyCruiser = Substitute.For<ICruiser>();
            enemyCruiser.GameObject.Returns(globalTarget);
            enemyCruiser.Position.Returns(x => (Vector2)globalTarget.transform.position);

            GlobalTargetFinder targetFinder = new GlobalTargetFinder(enemyCruiser);
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

            // Procressors
            targetFactoriesProvider.ProcessorFactory.BomberTargetProcessor.Returns(targetProcessor);
            targetFactoriesProvider.ProcessorFactory.OffensiveBuildableTargetProcessor.Returns(targetProcessor);
            targetFactoriesProvider.ProcessorFactory.CreateTargetProcessor(null).ReturnsForAnyArgs(targetProcessor);
            targetFactories.TargetProcessorFactory.BomberTargetProcessor.Returns(targetProcessor);
            targetFactories.TargetProcessorFactory.OffensiveBuildableTargetProcessor.Returns(targetProcessor);
            targetFactories.TargetProcessorFactory.CreateTargetProcessor(null).ReturnsForAnyArgs(targetProcessor);

            // Finders
            targetFactoriesProvider.FinderFactory.CreateRangedTargetFinder(null, null).ReturnsForAnyArgs(targetFinder);

            // Trackers
            targetFactoriesProvider.TrackerFactory.CreateRankedTargetTracker(null, null).ReturnsForAnyArgs(targetTracker);
            targetFactories.TargetTrackerFactory.CreateRankedTargetTracker(null, null).ReturnsForAnyArgs(targetTracker);

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

        /// <summary>
        /// Target processors assign all the provided targets.  The targets are lost
        /// as they are destroyed.
        /// </summary>
        public ITargetFactories CreateTargetFactories(IList<ITarget> targets)
        {
            ITargetFinder targetFinder = Substitute.For<ITargetFinder>();

            ITargetFactories targetFactories = CreateTargetFactories(targetFinder);

            // Emit target found events AFTER targets factory (target processor) is created
            foreach (ITarget target in targets)
            {
                target.Destroyed += (sender, e) => targetFinder.TargetLost += Raise.EventWith(targetFinder, new TargetEventArgs(target));
                targetFinder.TargetFound += Raise.EventWith(targetFinder, new TargetEventArgs(target));
            }

            return targetFactories;
        }

        /// <summary>
        /// Use ObservableCollection so that targets do not need to be known right now.
        /// Targets can be added later, once they are known, and the target finder
        /// will emit appropriate target found events.
        /// </summary>
        public ITargetFactories CreateTargetFactories(ObservableCollection<ITarget> targets)
        {
            ITargetFinder targetFinder = Substitute.For<ITargetFinder>();

            ITargetFactories targetFactories = CreateTargetFactories(targetFinder);

            // Emit target found events AFTER targets factory (target processor) is created
            targets.CollectionChanged += (sender, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    Assert.AreEqual(1, e.NewItems.Count);
                    ITarget newTarget = e.NewItems[0] as ITarget;
                    Assert.IsNotNull(newTarget);
                    newTarget.Destroyed += (target, args) => targetFinder.TargetLost += Raise.EventWith(targetFinder, new TargetEventArgs(newTarget));
                    targetFinder.TargetFound += Raise.EventWith(targetFinder, new TargetEventArgs(newTarget));
                }                    
            };

            return targetFactories;
        }

        private ITargetFactories CreateTargetFactories(ITargetFinder targetFinder)
        {
            ITargetFactories targetFactories = Substitute.For<ITargetFactories>();

            ITargetRanker targetRanker = new EqualTargetRanker();
            IRankedTargetTracker targetTracker = new RankedTargetTracker(targetFinder, targetRanker);
            ITargetProcessor targetProcessor = new TargetProcessor(targetTracker);
            ITargetFilter targetFilter = new DummyTargetFilter(isMatchResult: true);
            IExactMatchTargetFilter exactMatchTargetFilter = new ExactMatchTargetFilter();

            // Processors
            targetFactories.TargetFactoriesProvider.ProcessorFactory.BomberTargetProcessor.Returns(targetProcessor);
            targetFactories.TargetFactoriesProvider.ProcessorFactory.OffensiveBuildableTargetProcessor.Returns(targetProcessor);
            targetFactories.TargetProcessorFactory.BomberTargetProcessor.Returns(targetProcessor);
            targetFactories.TargetProcessorFactory.OffensiveBuildableTargetProcessor.Returns(targetProcessor);

            targetFactories.TargetFactoriesProvider.FilterFactory.CreateDummyTargetFilter(default).ReturnsForAnyArgs(targetFilter);
            targetFactories.TargetFactoriesProvider.FilterFactory.CreateExactMatchTargetFilter().Returns(exactMatchTargetFilter);
            targetFactories.TargetFactoriesProvider.FilterFactory.CreateExactMatchTargetFilter(null).ReturnsForAnyArgs(exactMatchTargetFilter);

            SetupCreateTargetFilter(targetFactories.TargetFactoriesProvider.FilterFactory);
            SetupCreateRangedTargetFinder(targetFactories.TargetFactoriesProvider.FinderFactory);

            SetupCreateRankedTargetTracker(targetFactories.TargetFactoriesProvider.TrackerFactory);
            targetFactories.TargetTrackerFactory.Returns(targetFactories.TargetFactoriesProvider.TrackerFactory);

            SetupCreateTargetProcessor(targetFactories.TargetFactoriesProvider.ProcessorFactory);
            targetFactories.TargetProcessorFactory.Returns(targetFactories.TargetFactoriesProvider.ProcessorFactory);

            return targetFactories;
        }

        // Copy real filter factory behaviour
        private void SetupCreateTargetFilter(ITargetFilterFactory filterFactory)
        {
            filterFactory
                .CreateTargetFilter(default, null)
                .ReturnsForAnyArgs(arg => new FactionAndTargetTypeFilter((Faction)arg.Args()[0], (IList<TargetType>)arg.Args()[1]));
        }

        // Copy real target finder behaviour
        private void SetupCreateRangedTargetFinder(ITargetFinderFactory finderFactory)
        {
            finderFactory
                .CreateRangedTargetFinder(null, null)
                .ReturnsForAnyArgs(arg => new RangedTargetFinder((ITargetDetector)arg.Args()[0], (ITargetFilter)arg.Args()[1]));
        }

        // Copy real tracker factory behaviour
        private void SetupCreateRankedTargetTracker(ITargetTrackerFactory trackerFactory)
        {
            trackerFactory
                .CreateRankedTargetTracker(null, null)
                .ReturnsForAnyArgs(arg => new RankedTargetTracker((ITargetFinder)arg.Args()[0], (ITargetRanker)arg.Args()[1]));
        }

        // Copy real processor factory behaviour
        private void SetupCreateTargetProcessor(ITargetProcessorFactory processorFactory)
        {
            processorFactory
                .CreateTargetProcessor(null)
                .ReturnsForAnyArgs(arg => new TargetProcessor((IRankedTargetTracker)arg.Args()[0]));
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
            BuildableInitialisationArgs initialisationArgs = new BuildableInitialisationArgs(this);

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
            unit.CompletedBuildable += (sender, e) => parentCruiser.UnitMonitor.UnitCompleted += Raise.EventWith(new UnitCompletedEventArgs(unit));
            unit.Destroyed += (sender, e) => parentCruiser.UnitMonitor.UnitDestroyed += Raise.EventWith(new UnitDestroyedEventArgs(unit));
        }
	}
}
