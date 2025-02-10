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
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.BuildableOutline;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Data.Models.PrefabKeys;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using Object = UnityEngine.Object;
using BattleCruisers.Utils.Threading;

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

        public IPvPBuildableWrapper<IPvPBuilding> GetBuildingWrapperPrefab(IPrefabKey buildingKey)
        {
            return _prefabCache.GetBuilding(buildingKey);
        }

        public IPvPBuilding CreateBuilding(
            IPvPBuildableWrapper<IPvPBuilding> buildingWrapperPrefab,
            IPvPUIManager uiManager,
            IPvPFactoryProvider factoryProvider,
            ulong clientID)
        {
            return CreateBuildingBuildable(buildingWrapperPrefab.UnityObject, factoryProvider, clientID);
        }

        public IPvPBuildableWrapper<IPvPUnit> GetUnitWrapperPrefab(IPrefabKey unitKey)
        {
            return _prefabCache.GetUnit(unitKey);
        }

        public PvPBuildableOutlineController GetOutline(IPrefabKey outlineKey)
        {
            return _prefabCache.GetOutline(outlineKey);
        }

        public async Task<IPvPUnit> CreateUnit(
            IPvPBuildableWrapper<IPvPUnit> unitWrapperPrefab,
            /* IPvPUIManager uiManager , */
            IPvPFactoryProvider factoryProvider)
        {
            var _unitBuildable = await CreateUnitBuildable(unitWrapperPrefab.UnityObject, factoryProvider);
            return _unitBuildable;
        }

        private TBuildable CreateBuildingBuildable<TBuildable>(
            PvPBuildableWrapper<TBuildable> buildableWrapperPrefab,
            IPvPFactoryProvider factoryProvider,
            ulong clientID) where TBuildable : class, IPvPBuilding
        {
            PvPHelper.AssertIsNotNull(buildableWrapperPrefab, factoryProvider);
            PvPBuildableWrapper<TBuildable> buildableWrapper = Object.Instantiate(buildableWrapperPrefab);
            buildableWrapper.gameObject.SetActive(true);
            buildableWrapper.GetComponent<NetworkObject>().SpawnWithOwnership(clientID);
            buildableWrapper.GetComponent<NetworkObject>().DontDestroyWithOwner = false;
            buildableWrapper.StaticInitialise(_commonStrings);
            buildableWrapper.Buildable.Initialise(factoryProvider);
            return buildableWrapper.Buildable;
        }
        public PvPBuildableOutlineController CreateOutline(PvPBuildableOutlineController outlinePrefab)
        {
            PvPBuildableOutlineController outline = Object.Instantiate(outlinePrefab);
            outline.StaticInitialise(_commonStrings);
            return outline;
        }

        private async Task<TBuildable> CreateUnitBuildable<TBuildable>(
            PvPBuildableWrapper<TBuildable> buildableWrapperPrefab,
            IPvPFactoryProvider factoryProvider) where TBuildable : class, IPvPUnit
        {
            PvPHelper.AssertIsNotNull(buildableWrapperPrefab, factoryProvider);
            PvPBuildableWrapper<TBuildable> buildableWrapper = Object.Instantiate(buildableWrapperPrefab);
            buildableWrapper.gameObject.SetActive(true);
            buildableWrapper.GetComponent<NetworkObject>().Spawn();
            buildableWrapper.StaticInitialise(_commonStrings);
            buildableWrapper.Buildable.Initialise(factoryProvider);
            return buildableWrapper.Buildable;
        }

        public PvPCruiser GetCruiserPrefab(IPrefabKey hullKey)
        {
            return _prefabCache.GetCruiser(hullKey);
        }

        public PvPCruiser CreateCruiser(PvPCruiser cruiserPrefab, ulong ClientNetworkId, float x)
        {
            PvPCruiser cruiser = Object.Instantiate(cruiserPrefab, new Vector3(x, 0f, 0f), Quaternion.identity);
            cruiser.GetComponent<NetworkObject>().SpawnWithOwnership(ClientNetworkId, true);
            cruiser.GetComponent<NetworkObject>().DontDestroyWithOwner = false;
            cruiser.StaticInitialise(_commonStrings);
            return cruiser;
        }
        public PvPCruiser CreateAIBotCruiser(PvPCruiser cruiserPrefab, float x)
        {
            PvPCruiser cruiser = Object.Instantiate(cruiserPrefab, new Vector3(x, 0f, 0f), Quaternion.identity);
            cruiser.GetComponent<NetworkObject>().Spawn();
            cruiser.GetComponent<NetworkObject>().DontDestroyWithOwner = false;
            cruiser.StaticInitialise(_commonStrings);
            cruiser.SetAIBotMode();
            return cruiser;
        }
        public PvPCruiser CreateCruiser(string prefabName, ulong ClientNetworkId, float x)
        {
            PvPCruiser cruiser = GameObject.Instantiate(Resources.Load<PvPCruiser>("Hulls/" + prefabName), new Vector3(x, 0f, 0f), Quaternion.identity);
            cruiser.GetComponent<NetworkObject>().SpawnWithOwnership(ClientNetworkId, true);
            cruiser.GetComponent<NetworkObject>().DontDestroyWithOwner = false;
            cruiser.StaticInitialise(_commonStrings);
            return cruiser;
        }

        public async Task<IPvPExplosion> CreateExplosion(PvPExplosionKey explosionKey)
        {
            PvPExplosionController explosionPrefab = _prefabCache.GetExplosion(explosionKey);
            PvPExplosionController newExplosion = Object.Instantiate(explosionPrefab);
            newExplosion.GetComponent<NetworkObject>().Spawn();
            return newExplosion.Initialise();
        }

        public async Task<IPvPShipDeath> CreateShipDeath(PvPShipDeathKey shipDeathKey)
        {
            PvPShipDeathInitialiser shipDeathPrefab = _prefabCache.GetShipDeath(shipDeathKey);
            PvPShipDeathInitialiser newShipDeath = Object.Instantiate(shipDeathPrefab);
            newShipDeath.GetComponent<NetworkObject>().Spawn();
            return newShipDeath.CreateShipDeath();
        }

        public async Task<TProjectile> CreateProjectile<TProjectile, TActiavtionArgs, TStats>(PvPProjectileKey prefabKey, IPvPFactoryProvider factoryProvider)
            where TProjectile : PvPProjectileControllerBase<TActiavtionArgs, TStats>
            where TActiavtionArgs : PvPProjectileActivationArgs<TStats>
            where TStats : IPvPProjectileStats
        {
            Assert.IsNotNull(factoryProvider);
            TProjectile prefab = _prefabCache.GetProjectile<TProjectile>(prefabKey);
            TProjectile projectile = Object.Instantiate(prefab);
            projectile.GetComponent<NetworkObject>().Spawn();
            projectile.Initialise(_commonStrings, factoryProvider);
            return projectile;
        }

        public async Task<IPvPDroneController> CreateDrone()
        {
            PvPDroneController newDrone = Object.Instantiate(_prefabCache.Drone);
            newDrone.GetComponent<NetworkObject>().Spawn();
            newDrone.StaticInitialise(_commonStrings);
            return newDrone;
        }

        public async Task<IPvPAudioSourcePoolable> CreateAudioSource(IDeferrer realTimeDeferrer)
        {
            Assert.IsNotNull(realTimeDeferrer);
            PvPAudioSourceInitialiser audioSourceInitialiser = Object.Instantiate(_prefabCache.AudioSource);
            return audioSourceInitialiser.Initialise(realTimeDeferrer, _settingsManager);
        }

        public PvPPrefab GetPrefab(string prefabPath)
        {
            return _prefabCache.GetPrefab(prefabPath);
        }

        public async Task<Bodykit> GetBodykit(IPrefabKey prefabKey)
        {
            string addressableKey = "Assets/Resources_moved/" + prefabKey.PrefabPath + ".prefab";
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(addressableKey);
            await handle.Task;
            if (handle.Status != AsyncOperationStatus.Succeeded
    || handle.Result == null)
            {
                throw new ArgumentException("Failed to retrieve prefab: " + addressableKey);
            }
            return handle.Result.GetComponent<Bodykit>();
        }

        public async Task<VariantPrefab> GetVariant(IPrefabKey prefabKey)
        {
            string addressableKey = "Assets/Resources_moved/" + prefabKey.PrefabPath + ".prefab";
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(addressableKey);
            await handle.Task;
            if (handle.Status != AsyncOperationStatus.Succeeded || handle.Result == null)
            {
                throw new ArgumentException("Failed to retrieve prefab: " + addressableKey);
            }
            return handle.Result.GetComponent<VariantPrefab>();
        }
    }
}
