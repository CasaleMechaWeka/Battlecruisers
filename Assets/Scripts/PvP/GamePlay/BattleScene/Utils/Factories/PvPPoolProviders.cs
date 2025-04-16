using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Effects.Deaths.Pools;
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
    public class PvPPoolProviders
    {
        private PvPExplosionPoolProvider _explosionPoolProvider;
        public IExplosionPoolProvider ExplosionPoolProvider => _explosionPoolProvider;

        private PvPShipDeathPoolProvider _shipDeathPoolProvider;
        public IShipDeathPoolProvider ShipDeathPoolProvider => _shipDeathPoolProvider;

        private PvPProjectilePoolProvider _projectilePoolProvider;
        public IPvPProjectilePoolProvider ProjectilePoolProvider => _projectilePoolProvider;

        private PvPUnitPoolProvider _unitPoolProvider;
        public PvPUnitPoolProvider UnitPoolProvider => _unitPoolProvider;

        private Pool<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs> _audioSourcePool;
        public Pool<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs> AudioSourcePool => _audioSourcePool;

        public PvPUnitToPoolMap UnitToPoolMap { get; }

        // 16 per cruiser
        private const int AUDIO_SOURCE_INITIAL_CAPACITY = 20;

        public PvPPoolProviders(IDroneFactory droneFactory)
        {
            PvPHelper.AssertIsNotNull(droneFactory);

            _explosionPoolProvider = new PvPExplosionPoolProvider();
            _shipDeathPoolProvider = new PvPShipDeathPoolProvider();
            _projectilePoolProvider = new PvPProjectilePoolProvider();
            _unitPoolProvider = new PvPUnitPoolProvider();
            /*
                        IPoolableFactory<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs> audioSourceFactory = new PvPAudioSourcePoolableFactory(factoryProvider.PrefabFactory, factoryProvider.DeferrerProvider.RealTimeDeferrer);
                        _audioSourcePool = new Pool<IPoolable<AudioSourceActivationArgs>, PvPAudioSourceActivationArgs>(audioSourceFactory);*/

            UnitToPoolMap = new PvPUnitToPoolMap(UnitPoolProvider);
        }


        public PvPPoolProviders(
            IPvPUIManager uiManager,
            IDroneFactory droneFactory)
        {
            PvPHelper.AssertIsNotNull(uiManager, droneFactory);

            _explosionPoolProvider = new PvPExplosionPoolProvider();
            _shipDeathPoolProvider = new PvPShipDeathPoolProvider();
            _projectilePoolProvider = new PvPProjectilePoolProvider();
            _unitPoolProvider = new PvPUnitPoolProvider();

            IPoolableFactory<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs> audioSourceFactory
                = new PvPAudioSourcePoolableFactory();
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
        }

        public void SetInitialCapacities_Rest()
        {
            _explosionPoolProvider.SetInitialCapacity_Rest();
            _projectilePoolProvider.SetInitialCapacity_Rest();
        }
    }
}