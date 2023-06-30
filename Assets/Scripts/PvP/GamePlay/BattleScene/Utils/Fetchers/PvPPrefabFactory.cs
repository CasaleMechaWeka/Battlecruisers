using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Cache;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using System.Threading.Tasks;
using BattleCruisers.Buildables;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Projectiles;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using static BattleCruisers.Data.Static.StaticPrefabKeys;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers
{
    public class PvPPrefabFactory : IPvPPrefabFactory
    {
        private readonly IPvPPrefabCache _prefabCache;
        private readonly ISettingsManager _settingsManager;
        private readonly ILocTable _commonStrings;

        public PvPPrefabFactory(IPvPPrefabCache prefabCache, ISettingsManager settingsManager, ILocTable commonStrings)
        {
            PvPHelper.AssertIsNotNull(prefabCache, settingsManager, commonStrings);

            _prefabCache = prefabCache;
            _settingsManager = settingsManager;
            _commonStrings = commonStrings;
        }

        public IPvPBuildableWrapper<IPvPBuilding> GetBuildingWrapperPrefab(IPvPPrefabKey buildingKey)
        {
            return _prefabCache.GetBuilding(buildingKey);
        }

        public Task<IPvPBuilding> CreateBuilding(
            IPvPBuildableWrapper<IPvPBuilding> buildingWrapperPrefab,
            IPvPUIManager uiManager,
            IPvPFactoryProvider factoryProvider,
            ulong clientID)
        {
            return CreateBuildingBuildable(buildingWrapperPrefab.UnityObject, factoryProvider, clientID);
        }
        
        public IPvPBuildableWrapper<IPvPUnit> GetUnitWrapperPrefab(IPvPPrefabKey unitKey)
        {
            return _prefabCache.GetUnit(unitKey);
        }

        public async Task<IPvPUnit> CreateUnit(
            IPvPBuildableWrapper<IPvPUnit> unitWrapperPrefab,
            /* IPvPUIManager uiManager , */
            IPvPFactoryProvider factoryProvider)
        {
            var _unitBuildable = await CreateUnitBuildable(unitWrapperPrefab.UnityObject, factoryProvider);
            return _unitBuildable;
        }

        private async Task<TBuildable> CreateBuildingBuildable<TBuildable>(
            PvPBuildableWrapper<TBuildable> buildableWrapperPrefab,
            IPvPFactoryProvider factoryProvider,
            ulong clientID) where TBuildable : class, IPvPBuilding
        {
            PvPHelper.AssertIsNotNull(buildableWrapperPrefab, factoryProvider);

         //   var IsLoaded = await SynchedServerData.Instance.TrySpawnCruiserDynamicSynchronously(new PvPBuildingKey(buildableWrapperPrefab.Buildable.Category, buildableWrapperPrefab.Buildable.PrefabName), buildableWrapperPrefab);

         //   if (IsLoaded)
         //   {
                PvPBuildableWrapper<TBuildable> buildableWrapper = Object.Instantiate(buildableWrapperPrefab);
                buildableWrapper.GetComponent<NetworkObject>().SpawnWithOwnership(clientID);
                buildableWrapper.gameObject.SetActive(true);
                buildableWrapper.StaticInitialise(_commonStrings);
                buildableWrapper.Buildable.Initialise(factoryProvider);
                // Logging.Log(Tags.PREFAB_FACTORY, $"Building: {buildableWrapper.Buildable}  Prefab id: {buildableWrapperPrefab.GetInstanceID()}  New instance id: {buildableWrapper.GetInstanceID()}");
                return buildableWrapper.Buildable;
        //    }
         //   return null;

        }

        private async Task<TBuildable> CreateUnitBuildable<TBuildable>(
            PvPBuildableWrapper<TBuildable> buildableWrapperPrefab,
            IPvPFactoryProvider factoryProvider) where TBuildable : class, IPvPUnit
        {
            PvPHelper.AssertIsNotNull(buildableWrapperPrefab, factoryProvider);

            var IsLoaded = await SynchedServerData.Instance.TrySpawnCruiserDynamicSynchronously(new PvPUnitKey(buildableWrapperPrefab.Buildable.Category, buildableWrapperPrefab.Buildable.PrefabName), buildableWrapperPrefab);

            if (IsLoaded)
            {
                PvPBuildableWrapper<TBuildable> buildableWrapper = Object.Instantiate(buildableWrapperPrefab);
                buildableWrapper.GetComponent<NetworkObject>().Spawn();
                buildableWrapper.gameObject.SetActive(true);
                buildableWrapper.StaticInitialise(_commonStrings);
                buildableWrapper.Buildable.Initialise(factoryProvider);
                return buildableWrapper.Buildable;
            }

            return null;
            // Logging.Log(Tags.PREFAB_FACTORY, $"Building: {buildableWrapper.Buildable}  Prefab id: {buildableWrapperPrefab.GetInstanceID()}  New instance id: {buildableWrapper.GetInstanceID()}");

        }

        public PvPCruiser GetCruiserPrefab(IPvPPrefabKey hullKey)
        {
            return _prefabCache.GetCruiser(hullKey);
        }

        public PvPCruiser CreateCruiser(PvPCruiser cruiserPrefab, ulong ClientNetworkId, float x)
        {
            PvPCruiser cruiser = Object.Instantiate(cruiserPrefab, new Vector3(x, 0f, 0f), Quaternion.identity);
            cruiser.GetComponent<NetworkObject>().SpawnWithOwnership(ClientNetworkId);
            cruiser.StaticInitialise(_commonStrings);
            return cruiser;
        }

        public async Task<IPvPExplosion> CreateExplosion(PvPExplosionKey explosionKey)
        {
            PvPExplosionController explosionPrefab = _prefabCache.GetExplosion(explosionKey);
            var IsLoaded = await SynchedServerData.Instance.TrySpawnCruiserDynamicSynchronously(explosionKey, explosionPrefab);
            if (IsLoaded)
            {
                PvPExplosionController newExplosion = Object.Instantiate(explosionPrefab);
                newExplosion.GetComponent<NetworkObject>().Spawn();
                return newExplosion.Initialise();
            }
            return null;
        }

        public async Task<IPvPShipDeath> CreateShipDeath(PvPShipDeathKey shipDeathKey)
        {
            PvPShipDeathInitialiser shipDeathPrefab = _prefabCache.GetShipDeath(shipDeathKey);
            var IsLoaded = await SynchedServerData.Instance.TrySpawnCruiserDynamicSynchronously(shipDeathKey, shipDeathPrefab);
            if (IsLoaded)
            {
                PvPShipDeathInitialiser newShipDeath = Object.Instantiate(shipDeathPrefab);
                newShipDeath.GetComponent<NetworkObject>().Spawn();
                return newShipDeath.CreateShipDeath();
            }
            return null;
        }

        public async Task<TProjectile> CreateProjectile<TProjectile, TActiavtionArgs, TStats>(PvPProjectileKey prefabKey, IPvPFactoryProvider factoryProvider)
            where TProjectile : PvPProjectileControllerBase<TActiavtionArgs, TStats>
            where TActiavtionArgs : PvPProjectileActivationArgs<TStats>
            where TStats : IPvPProjectileStats
        {
            Assert.IsNotNull(factoryProvider);

            TProjectile prefab = _prefabCache.GetProjectile<TProjectile>(prefabKey);
            var IsLoaded = await SynchedServerData.Instance.TrySpawnCruiserDynamicSynchronously(prefabKey, prefab);
            if (IsLoaded)
            {
                TProjectile projectile = Object.Instantiate(prefab);
                projectile.GetComponent<NetworkObject>().Spawn();
                projectile.Initialise(_commonStrings, factoryProvider);
                return projectile;
            }

            return null;
        }

        public async Task<IPvPDroneController> CreateDrone()
        {
            var IsLoaded = await SynchedServerData.Instance.TrySpawnCruiserDynamicSynchronously(PvPStaticPrefabKeys.PvPEffects.PvPBuilderDrone, _prefabCache.Drone);
            if (IsLoaded)
            {
                PvPDroneController newDrone = Object.Instantiate(_prefabCache.Drone);
                newDrone.GetComponent<NetworkObject>().Spawn();
                newDrone.StaticInitialise(_commonStrings);
                return newDrone;
            }
            return null;
        }

        public async Task<IPvPAudioSourcePoolable> CreateAudioSource(IPvPDeferrer realTimeDeferrer)
        {
            Assert.IsNotNull(realTimeDeferrer);
        //    var IsLoaded = await SynchedServerData.Instance.TrySpawnCruiserDynamicSynchronously(PvPStaticPrefabKeys.AudioSource, _prefabCache.AudioSource);
        //    if (IsLoaded)
        //    {
                PvPAudioSourceInitialiser audioSourceInitialiser = Object.Instantiate(_prefabCache.AudioSource);
                return audioSourceInitialiser.Initialise(realTimeDeferrer, _settingsManager);
        //    }
        //    return null;
        }

        public PvPPrefab GetPrefab(string prefabPath)
        {
            return _prefabCache.GetPrefab(prefabPath);
        }
    }
}
