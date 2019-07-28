using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Cruisers;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Projectiles.Trackers;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using UnityCommon.PlatformAbstractions;
using BcUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class BuildableInitialisationArgs
    {
        public IUIManager UiManager { get; }
        public ICruiser ParentCruiser { get; }
        public ICruiser EnemyCruiser { get; }
        public IFactoryProvider FactoryProvider { get; }
        public Direction ParentCruiserFacingDirection { get; }

        public BuildableInitialisationArgs(
            Helper helper,
            Faction faction = Faction.Blues,
            IUIManager uiManager = null,
            ICruiser parentCruiser = null,
            ICruiser enemyCruiser = null,
            IAircraftProvider aircraftProvider = null,
            IPrefabFactory prefabFactory = null,
            ITargetFactoriesProvider targetFactories = null,
            IMovementControllerFactory movementControllerFactory = null,
            IAngleCalculatorFactory angleCalculatorFactory = null,
            ITargetPositionPredictorFactory targetPositionPredictorFactory = null,
            IFlightPointsProviderFactory flightPointsProviderFactory = null,
            IBoostFactory boostFactory = null,
            IGlobalBoostProviders globalBoostProviders = null,
            IDamageApplierFactory damageApplierFactory = null,
            Direction parentCruiserDirection = Direction.Right,
            IAccuracyAdjusterFactory accuracyAdjusterFactory = null,
            ITargetPositionValidatorFactory targetPositionValidatorFactory = null,
            IAngleLimiterFactory angleLimiterFactory = null,
			ISoundFetcher soundFetcher = null,
            ISoundPlayer soundPlayer = null,
            ISpriteChooserFactory spriteChooserFactory = null,
            IDeferrer deferrer = null,
            IUserChosenTargetManager userChosenTargetManager = null,
            IExplosionManager explosionManager = null,
            ITrackerFactory trackerFactory = null,
            IUpdaterProvider updaterProvider = null)
        {
            ParentCruiserFacingDirection = parentCruiserDirection;
            ParentCruiser = parentCruiser ?? helper.CreateCruiser(ParentCruiserFacingDirection, faction);
            EnemyCruiser = enemyCruiser ?? helper.CreateCruiser(Direction.Left, BcUtils.Helper.GetOppositeFaction(faction));
            UiManager = uiManager ?? Substitute.For<IUIManager>();
            userChosenTargetManager = userChosenTargetManager ?? new UserChosenTargetManager();
            updaterProvider = updaterProvider ?? Substitute.For<IUpdaterProvider>();
            targetFactories = targetFactories ?? new TargetFactoriesProvider(ParentCruiser, EnemyCruiser, userChosenTargetManager, updaterProvider);
            prefabFactory = prefabFactory ?? new PrefabFactory(new PrefabFetcher());
            soundFetcher = soundFetcher ?? new SoundFetcher();
            deferrer = deferrer ?? Substitute.For<IDeferrer>();
            globalBoostProviders = globalBoostProviders ?? new GlobalBoostProviders();
            boostFactory = boostFactory ?? new BoostFactory();
            explosionManager = explosionManager ?? new ExplosionManager(prefabFactory);

            FactoryProvider
                = CreateFactoryProvider(
                    prefabFactory,
                    movementControllerFactory ?? new MovementControllerFactory(new TimeBC()),
                    angleCalculatorFactory ?? new AngleCalculatorFactory(),
                    targetPositionPredictorFactory ?? new TargetPositionPredictorFactory(),
                    aircraftProvider ?? helper.CreateAircraftProvider(),
                    flightPointsProviderFactory ?? new FlightPointsProviderFactory(),
                    boostFactory,
                    globalBoostProviders,
                    damageApplierFactory ?? new DamageApplierFactory(targetFactories.FilterFactory),
                    accuracyAdjusterFactory ?? helper.CreateDummyAccuracyAdjuster(),
                    targetPositionValidatorFactory ?? new TargetPositionValidatorFactory(),
                    angleLimiterFactory ?? new AngleLimiterFactory(),
                    soundFetcher,
                    soundPlayer ?? new SoundPlayer(soundFetcher, new AudioClipPlayer(), Substitute.For<ICamera>()),
                    spriteChooserFactory ??
                        new SpriteChooserFactory(
                            new AssignerFactory(),
                            new SpriteProvider(new SpriteFetcher())),
                    new SoundPlayerFactory(soundFetcher, deferrer),
                    new TurretStatsFactory(boostFactory, globalBoostProviders),
                    new AttackablePositionFinderFactory(),
                    new DeferrerProvider(deferrer),
                    explosionManager,
                    trackerFactory ?? Substitute.For<ITrackerFactory>(),
                    targetFactories,
                    new SpawnDeciderFactory());
        }

        private IFactoryProvider CreateFactoryProvider(
            IPrefabFactory prefabFactory,
            IMovementControllerFactory movementControllerFactory,
            IAngleCalculatorFactory angleCalculatorFactory,
            ITargetPositionPredictorFactory targetPositionControllerFactory,
            IAircraftProvider aircraftProvider,
            IFlightPointsProviderFactory flightPointsProviderFactory,
            IBoostFactory boostFactory,
            IGlobalBoostProviders globalBoostProviders,
            IDamageApplierFactory damageApplierFactory,
            IAccuracyAdjusterFactory accuracyAdjusterFactory,
            ITargetPositionValidatorFactory targetPositionValidatorFactory,
            IAngleLimiterFactory angleLimiterFactory,
            ISoundFetcher soundFetcher,
            ISoundPlayer soundManager,
            ISpriteChooserFactory spriteChooserFactory,
            ISoundPlayerFactory soundPlayerFactory,
            ITurretStatsFactory turretStatsFactory,
            IAttackablePositionFinderFactory attackablePositionFinderFactory,
            IDeferrerProvider deferrerProvider,
            IExplosionManager explosionManager,
            ITrackerFactory trackerFactory,
            ITargetFactoriesProvider targetFactories,
            ISpawnDeciderFactory spawnDeciderFactory)
        {
            IFactoryProvider factoryProvider = Substitute.For<IFactoryProvider>();

            factoryProvider.AircraftProvider.Returns(aircraftProvider);
            factoryProvider.BoostFactory.Returns(boostFactory);
            factoryProvider.DamageApplierFactory.Returns(damageApplierFactory);
            factoryProvider.DeferrerProvider.Returns(deferrerProvider);
            factoryProvider.ExplosionManager.Returns(explosionManager);
            factoryProvider.FlightPointsProviderFactory.Returns(flightPointsProviderFactory);
            factoryProvider.GlobalBoostProviders.Returns(globalBoostProviders);
            factoryProvider.MovementControllerFactory.Returns(movementControllerFactory);
            factoryProvider.PrefabFactory.Returns(prefabFactory);
            factoryProvider.SpawnDeciderFactory.Returns(spawnDeciderFactory);
            factoryProvider.SpriteChooserFactory.Returns(spriteChooserFactory);
            factoryProvider.TargetFactories.Returns(targetFactories);
            factoryProvider.TargetPositionPredictorFactory.Returns(targetPositionControllerFactory);
            factoryProvider.TrackerFactory.Returns(trackerFactory);

            // Turrets
            ITurretFactoryProvider turretFactoryProvider = Substitute.For<ITurretFactoryProvider>();
            turretFactoryProvider.AccuracyAdjusterFactory.Returns(accuracyAdjusterFactory);
            turretFactoryProvider.AngleCalculatorFactory.Returns(angleCalculatorFactory);
            turretFactoryProvider.AngleLimiterFactory.Returns(angleLimiterFactory);
            turretFactoryProvider.AttackablePositionFinderFactory.Returns(attackablePositionFinderFactory);
            turretFactoryProvider.TargetPositionValidatorFactory.Returns(targetPositionValidatorFactory);
            turretFactoryProvider.TurretStatsFactory.Returns(turretStatsFactory);
            factoryProvider.Turrets.Returns(turretFactoryProvider);

            // Sound
            ISoundFactoryProvider soundFactoryProvider = Substitute.For<ISoundFactoryProvider>();
            soundFactoryProvider.SoundFetcher.Returns(soundFetcher);
            soundFactoryProvider.SoundPlayer.Returns(soundManager);
            soundFactoryProvider.SoundPlayerFactory.Returns(soundPlayerFactory);
            factoryProvider.Sound.Returns(soundFactoryProvider);

            return factoryProvider;
        }
    }
}
