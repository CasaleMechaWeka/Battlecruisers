using BattleCruisers.Buildables.Boost;
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
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Utils.Factories
{
    public class FactoryProvider : IFactoryProvider
	{
        public IPrefabFactory PrefabFactory { get; private set; }
		public ITargetsFactory TargetsFactory { get; private set; }
		public IMovementControllerFactory MovementControllerFactory { get; private set; }
		public ITargetPositionPredictorFactory TargetPositionPredictorFactory { get; private set; }
		public IAircraftProvider AircraftProvider { get; private set; }
		public IFlightPointsProviderFactory FlightPointsProviderFactory { get; private set; } 
        public IGlobalBoostProviders GlobalBoostProviders { get; private set; }
        public IBoostFactory BoostFactory { get; private set; }
        public IDamageApplierFactory DamageApplierFactory { get; private set; }
        public IExplosionFactory ExplosionFactory { get; private set; }
        public ISpriteChooserFactory SpriteChooserFactory { get; private set; }
        public ISoundFetcher SoundFetcher { get; private set; }
        public ISoundManager SoundManager { get; private set; }
        public ISoundPlayerFactory SoundPlayerFactory { get; private set; }
        public ITurretFactoryProvider Turrets { get; private set; }

        public FactoryProvider(
            IPrefabFactory prefabFactory, 
            ICruiser friendlyCruiser, 
            ICruiser enemyCruiser, 
            ISpriteProvider spriteProvider,
            IVariableDelayDeferrer deferrer,
            IRankedTargetTracker userChosenTargetTracker,
            IUserChosenTargetHelper userChosenTargetHelper)
		{
            Helper.AssertIsNotNull(prefabFactory, friendlyCruiser, enemyCruiser, spriteProvider, deferrer, userChosenTargetTracker, userChosenTargetHelper);

			PrefabFactory = prefabFactory;
			TargetsFactory = new TargetsFactory(enemyCruiser, userChosenTargetTracker, userChosenTargetHelper);
			TargetPositionPredictorFactory = new TargetPositionPredictorFactory();
			MovementControllerFactory = new MovementControllerFactory();
            AircraftProvider = new AircraftProvider(friendlyCruiser.Position, enemyCruiser.Position, new RandomGenerator());
			FlightPointsProviderFactory = new FlightPointsProviderFactory();
            BoostFactory = new BoostFactory();
            GlobalBoostProviders = new GlobalBoostProviders();
            DamageApplierFactory = new DamageApplierFactory(TargetsFactory);
            ExplosionFactory = new ExplosionFactory(PrefabFactory);
            SoundFetcher = new SoundFetcher();
            SoundManager = new SoundManager(SoundFetcher, new SoundPlayer());
            SpriteChooserFactory
                = new SpriteChooserFactory(
                    new AssignerFactory(),
                    spriteProvider);
            SoundPlayerFactory = new SoundPlayerFactory(SoundFetcher, deferrer);

            Turrets = new TurretFactoryProvider(BoostFactory, GlobalBoostProviders);
        }
	}
}
