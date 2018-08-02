using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Cruisers;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Explosions;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Targets;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Utils
{
    public class FactoryProvider : IFactoryProvider
	{
		public IPrefabFactory PrefabFactory { get; private set; }
		public ITargetsFactory TargetsFactory { get; private set; }
		public IMovementControllerFactory MovementControllerFactory { get; private set; }
		public IAngleCalculatorFactory AngleCalculatorFactory { get; private set; }
		public ITargetPositionPredictorFactory TargetPositionPredictorFactory { get; private set; }
		public IAircraftProvider AircraftProvider { get; private set; }
		public IFlightPointsProviderFactory FlightPointsProviderFactory { get; private set; } 
        public IGlobalBoostProviders GlobalBoostProviders { get; private set; }
        public IBoostFactory BoostFactory { get; private set; }
        public ITurretStatsFactory TurretStatsFactory { get; private set; }
        public IDamageApplierFactory DamageApplierFactory { get; private set; }
        public IExplosionFactory ExplosionFactory { get; private set; }
        public IAccuracyAdjusterFactory AccuracyAdjusterFactory { get; private set; }
        public ITargetPositionValidatorFactory TargetPositionValidatorFactory { get; private set; }
        public IAngleLimiterFactory AngleLimiterFactory { get; private set; }
        public ISpriteChooserFactory SpriteChooserFactory { get; private set; }
        public ISoundFetcher SoundFetcher { get; private set; }
        public ISoundManager SoundManager { get; private set; }
        public ISoundPlayerFactory SoundPlayerFactory { get; private set; }
        public IClickHandlerFactory ClickHandlerFactory { get; private set; }

        public FactoryProvider(
            IPrefabFactory prefabFactory, 
            ICruiser friendlyCruiser, 
            ICruiser enemyCruiser, 
            ISpriteProvider spriteProvider,
            IVariableDelayDeferrer deferrer)
		{
            Helper.AssertIsNotNull(prefabFactory, friendlyCruiser, enemyCruiser, spriteProvider, deferrer);

			PrefabFactory = prefabFactory;
			TargetsFactory = new TargetsFactory(enemyCruiser);
			AngleCalculatorFactory = new AngleCalculatorFactory();
			TargetPositionPredictorFactory = new TargetPositionPredictorFactory();
			MovementControllerFactory = new MovementControllerFactory();
            AircraftProvider = new AircraftProvider(friendlyCruiser.Position, enemyCruiser.Position, new RandomGenerator());
			FlightPointsProviderFactory = new FlightPointsProviderFactory();
            BoostFactory = new BoostFactory();
            GlobalBoostProviders = new GlobalBoostProviders();
            TurretStatsFactory = new TurretStatsFactory(BoostFactory, GlobalBoostProviders);
            DamageApplierFactory = new DamageApplierFactory(TargetsFactory);
            ExplosionFactory = new ExplosionFactory(PrefabFactory);
            AccuracyAdjusterFactory = new AccuracyAdjusterFactory();
            TargetPositionValidatorFactory = new TargetPositionValidatorFactory();
            AngleLimiterFactory = new AngleLimiterFactory();
            SoundFetcher = new SoundFetcher();
            SoundManager = new SoundManager(SoundFetcher, new SoundPlayer());
            SpriteChooserFactory
                = new SpriteChooserFactory(
                    new AssignerFactory(),
                    spriteProvider);
            SoundPlayerFactory = new SoundPlayerFactory(SoundFetcher, deferrer);
            ClickHandlerFactory = new ClickHandlerFactory();
        }
	}
}
