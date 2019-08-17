using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Targets.Factories;
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
            ISpriteProvider spriteProvider,
            IDeferrer deferrer,
            ICamera soleCamera,
            IAudioSource audioSource,
            IUpdaterProvider updaterProvider)
		{
            Helper.AssertIsNotNull(prefabFactory, spriteProvider, deferrer, soleCamera, audioSource, updaterProvider);

			PrefabFactory = prefabFactory;
            TargetFactories = new TargetFactoriesProvider();
			TargetPositionPredictorFactory = new TargetPositionPredictorFactory();
			MovementControllerFactory = new MovementControllerFactory();
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

            Sound = new SoundFactoryProvider(deferrer, soleCamera, audioSource);
            Turrets = new TurretFactoryProvider();
        }
	}
}
