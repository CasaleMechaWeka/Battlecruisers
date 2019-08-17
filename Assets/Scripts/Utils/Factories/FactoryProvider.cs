using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Cruisers;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Utils.Factories
{
    public class FactoryProvider : IFactoryProvider
    {
        public ISoundFactoryProvider Sound { get; }
        public ITurretFactoryProvider Turrets { get; }
        public IAircraftProvider AircraftProvider { get; }
        public IBoostFactory BoostFactory { get; }
        public IDamageApplierFactory DamageApplierFactory { get; }
        public IDeferrerProvider DeferrerProvider { get; }
        public IFlightPointsProviderFactory FlightPointsProviderFactory { get; }
        public IMovementControllerFactory MovementControllerFactory { get; }
        public IPrefabFactory PrefabFactory { get; }
        public ISpawnDeciderFactory SpawnDeciderFactory { get; }
        public ISpriteChooserFactory SpriteChooserFactory { get; }
        public ITargetFactoriesProvider TargetFactories { get; }
        public ITargetPositionPredictorFactory TargetPositionPredictorFactory { get; }
        public IUpdaterProvider UpdaterProvider { get; }

        private IPoolProviders _poolProviders;
        public IPoolProviders PoolProviders
        {
            get
            {
                if (_poolProviders == null)
                {
                    _poolProviders = new PoolProviders(this);
                }
                return _poolProviders;
            }
        }

        public FactoryProvider(
            IPrefabFactory prefabFactory, 
            ICruiser friendlyCruiser, 
            ICruiser enemyCruiser, 
            ISpriteProvider spriteProvider,
            IDeferrer deferrer,
            IRankedTargetTracker userChosenTargetTracker,
            ICamera soleCamera,
            bool isPlayerCruiser,
            IAudioSource audioSource,
            IUpdaterProvider updaterProvider)
		{
            Helper.AssertIsNotNull(prefabFactory, friendlyCruiser, enemyCruiser, spriteProvider, deferrer, userChosenTargetTracker, soleCamera, audioSource, updaterProvider);

			PrefabFactory = prefabFactory;
            TargetFactories = new TargetFactoriesProvider();
			TargetPositionPredictorFactory = new TargetPositionPredictorFactory();
			MovementControllerFactory = new MovementControllerFactory();
            AircraftProvider = new AircraftProvider(friendlyCruiser.Position, enemyCruiser.Position, new RandomGenerator());
			FlightPointsProviderFactory = new FlightPointsProviderFactory();
            BoostFactory = new BoostFactory();
            DamageApplierFactory = new DamageApplierFactory(TargetFactories.FilterFactory);
            SpriteChooserFactory
                = new SpriteChooserFactory(
                    new AssignerFactory(),
                    spriteProvider);
            DeferrerProvider = new DeferrerProvider(deferrer);
            SpawnDeciderFactory = new SpawnDeciderFactory();
            UpdaterProvider = updaterProvider;

            Sound = new SoundFactoryProvider(deferrer, soleCamera, isPlayerCruiser, audioSource);
            Turrets = new TurretFactoryProvider();
        }
	}
}
