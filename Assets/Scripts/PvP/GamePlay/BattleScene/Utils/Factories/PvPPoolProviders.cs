using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public class PvPPoolProviders : IPvPPoolProviders
    {
        private PvPExplosionPoolProvider _explosionPoolProvider;
        public IPvPExplosionPoolProvider ExplosionPoolProvider => _explosionPoolProvider;

        private PvPShipDeathPoolProvider _shipDeathPoolProvider;
        public IPvPShipDeathPoolProvider ShipDeathPoolProvider => _shipDeathPoolProvider;

        private PvPProjectilePoolProvider _projectilePoolProvider;
        public IPvPProjectilePoolProvider ProjectilePoolProvider => _projectilePoolProvider;

        private PvPUnitPoolProvider _unitPoolProvider;
        public IPvPUnitPoolProvider UnitPoolProvider => _unitPoolProvider;

        private PvPPool<IPvPDroneController, PvPDroneActivationArgs> _dronePool;
        public IPvPPool<IPvPDroneController, PvPDroneActivationArgs> DronePool => _dronePool;

        private PvPPool<IPvPAudioSourcePoolable, PvPAudioSourceActivationArgs> _audioSourcePool;
        public IPvPPool<IPvPAudioSourcePoolable, PvPAudioSourceActivationArgs> AudioSourcePool => _audioSourcePool;

        public IPvPUnitToPoolMap UnitToPoolMap { get; }

        // 16 per cruiser
        private const int DRONES_INITIAL_CAPACITY = 32;
        private const int AUDIO_SOURCE_INITIAL_CAPACITY = 20;


        public PvPPoolProviders(
            IPvPFactoryProvider factoryProvider,
            // IPvPUIManager uiManager,
            IPvPDroneFactory droneFactory)
        {
            PvPHelper.AssertIsNotNull(factoryProvider, droneFactory);

            _explosionPoolProvider = new PvPExplosionPoolProvider(factoryProvider.PrefabFactory);
            _shipDeathPoolProvider = new PvPShipDeathPoolProvider(factoryProvider.PrefabFactory);
            _projectilePoolProvider = new PvPProjectilePoolProvider(factoryProvider);
            _unitPoolProvider = new PvPUnitPoolProvider(factoryProvider);
            _dronePool = new PvPPool<IPvPDroneController, PvPDroneActivationArgs>(droneFactory);
/*
            IPvPAudioSourcePoolableFactory audioSourceFactory = new PvPAudioSourcePoolableFactory(factoryProvider.PrefabFactory, factoryProvider.DeferrerProvider.RealTimeDeferrer);
            _audioSourcePool = new PvPPool<IPvPAudioSourcePoolable, PvPAudioSourceActivationArgs>(audioSourceFactory);*/

            UnitToPoolMap = new PvPUnitToPoolMap(UnitPoolProvider);
        }


        public PvPPoolProviders(
            IPvPFactoryProvider factoryProvider,
            IPvPUIManager uiManager,
            IPvPDroneFactory droneFactory)
        {
            PvPHelper.AssertIsNotNull(factoryProvider, uiManager, droneFactory);

            _explosionPoolProvider = new PvPExplosionPoolProvider(factoryProvider.PrefabFactory);
            _shipDeathPoolProvider = new PvPShipDeathPoolProvider(factoryProvider.PrefabFactory);
            _projectilePoolProvider = new PvPProjectilePoolProvider(factoryProvider);
            _unitPoolProvider = new PvPUnitPoolProvider(uiManager, factoryProvider);
            _dronePool = new PvPPool<IPvPDroneController, PvPDroneActivationArgs>(droneFactory);

            IPvPAudioSourcePoolableFactory audioSourceFactory = new PvPAudioSourcePoolableFactory(factoryProvider.PrefabFactory, factoryProvider.DeferrerProvider.RealTimeDeferrer);
            _audioSourcePool = new PvPPool<IPvPAudioSourcePoolable, PvPAudioSourceActivationArgs>(audioSourceFactory);

            UnitToPoolMap = new PvPUnitToPoolMap(UnitPoolProvider);
        }

        // Not part of constructor, because ProjecilePoolProvider and UnitPollProvider depend on ExplosionPoolProvider :/
        public async Task SetInitialCapacities()
        {
            await _explosionPoolProvider.SetInitialCapacity();
            await _shipDeathPoolProvider.SetInitialCapacity();
            await _projectilePoolProvider.SetInitialCapacity();
            await _unitPoolProvider.SetInitialCapacity();
            await _dronePool.AddCapacity(DRONES_INITIAL_CAPACITY);
        //    _audioSourcePool.AddCapacity(AUDIO_SOURCE_INITIAL_CAPACITY);
        }
    }
}