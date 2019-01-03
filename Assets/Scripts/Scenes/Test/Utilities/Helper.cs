using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings;
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
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Projectiles.Trackers;
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
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class Helper
	{
        private readonly int _numOfDrones;
        private readonly float _buildSpeedMultiplier;
        private readonly IVariableDelayDeferrer _variableDelayDeferrer;

        private const int DEFAULT_NUM_OF_DRONES = 10;

        public Helper(
            int numOfDrones = DEFAULT_NUM_OF_DRONES, 
            float buildSpeedMultiplier = BuildSpeedMultipliers.VERY_FAST,
            IVariableDelayDeferrer variableDelayDeferrer = null)
		{
            _numOfDrones = numOfDrones;
            _buildSpeedMultiplier = buildSpeedMultiplier;
            _variableDelayDeferrer = variableDelayDeferrer;
		}

        public void InitialiseBuilding(
            IBuilding building,
            Faction faction = Faction.Blues,
            IUIManager uiManager = null,
            ICruiser parentCruiser = null,
            ICruiser enemyCruiser = null,
            IAircraftProvider aircraftProvider = null,
            IPrefabFactory prefabFactory = null,
            ITargetFactoriesProvider targetFactories = null,
            // FELIX  Remove :P
            ITargetsFactory targetsFactory = null,
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
            ITrackerFactory trackerFactory = null)
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
                    targetsFactory,
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
                    variableDelayDeferrer: _variableDelayDeferrer,
                    trackerFactory: trackerFactory);

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
            ITargetFactoriesProvider targetFactories = null,
			ITargetsFactory targetsFactory = null,
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
            ITrackerFactory trackerFactory = null)
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
					targetsFactory,
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
                    trackerFactory: trackerFactory);

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
			droneConsumerProvider.RequestDroneConsumer(default(int)).ReturnsForAnyArgs(callInfo =>
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

        public ITargetFactoriesProvider CreateTargetFactories(
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
            ITargetFactoriesProvider targetFactories = Substitute.For<ITargetFactoriesProvider>();
            targetFinder.EmitCruiserAsGlobalTarget();

            if (exactMatchTargetFilter == null)
            {
                exactMatchTargetFilter = new ExactMatchTargetFilter();
            }

            targetFactories.ProcessorFactory.BomberTargetProcessor.Returns(targetProcessor);
            targetFactories.ProcessorFactory.OffensiveBuildableTargetProcessor.Returns(targetProcessor);
            targetFactories.FinderFactory.CreateRangedTargetFinder(null, null).ReturnsForAnyArgs(targetFinder);
            targetFactories.TrackerFactory.CreateRankedTargetTracker(null, null).ReturnsForAnyArgs(targetTracker);
            targetFactories.ProcessorFactory.CreateTargetProcessor(null).ReturnsForAnyArgs(targetProcessor);
            targetFactories.FilterFactory.CreateExactMatchTargetFilter().Returns(exactMatchTargetFilter);
            targetFactories.FilterFactory.CreateExactMatchTargetFilter(null).ReturnsForAnyArgs(exactMatchTargetFilter);
            targetFactories.FilterFactory.CreateDummyTargetFilter(true).ReturnsForAnyArgs(new DummyTargetFilter(isMatchResult: true));

            if (targetFilter != null)
            {
                targetFactories.FilterFactory.CreateTargetFilter(default(Faction), null).ReturnsForAnyArgs(targetFilter);
            }
            else
            {
                SetupCreateTargetFilter(targetFactories.FilterFactory);
            }

            return targetFactories;
        }

        /// <summary>
        /// Target processors assign all the provided targets.  The targets are lost
        /// as they are destroyed.
        /// </summary>
        public ITargetFactoriesProvider CreateTargetFactories(IList<ITarget> targets)
        {
            ITargetFinder targetFinder = Substitute.For<ITargetFinder>();

            ITargetFactoriesProvider targetFactories = CreateTargetFactories(targetFinder);

            // Emit target found events AFTER targets factory (target processor) is created
            foreach (ITarget target in targets)
            {
                target.Destroyed += (sender, e) => targetFinder.TargetLost += Raise.EventWith(targetFinder, new TargetEventArgs(target));
                targetFinder.TargetFound += Raise.EventWith(targetFinder, new TargetEventArgs(target));
            }

            return targetFactories;
        }

        /// <summary>
        /// Use IObservableCollection so that targets do not need to be known right now.
        /// Targets can be added later, once they are know, and the target finder
        /// will emit appropriate target found events.
        /// </summary>
        public ITargetsFactory CreateTargetsFactory(IObservableCollection<ITarget> targets)
        {
            ITargetFinder targetFinder = Substitute.For<ITargetFinder>();

            ITargetsFactory targetsFactory = CreateTargetsFactory(targetFinder);

            // Emit target found events AFTER targets factory (target processor) is created
            targets.Changed += (sender, e) =>
            {
                if (e.Type == ChangeType.Add)
                {
                    e.Item.Destroyed += (target, args) => targetFinder.TargetLost += Raise.EventWith(targetFinder, new TargetEventArgs(e.Item));
                    targetFinder.TargetFound += Raise.EventWith(targetFinder, new TargetEventArgs(e.Item));
                }                    
            };

            return targetsFactory;
        }

        private ITargetsFactory CreateTargetsFactory(ITargetFinder targetFinder)
        {
            ITargetRanker targetRanker = new EqualTargetRanker();
            IRankedTargetTracker targetTracker = new RankedTargetTracker(targetFinder, targetRanker);
            ITargetProcessor targetProcessor = new TargetProcessor(targetTracker);
            ITargetFilter targetFilter = new DummyTargetFilter(isMatchResult: true);
            IExactMatchTargetFilter exactMatchTargetFilter = new ExactMatchTargetFilter();

            ITargetsFactory targetsFactory = Substitute.For<ITargetsFactory>();

            targetsFactory.BomberTargetProcessor.Returns(targetProcessor);
            targetsFactory.CreateDummyTargetFilter(default(bool)).ReturnsForAnyArgs(targetFilter);
            targetsFactory.OffensiveBuildableTargetProcessor.Returns(targetProcessor);
            targetsFactory.CreateExactMatchTargetFilter().Returns(exactMatchTargetFilter);
            targetsFactory.CreateExactMatchTargetFilter(null).ReturnsForAnyArgs(exactMatchTargetFilter);

            SetupCreateTargetFilter(targetsFactory);
            SetupCreateRangedTargetFinder(targetsFactory);
            SetupCreateRankedTargetTracker(targetsFactory);
            SetupCreateTargetProcessor(targetsFactory);

            return targetsFactory;
        }

        private ITargetFactoriesProvider CreateTargetFactories(ITargetFinder targetFinder)
        {
            ITargetRanker targetRanker = new EqualTargetRanker();
            IRankedTargetTracker targetTracker = new RankedTargetTracker(targetFinder, targetRanker);
            ITargetProcessor targetProcessor = new TargetProcessor(targetTracker);
            ITargetFilter targetFilter = new DummyTargetFilter(isMatchResult: true);
            IExactMatchTargetFilter exactMatchTargetFilter = new ExactMatchTargetFilter();

            ITargetFactoriesProvider targetFactories = Substitute.For<ITargetFactoriesProvider>();

            targetFactories.ProcessorFactory.BomberTargetProcessor.Returns(targetProcessor);
            targetFactories.FilterFactory.CreateDummyTargetFilter(default(bool)).ReturnsForAnyArgs(targetFilter);
            targetFactories.ProcessorFactory.OffensiveBuildableTargetProcessor.Returns(targetProcessor);
            targetFactories.FilterFactory.CreateExactMatchTargetFilter().Returns(exactMatchTargetFilter);
            targetFactories.FilterFactory.CreateExactMatchTargetFilter(null).ReturnsForAnyArgs(exactMatchTargetFilter);

            SetupCreateTargetFilter(targetFactories.FilterFactory);
            SetupCreateRangedTargetFinder(targetFactories.FinderFactory);
            SetupCreateRankedTargetTracker(targetFactories.TrackerFactory);
            SetupCreateTargetProcessor(targetFactories.ProcessorFactory);

            return targetFactories;
        }

        // FELIX  Remove :P
        // Copy real TargetsFactory behaviour
        private void SetupCreateTargetFilter(ITargetsFactory targetsFactory)
        {
            targetsFactory
                .CreateTargetFilter(default(Faction), null)
                .ReturnsForAnyArgs(arg => new FactionAndTargetTypeFilter((Faction)arg.Args()[0], (IList<TargetType>)arg.Args()[1]));
        }
        
        // Copy real filter factory behaviour
        private void SetupCreateTargetFilter(ITargetFilterFactory filterFactory)
        {
            filterFactory
                .CreateTargetFilter(default(Faction), null)
                .ReturnsForAnyArgs(arg => new FactionAndTargetTypeFilter((Faction)arg.Args()[0], (IList<TargetType>)arg.Args()[1]));
        }

        // FELIX  Remove :)
        // Copy real TargetsFactory behaviour
        private void SetupCreateRangedTargetFinder(ITargetsFactory targetsFactory)
        {
            targetsFactory
                .CreateRangedTargetFinder(null, null)
                .ReturnsForAnyArgs(arg => new RangedTargetFinder((ITargetDetector)arg.Args()[0], (ITargetFilter)arg.Args()[1]));
        }

        // Copy real target finder behaviour
        private void SetupCreateRangedTargetFinder(ITargetFinderFactory finderFactory)
        {
            finderFactory
                .CreateRangedTargetFinder(null, null)
                .ReturnsForAnyArgs(arg => new RangedTargetFinder((ITargetDetector)arg.Args()[0], (ITargetFilter)arg.Args()[1]));
        }

        // FELIX  Remove :)
        // Copy real TargetsFactory behaviour
        private void SetupCreateRankedTargetTracker(ITargetsFactory targetsFactory)
        {
            targetsFactory
                .CreateRankedTargetTracker(null, null)
                .ReturnsForAnyArgs(arg => new RankedTargetTracker((ITargetFinder)arg.Args()[0], (ITargetRanker)arg.Args()[1]));
        }

        // Copy real tracker factory behaviour
        private void SetupCreateRankedTargetTracker(ITargetTrackerFactory trackerFactory)
        {
            trackerFactory
                .CreateRankedTargetTracker(null, null)
                .ReturnsForAnyArgs(arg => new RankedTargetTracker((ITargetFinder)arg.Args()[0], (ITargetRanker)arg.Args()[1]));
        }

        // FELIX  Remove :)
        // Copy real TargetsFactory behaviour
        private void SetupCreateTargetProcessor(ITargetsFactory targetsFactory)
        {
            targetsFactory
                .CreateTargetProcessor(null)
                .ReturnsForAnyArgs(arg => new TargetProcessor((IRankedTargetTracker)arg.Args()[0]));
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
			provider.FindDeathstarPatrolPoints(default(Vector2), 0).ReturnsForAnyArgs(deathstarPatrolPoints);

            if (spySatellitePatrolPoints == null)
            {
                spySatellitePatrolPoints = new List<Vector2>()
                {
					new Vector2(0, 10),
					new Vector2(0, 20),
					new Vector2(0, 30)
                };
            }
            provider.FindSpySatellitePatrolPoints(default(Vector2), 0).ReturnsForAnyArgs(spySatellitePatrolPoints);

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
            ITargetPositionValidator targetPositionValidator = null,
            IAngleLimiter angleLimiter = null,
            IFactoryProvider factoryProvider = null,
            ITarget parent = null,
            ISoundKey firingSound = null,
            IObservableCollection<IBoostProvider> localBoostProviders = null,
            IAttackablePositionFinder attackablePositionFinder = null)
        {
            return
                new BarrelControllerArgs(
                    targetFilter ?? Substitute.For<ITargetFilter>(),
                    targetPositionPredictor ?? new DummyTargetPositionpredictor(),
                    angleCalculator ?? new AngleCalculator(new AngleHelper()),
                    attackablePositionFinder ?? new DummyPositionFinder(),
                    accuracyAdjuster ?? new DummyAccuracyAdjuster(),
                    rotationMovementController ?? CreateRotationMovementController(barrel),
                    targetPositionValidator ?? new DummyPositionValidator(),
                    angleLimiter ?? new DummyAngleLimiter(),
                    factoryProvider ?? new BuildableInitialisationArgs(this).FactoryProvider,
                    parent ?? Substitute.For<ITarget>(),
                    firingSound ?? SoundKeys.Firing.BigCannon,
                    localBoostProviders);
        }

        private IRotationMovementController CreateRotationMovementController(BarrelController barrel)
        {
            return
                new RotationMovementController(
                    new RotationHelper(),
                    new TransformBC(barrel.transform),
                    new TimeBC(),
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

            IObservableCollection<IBoostProvider> aircraftBoostProviders = Substitute.For<IObservableCollection<IBoostProvider>>();
            globalBoostProviders.AircraftBoostProviders.Returns(aircraftBoostProviders);

            IObservableCollection<IBoostProvider> turretAccuracyBoostProviders = Substitute.For<IObservableCollection<IBoostProvider>>();
            globalBoostProviders.TurretAccuracyBoostProviders.Returns(turretAccuracyBoostProviders);

            IObservableCollection<IBoostProvider> turretFireRateBoostProviders = Substitute.For<IObservableCollection<IBoostProvider>>();
            globalBoostProviders.TurretFireRateBoostProviders.Returns(turretFireRateBoostProviders);

            return globalBoostProviders;
        }
	}
}
