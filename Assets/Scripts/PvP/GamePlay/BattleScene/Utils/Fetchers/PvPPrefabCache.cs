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
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools;
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
        private static PvPAudioSourceInitialiser _audioSource;

        private static readonly object _cacheInitLock = new(); // for thread-safety

        // this code is suboptimal for performance but should be relatively safe,
        // if we need better performance, only selectively waiting for tasks to
        // finish should be possible to implement relatively easily

        public static PvPBuildableWrapper<IPvPBuilding> GetBuilding(IPrefabKey key)
        {
            lock (_cacheInitLock)
            {
                if (retrievePrefabsTasks[0] == null)
                    CreatePvPPrefabCacheAsync().GetAwaiter().GetResult();
                else
                    retrievePrefabsTasks[0].GetAwaiter().GetResult();

                return _buildings.GetPrefab(key);
            }
        }

        public static PvPBuildableWrapper<IPvPUnit> GetUnit(IPrefabKey key)
        {
            lock (_cacheInitLock)
            {
                if (retrievePrefabsTasks[1] == null)
                    CreatePvPPrefabCacheAsync().GetAwaiter().GetResult();
                else
                    retrievePrefabsTasks[1].GetAwaiter().GetResult();
                return _units.GetPrefab(key);
            }
        }
        public static PvPCruiser GetCruiser(IPrefabKey key)
        {
            lock (_cacheInitLock)
            {
                if (retrievePrefabsTasks[2] == null)
                    CreatePvPPrefabCacheAsync().GetAwaiter().GetResult();
                else
                    retrievePrefabsTasks[2].GetAwaiter().GetResult();
                return _cruisers.GetPrefab(key);
            }
        }

        public static PvPExplosionController GetExplosion(IPrefabKey key)
        {
            lock (_cacheInitLock)
            {
                if (retrievePrefabsTasks[3] == null)
                    CreatePvPPrefabCacheAsync().GetAwaiter().GetResult();
                else
                    retrievePrefabsTasks[3].GetAwaiter().GetResult();
                return _explosions.GetPrefab(key);
            }
        }

        public static PvPShipDeathInitialiser GetShipDeath(IPrefabKey key)
        {
            lock (_cacheInitLock)
            {
                if (retrievePrefabsTasks[4] == null)
                    CreatePvPPrefabCacheAsync().GetAwaiter().GetResult();
                else
                    retrievePrefabsTasks[4].GetAwaiter().GetResult();
                return _shipDeaths.GetPrefab(key);
            }
        }

        public static PvPPrefab GetProjectile(IPrefabKey prefabKey)
        {
            lock (_cacheInitLock)
            {
                if (retrievePrefabsTasks[5] == null)
                    CreatePvPPrefabCacheAsync().GetAwaiter().GetResult();
                else
                    retrievePrefabsTasks[5].GetAwaiter().GetResult();
                return _projectiles.GetPrefab(prefabKey);
            }
        }

        public static PvPDroneController Drone
        {
            get
            {
                lock (_cacheInitLock)
                {
                    if (retrievePrefabsTasks[6] == null)
                        CreatePvPPrefabCacheAsync().GetAwaiter().GetResult();
                    else
                        retrievePrefabsTasks[6].GetAwaiter().GetResult();
                    return _drone;
                }
            }
        }

        public static PvPAudioSourceInitialiser AudioSource
        {
            get
            {
                lock (_cacheInitLock)
                {
                    if (retrievePrefabsTasks[7] == null)
                        CreatePvPPrefabCacheAsync().GetAwaiter().GetResult();
                    else
                        retrievePrefabsTasks[7].GetAwaiter().GetResult();
                    return _audioSource;
                }
            }
        }

        public static PvPBuildableOutlineController GetOutline(IPrefabKey key)
        {
            lock (_cacheInitLock)
            {
                if (retrievePrefabsTasks[8] == null)
                    CreatePvPPrefabCacheAsync().GetAwaiter().GetResult();
                else
                    retrievePrefabsTasks[8].GetAwaiter().GetResult();
                return _outlines.GetPrefab(key);
            }
        }

        private static Task[] retrievePrefabsTasks = new Task[9];

        public static async Task CreatePvPPrefabCacheAsync()
        {
            IDictionary<IPrefabKey, PvPBuildableWrapper<IPvPBuilding>> keyToBuilding = new ConcurrentDictionary<IPrefabKey, PvPBuildableWrapper<IPvPBuilding>>();
            IDictionary<IPrefabKey, PvPBuildableWrapper<IPvPUnit>> keyToUnit = new ConcurrentDictionary<IPrefabKey, PvPBuildableWrapper<IPvPUnit>>();
            IDictionary<IPrefabKey, PvPCruiser> keyToCruiser = new ConcurrentDictionary<IPrefabKey, PvPCruiser>();
            IDictionary<IPrefabKey, PvPExplosionController> keyToExplosion = new ConcurrentDictionary<IPrefabKey, PvPExplosionController>();
            IDictionary<IPrefabKey, PvPShipDeathInitialiser> keyToDeath = new ConcurrentDictionary<IPrefabKey, PvPShipDeathInitialiser>();
            IDictionary<IPrefabKey, PvPPrefab> keyToProjectile = new ConcurrentDictionary<IPrefabKey, PvPPrefab>();
            Container<PvPDroneController> droneContainer = new Container<PvPDroneController>();
            Container<PvPAudioSourceInitialiser> audioSourceContainer = new Container<PvPAudioSourceInitialiser>();
            IDictionary<IPrefabKey, PvPBuildableOutlineController> keyToOutline = new ConcurrentDictionary<IPrefabKey, PvPBuildableOutlineController>();

            lock (_cacheInitLock)
            {
                retrievePrefabsTasks = new Task[9];
                retrievePrefabsTasks[0] = GetPrefabs(PvPStaticPrefabKeys.PvPBuildings.AllKeys, keyToBuilding);
                retrievePrefabsTasks[1] = GetPrefabs(PvPStaticPrefabKeys.PvPUnits.AllKeys, keyToUnit);
                retrievePrefabsTasks[2] = GetPrefabs(PvPStaticPrefabKeys.PvPHulls.AllKeys, keyToCruiser);
                retrievePrefabsTasks[3] = GetPrefabs(PvPStaticPrefabKeys.PvPExplosions.AllKeys, keyToExplosion);
                retrievePrefabsTasks[4] = GetPrefabs(PvPStaticPrefabKeys.PvPShipDeaths.AllKeys, keyToDeath);
                retrievePrefabsTasks[5] = GetPrefabs(PvPStaticPrefabKeys.PvPProjectiles.AllKeys, keyToProjectile);
                retrievePrefabsTasks[6] = GetPrefab(PvPStaticPrefabKeys.PvPEffects.PvPBuilderDrone, droneContainer);
                retrievePrefabsTasks[7] = GetPrefab(PvPStaticPrefabKeys.AudioSource, audioSourceContainer);
                retrievePrefabsTasks[8] = GetPrefabs(PvPStaticPrefabKeys.PvPBuildableOutlines.AllKeys, keyToOutline);
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
            _audioSource = audioSourceContainer.Value;
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