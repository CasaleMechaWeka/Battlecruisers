using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Effects.Deaths.Pools;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Effects.Explosions.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools;
using BattleCruisers.UI.Sound.Pools;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public class PvPPoolProviders : IPvPPoolProviders
    {
        private PvPExplosionPoolProvider _explosionPoolProvider;
        public IExplosionPoolProvider ExplosionPoolProvider => _explosionPoolProvider;

        private PvPShipDeathPoolProvider _shipDeathPoolProvider;
        public IShipDeathPoolProvider ShipDeathPoolProvider => _shipDeathPoolProvider;

        private PvPProjectilePoolProvider _projectilePoolProvider;
        public IPvPProjectilePoolProvider ProjectilePoolProvider => _projectilePoolProvider;

        private PvPUnitPoolProvider _unitPoolProvider;
        public IPvPUnitPoolProvider UnitPoolProvider => _unitPoolProvider;

        private Pool<IDroneController, DroneActivationArgs> _dronePool;
        public Pool<IDroneController, DroneActivationArgs> DronePool => _dronePool;

        private Pool<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs> _audioSourcePool;
        public Pool<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs> AudioSourcePool => _audioSourcePool;

        public IPvPUnitToPoolMap UnitToPoolMap { get; }

        // 16 per cruiser
        private const int DRONES_INITIAL_CAPACITY = 32;
        private const int AUDIO_SOURCE_INITIAL_CAPACITY = 20;


        public PvPPoolProviders(
            IPvPFactoryProvider factoryProvider,
            // IPvPUIManager uiManager,
            IDroneFactory droneFactory)
        {
            PvPHelper.AssertIsNotNull(factoryProvider, droneFactory);

            _explosionPoolProvider = new PvPExplosionPoolProvider(factoryProvider.PrefabFactory);
            _shipDeathPoolProvider = new PvPShipDeathPoolProvider(factoryProvider.PrefabFactory);
            _projectilePoolProvider = new PvPProjectilePoolProvider(factoryProvider);
            _unitPoolProvider = new PvPUnitPoolProvider(factoryProvider);
            _dronePool = new Pool<IDroneController, DroneActivationArgs>(droneFactory);
            /*
                        IPoolableFactory<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs> audioSourceFactory = new PvPAudioSourcePoolableFactory(factoryProvider.PrefabFactory, factoryProvider.DeferrerProvider.RealTimeDeferrer);
                        _audioSourcePool = new Pool<IPoolable<AudioSourceActivationArgs>, PvPAudioSourceActivationArgs>(audioSourceFactory);*/

            UnitToPoolMap = new PvPUnitToPoolMap(UnitPoolProvider);
        }


        public PvPPoolProviders(
            IPvPFactoryProvider factoryProvider,
            IPvPUIManager uiManager,
            IDroneFactory droneFactory)
        {
            PvPHelper.AssertIsNotNull(factoryProvider, uiManager, droneFactory);

            _explosionPoolProvider = new PvPExplosionPoolProvider(factoryProvider.PrefabFactory);
            _shipDeathPoolProvider = new PvPShipDeathPoolProvider(factoryProvider.PrefabFactory);
            _projectilePoolProvider = new PvPProjectilePoolProvider(factoryProvider);
            _unitPoolProvider = new PvPUnitPoolProvider(uiManager, factoryProvider);
            _dronePool = new Pool<IDroneController, DroneActivationArgs>(droneFactory);

            IPoolableFactory<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs> audioSourceFactory
                = new PvPAudioSourcePoolableFactory(factoryProvider.PrefabFactory, factoryProvider.DeferrerProvider.RealTimeDeferrer);
            _audioSourcePool = new Pool<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs>(audioSourceFactory);

            UnitToPoolMap = new PvPUnitToPoolMap(UnitPoolProvider);
        }

        // Not part of constructor, because ProjecilePoolProvider and UnitPollProvider depend on ExplosionPoolProvider :/
        public void SetInitialCapacities()
        {
            _explosionPoolProvider.SetInitialCapacity();
            _shipDeathPoolProvider.SetInitialCapacity();
            _projectilePoolProvider.SetInitialCapacity();
            _unitPoolProvider.SetInitialCapacity();
            _dronePool.AddCapacity(DRONES_INITIAL_CAPACITY);
        }

        public void SetInitialCapacities_Rest()
        {
            _explosionPoolProvider.SetInitialCapacity_Rest();
            _projectilePoolProvider.SetInitialCapacity_Rest();
        }
    }
}