using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.BuildableOutline;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Fetchers.Cache;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers
{
    public static class PvPPrefabCache
    {
        private static MultiCache<PvPBuildableWrapper<IPvPBuilding>> _buildings;
        private static MultiCache<PvPBuildableWrapper<IPvPUnit>> _units;
        private static MultiCache<PvPCruiser> _cruisers;
        private static MultiCache<PvPExplosionController> _explosions;
        private static MultiCache<PvPShipDeathInitialiser> _shipDeaths;
        private static MultiCache<PvPPrefab> _projectiles;
        private static MultiCache<PvPBuildableOutlineController> _outlines;
        private static PvPDroneController _drone;

        private static readonly object _cacheInitLock = new(); // for thread-safety

        // this code is suboptimal for performance but should be relatively safe,
        // if we need better performance, only selectively waiting for tasks to
        // finish should be possible to implement relatively easily

        public static PvPBuildableWrapper<IPvPBuilding> GetBuilding(IPrefabKey key)
        {
            if (_buildings != null)
                return _buildings.GetPrefab(key);

            return PrefabFetcher.GetPrefabSync<PvPBuildableWrapper<IPvPBuilding>>(key);
        }

        public static PvPBuildableWrapper<IPvPUnit> GetUnit(IPrefabKey key)
        {
            if (_units != null)
                return _units.GetPrefab(key);

            return PrefabFetcher.GetPrefabSync<PvPBuildableWrapper<IPvPUnit>>(key);

        }
        public static PvPCruiser GetCruiser(IPrefabKey key)
        {
            if (_cruisers != null)
                return _cruisers.GetPrefab(key);

            return PrefabFetcher.GetPrefabSync<PvPCruiser>(key);
        }

        public static PvPExplosionController GetExplosion(IPrefabKey key)
        {
            if (_explosions != null)
                return _explosions.GetPrefab(key);
            return PrefabFetcher.GetPrefabSync<PvPExplosionController>(key);
        }

        public static PvPShipDeathInitialiser GetShipDeath(IPrefabKey key)
        {
            if (_shipDeaths != null)
                return _shipDeaths.GetPrefab(key);
            return PrefabFetcher.GetPrefabSync<PvPShipDeathInitialiser>(key);
        }

        public static PvPPrefab GetProjectile(IPrefabKey key)
        {
            if (_shipDeaths != null)
                return _projectiles.GetPrefab(key);
            return PrefabFetcher.GetPrefabSync<PvPPrefab>(key);
        }

        public static PvPDroneController Drone
        {
            get
            {
                if (_drone != null)
                    return _drone;
                return PrefabFetcher.GetPrefabSync<PvPDroneController>(PvPStaticPrefabKeys.PvPEffects.PvPBuilderDrone);
            }
        }

        public static PvPBuildableOutlineController GetOutline(IPrefabKey key)
        {
            if (retrievePrefabsTasks[7] == null)
                CreatePvPPrefabCacheAsync().GetAwaiter().GetResult();
            else
                retrievePrefabsTasks[7].GetAwaiter().GetResult();
            return _outlines.GetPrefab(key);
        }

        private static Task[] retrievePrefabsTasks = new Task[9];

        public static async Task CreatePvPPrefabCacheAsync()
        {
            lock (_cacheInitLock)
            {
                if (retrievePrefabsTasks.Any(t => t != null)) return; // Already started
            }

            IDictionary<IPrefabKey, PvPBuildableWrapper<IPvPBuilding>> keyToBuilding;
            IDictionary<IPrefabKey, PvPBuildableWrapper<IPvPUnit>> keyToUnit;
            IDictionary<IPrefabKey, PvPCruiser> keyToCruiser;
            IDictionary<IPrefabKey, PvPExplosionController> keyToExplosion;
            IDictionary<IPrefabKey, PvPShipDeathInitialiser> keyToDeath;
            IDictionary<IPrefabKey, PvPPrefab> keyToProjectile;
            Container<PvPDroneController> droneContainer;
            IDictionary<IPrefabKey, PvPBuildableOutlineController> keyToOutline;

            lock (_cacheInitLock)
            {
                keyToBuilding = new ConcurrentDictionary<IPrefabKey, PvPBuildableWrapper<IPvPBuilding>>();
                keyToUnit = new ConcurrentDictionary<IPrefabKey, PvPBuildableWrapper<IPvPUnit>>();
                keyToCruiser = new ConcurrentDictionary<IPrefabKey, PvPCruiser>();
                keyToExplosion = new ConcurrentDictionary<IPrefabKey, PvPExplosionController>();
                keyToDeath = new ConcurrentDictionary<IPrefabKey, PvPShipDeathInitialiser>();
                keyToProjectile = new ConcurrentDictionary<IPrefabKey, PvPPrefab>();
                droneContainer = new Container<PvPDroneController>();
                keyToOutline = new ConcurrentDictionary<IPrefabKey, PvPBuildableOutlineController>();

                retrievePrefabsTasks = new Task[8];
                retrievePrefabsTasks[0] = GetPrefabs(PvPStaticPrefabKeys.PvPBuildings.AllKeys, keyToBuilding);
                retrievePrefabsTasks[1] = GetPrefabs(PvPStaticPrefabKeys.PvPUnits.AllKeys, keyToUnit);
                retrievePrefabsTasks[2] = GetPrefabs(PvPStaticPrefabKeys.PvPHulls.AllKeys, keyToCruiser);
                retrievePrefabsTasks[3] = GetPrefabs(PvPStaticPrefabKeys.PvPExplosions.AllKeys, keyToExplosion);
                retrievePrefabsTasks[4] = GetPrefabs(PvPStaticPrefabKeys.PvPShipDeaths.AllKeys, keyToDeath);
                retrievePrefabsTasks[5] = GetPrefabs(PvPStaticPrefabKeys.PvPProjectiles.AllKeys, keyToProjectile);
                retrievePrefabsTasks[6] = GetPrefab(PvPStaticPrefabKeys.PvPEffects.PvPBuilderDrone, droneContainer);
                retrievePrefabsTasks[7] = GetPrefabs(PvPStaticPrefabKeys.PvPBuildableOutlines.AllKeys, keyToOutline);
                // Logging.Log(Tags.PREFAB_CACHE_FACTORY, "Pre retrieve all prefabs task");
            }

            await Task.WhenAll(retrievePrefabsTasks);

            _buildings = new MultiCache<PvPBuildableWrapper<IPvPBuilding>>(keyToBuilding);
            _units = new MultiCache<PvPBuildableWrapper<IPvPUnit>>(keyToUnit);
            _cruisers = new MultiCache<PvPCruiser>(keyToCruiser);
            _explosions = new MultiCache<PvPExplosionController>(keyToExplosion);
            _shipDeaths = new MultiCache<PvPShipDeathInitialiser>(keyToDeath);
            _projectiles = new MultiCache<PvPPrefab>(keyToProjectile);
            _drone = droneContainer.Value;
            _outlines = new MultiCache<PvPBuildableOutlineController>(keyToOutline);
        }

        public static void Clear()
        {
            Debug.Log("CLEARED PVP CACHE");
            _buildings = null;
            _units = null;
            _cruisers = null;
            _explosions = null;
            _shipDeaths = null;
            _projectiles = null;
            _outlines = null;
            retrievePrefabsTasks = new Task[9];
        }

        private static async Task GetPrefabs<TPrefab>(
        IList<IPrefabKey> prefabKeys,
        IDictionary<IPrefabKey, TPrefab> keyToPrefab)
        where TPrefab : class, IPrefab
        {
            IEnumerable<Task> prefabTasks = prefabKeys.Select(prefabKey => GetPrefab(keyToPrefab, prefabKey));
            await Task.WhenAll(prefabTasks);
        }

        private static async Task GetPrefab<TPrefab>(
            IDictionary<IPrefabKey, TPrefab> keyToPrefab,
            IPrefabKey prefabKey)
                where TPrefab : class, IPrefab
        {
            // Logging.Log(Tags.PREFAB_CACHE_FACTORY, "Pre GetPrefabAsync");
            PrefabContainer<TPrefab> prefabContainer = await PrefabFetcher.GetPrefabAsync<TPrefab>(prefabKey);
            // Logging.Log(Tags.PREFAB_CACHE_FACTORY, "After GetPrefabAsync");
            prefabContainer.Prefab.StaticInitialise();
            keyToPrefab.Add(prefabKey, prefabContainer.Prefab);
        }

        private static async Task GetPrefab<TPrefab>(
            IPrefabKey prefabKey,
            Container<TPrefab> prefabContainer)
                where TPrefab : class, IPrefab
        {
            // Logging.Log(Tags.PREFAB_CACHE_FACTORY, "Pre GetPrefabAsync");
            PrefabContainer<TPrefab> result = await PrefabFetcher.GetPrefabAsync<TPrefab>(prefabKey);
            prefabContainer.Value = result.Prefab;
            // Logging.Log(Tags.PREFAB_CACHE_FACTORY, "After GetPrefabAsync");

            prefabContainer.Value.StaticInitialise();
        }
    }
}