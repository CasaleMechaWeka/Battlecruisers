using BattleCruisers.Buildables.Pools;
using BattleCruisers.Effects.Deaths.Pools;
using BattleCruisers.Effects.Explosions.Pools;
using BattleCruisers.Projectiles.Pools;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound.Pools;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Utils.Factories
{
    public class PoolProviders
    {
        private ExplosionPoolProvider _explosionPoolProvider;
        public IExplosionPoolProvider ExplosionPoolProvider => _explosionPoolProvider;

        private ShipDeathPoolProvider _shipDeathPoolProvider;
        public IShipDeathPoolProvider ShipDeathPoolProvider => _shipDeathPoolProvider;

        private ProjectilePoolProvider _projectilePoolProvider;
        public IProjectilePoolProvider ProjectilePoolProvider => _projectilePoolProvider;

        private UnitPoolProvider _unitPoolProvider;
        public UnitPoolProvider UnitPoolProvider => _unitPoolProvider;

        private Pool<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs> _audioSourcePool;
        public Pool<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs> AudioSourcePool => _audioSourcePool;

        public UnitToPoolMap UnitToPoolMap { get; }

        // 16 per cruiser
        private const int AUDIO_SOURCE_INITIAL_CAPACITY = 20;

        public PoolProviders(IUIManager uiManager)
        {
            Helper.AssertIsNotNull(uiManager);

            _explosionPoolProvider = new ExplosionPoolProvider();
            _shipDeathPoolProvider = new ShipDeathPoolProvider();
            _projectilePoolProvider = new ProjectilePoolProvider();
            _unitPoolProvider = new UnitPoolProvider(uiManager);

            IPoolableFactory<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs> audioSourceFactory = new AudioSourcePoolableFactory(FactoryProvider.DeferrerProvider.RealTimeDeferrer);
            _audioSourcePool = new Pool<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs>(audioSourceFactory);

            UnitToPoolMap = new UnitToPoolMap(UnitPoolProvider);
        }

        // Not part of constructor, because ProjecilePoolProvider and UnitPollProvider depend on ExplosionPoolProvider :/
        public void SetInitialCapacities()
        {
            /*
            _shipDeathPoolProvider.SetInitialCapacity();
            _projectilePoolProvider.SetInitialCapacity();
            _unitPoolProvider.SetInitialCapacity();
            _dronePool.AddCapacity(DRONES_INITIAL_CAPACITY);
            _audioSourcePool.AddCapacity(AUDIO_SOURCE_INITIAL_CAPACITY);
            */
        }
    }
}