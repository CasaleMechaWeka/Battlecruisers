using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
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
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Explosions;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using BcUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class BuildableInitialisationArgs
    {
        public IUIManager UiManager { get; private set; }
        public ICruiser ParentCruiser { get; private set; }
        public ICruiser EnemyCruiser { get; private set; }
        public IFactoryProvider FactoryProvider { get; private set; }
        public Direction ParentCruiserFacingDirection { get; private set; }

        public BuildableInitialisationArgs(
            Helper helper,
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
            IGlobalBoostProviders globalBoostProviders = null,
            IDamageApplierFactory damageApplierFactory = null,
            Direction parentCruiserDirection = Direction.Right,
            IExplosionFactory explosionFactory = null,
            IAccuracyAdjusterFactory accuracyAdjusterFactory = null,
            ITargetPositionValidatorFactory targetPositionValidatorFactory = null,
            IAngleLimiterFactory angleLimiterFactory = null,
			ISoundFetcher soundFetcher = null,
            ISoundManager soundManager = null,
            ISpriteChooserFactory spriteChooserFactory = null,
            IVariableDelayDeferrer variableDelayDeferrer = null,
            IUserChosenTargetManager userChosenTargetManager = null,
            IUserChosenTargetHelper userChosenTargetHelper = null)
        {
            ParentCruiserFacingDirection = parentCruiserDirection;
            ParentCruiser = parentCruiser ?? helper.CreateCruiser(ParentCruiserFacingDirection, faction);
            EnemyCruiser = enemyCruiser ?? helper.CreateCruiser(Direction.Left, BcUtils.Helper.GetOppositeFaction(faction));
            UiManager = uiManager ?? Substitute.For<IUIManager>();
            userChosenTargetManager = userChosenTargetManager ?? new UserChosenTargetManager();
            userChosenTargetHelper = userChosenTargetHelper ?? new UserChosenTargetHelper(userChosenTargetManager);
            targetsFactory = targetsFactory ?? new TargetsFactory(EnemyCruiser, userChosenTargetManager, userChosenTargetHelper);
            prefabFactory = prefabFactory ?? new PrefabFactory(new PrefabFetcher());
            soundFetcher = soundFetcher ?? new SoundFetcher();
            variableDelayDeferrer = variableDelayDeferrer ?? Substitute.For<IVariableDelayDeferrer>();
            globalBoostProviders = globalBoostProviders ?? new GlobalBoostProviders();
            boostFactory = boostFactory ?? new BoostFactory();

            FactoryProvider
                = CreateFactoryProvider(
                    prefabFactory,
                    targetsFactory,
                    movementControllerFactory ?? new MovementControllerFactory(),
                    angleCalculatorFactory ?? new AngleCalculatorFactory(),
                    targetPositionPredictorFactory ?? new TargetPositionPredictorFactory(),
                    aircraftProvider ?? helper.CreateAircraftProvider(),
                    flightPointsProviderFactory ?? new FlightPointsProviderFactory(),
                    boostFactory,
                    globalBoostProviders,
                    damageApplierFactory ?? new DamageApplierFactory(targetsFactory),
                    explosionFactory ?? new ExplosionFactory(prefabFactory),
                    accuracyAdjusterFactory ?? helper.CreateDummyAccuracyAdjuster(),
                    targetPositionValidatorFactory ?? new TargetPositionValidatorFactory(),
                    angleLimiterFactory ?? new AngleLimiterFactory(),
                    soundFetcher,
                    soundManager ?? new SoundManager(soundFetcher, new SoundPlayer()),
                    spriteChooserFactory ??
                        new SpriteChooserFactory(
                            new AssignerFactory(),
                            new SpriteProvider(new SpriteFetcher())),
                    new SoundPlayerFactory(soundFetcher, variableDelayDeferrer),
                    new TurretStatsFactory(boostFactory, globalBoostProviders),
                    new AttackablePositionFinderFactory());
        }

        private IFactoryProvider CreateFactoryProvider(
            IPrefabFactory prefabFactory,
            ITargetsFactory targetsFactory,
            IMovementControllerFactory movementControllerFactory,
            IAngleCalculatorFactory angleCalculatorFactory,
            ITargetPositionPredictorFactory targetPositionControllerFactory,
            IAircraftProvider aircraftProvider,
            IFlightPointsProviderFactory flightPointsProviderFactory,
            IBoostFactory boostFactory,
            IGlobalBoostProviders globalBoostProviders,
            IDamageApplierFactory damageApplierFactory,
            IExplosionFactory explosionFactory,
            IAccuracyAdjusterFactory accuracyAdjusterFactory,
            ITargetPositionValidatorFactory targetPositionValidatorFactory,
            IAngleLimiterFactory angleLimiterFactory,
            ISoundFetcher soundFetcher,
            ISoundManager soundManager,
            ISpriteChooserFactory spriteChooserFactory,
            ISoundPlayerFactory soundPlayerFactory,
            ITurretStatsFactory turretStatsFactory,
            IAttackablePositionFinderFactory attackablePositionFinderFactory)
        {
            IFactoryProvider factoryProvider = Substitute.For<IFactoryProvider>();

            factoryProvider.AircraftProvider.Returns(aircraftProvider);
            factoryProvider.BoostFactory.Returns(boostFactory);
            factoryProvider.DamageApplierFactory.Returns(damageApplierFactory);
            factoryProvider.ExplosionFactory.Returns(explosionFactory);
            factoryProvider.FlightPointsProviderFactory.Returns(flightPointsProviderFactory);
            factoryProvider.GlobalBoostProviders.Returns(globalBoostProviders);
            factoryProvider.MovementControllerFactory.Returns(movementControllerFactory);
            factoryProvider.PrefabFactory.Returns(prefabFactory);
            factoryProvider.SoundFetcher.Returns(soundFetcher);
            factoryProvider.SoundManager.Returns(soundManager);
            factoryProvider.SoundPlayerFactory.Returns(soundPlayerFactory);
            factoryProvider.SpriteChooserFactory.Returns(spriteChooserFactory);
            factoryProvider.TargetsFactory.Returns(targetsFactory);
            factoryProvider.TargetPositionPredictorFactory.Returns(targetPositionControllerFactory);

            // Turrets
            ITurretFactoryProvider turretFactoryProvider = Substitute.For<ITurretFactoryProvider>();
            turretFactoryProvider.AccuracyAdjusterFactory.Returns(accuracyAdjusterFactory);
            turretFactoryProvider.AngleCalculatorFactory.Returns(angleCalculatorFactory);
            turretFactoryProvider.AngleLimiterFactory.Returns(angleLimiterFactory);
            turretFactoryProvider.AttackablePositionFinderFactory.Returns(attackablePositionFinderFactory);
            turretFactoryProvider.TargetPositionValidatorFactory.Returns(targetPositionValidatorFactory);
            turretFactoryProvider.TurretStatsFactory.Returns(turretStatsFactory);
            factoryProvider.Turrets.Returns(turretFactoryProvider);

            return factoryProvider;
        }
    }
}
