using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.BuildableOutline;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using BattleCruisers.Utils.Localisation;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Cache
{
    /// <summary>
    /// Retrieving an addressable (such as a prefab) is an async operation.  To avoid async
    /// spread, this all prefabs are loaded at once (at the level start).  This means
    /// consuming code can remain synchronous.  Loading all prefabs takes about 1 seconds.
    /// </summary>
    /// PERF:  Only load prefabs required for level (ie, only 2 hulls, only unlocked buildables)
    public class PvPPrefabCacheFactory : IPvPPrefabCacheFactory
    {
        private readonly ILocTable _commonStrings;

        public PvPPrefabCacheFactory(ILocTable commonStrings)
        {
            Assert.IsNotNull(commonStrings);
            _commonStrings = commonStrings;
        }

        public async Task<IPvPPrefabCache> CreatePrefabCacheAsync(IPvPPrefabFetcher prefabFetcher)
        {
            Assert.IsNotNull(prefabFetcher);

            IList<Task> retrievePrefabsTasks = new List<Task>();

            IDictionary<IPvPPrefabKey, PvPBuildableWrapper<IPvPBuilding>> keyToBuilding = new ConcurrentDictionary<IPvPPrefabKey, PvPBuildableWrapper<IPvPBuilding>>();
            retrievePrefabsTasks.Add(GetPrefabs(prefabFetcher, PvPStaticPrefabKeys.PvPBuildings.AllKeys, keyToBuilding));

            IDictionary<IPvPPrefabKey, PvPBuildableWrapper<IPvPUnit>> keyToUnit = new ConcurrentDictionary<IPvPPrefabKey, PvPBuildableWrapper<IPvPUnit>>();
            retrievePrefabsTasks.Add(GetPrefabs(prefabFetcher, PvPStaticPrefabKeys.PvPUnits.AllKeys, keyToUnit));

            IDictionary<IPvPPrefabKey, PvPCruiser> keyToCruiser = new ConcurrentDictionary<IPvPPrefabKey, PvPCruiser>();
            retrievePrefabsTasks.Add(GetPrefabs(prefabFetcher, PvPStaticPrefabKeys.PvPHulls.AllKeys, keyToCruiser));

            IDictionary<IPvPPrefabKey, PvPExplosionController> keyToExplosion = new ConcurrentDictionary<IPvPPrefabKey, PvPExplosionController>();
            retrievePrefabsTasks.Add(GetPrefabs(prefabFetcher, PvPStaticPrefabKeys.PvPExplosions.AllKeys, keyToExplosion));

            IDictionary<IPvPPrefabKey, PvPShipDeathInitialiser> keyToDeath = new ConcurrentDictionary<IPvPPrefabKey, PvPShipDeathInitialiser>();
            retrievePrefabsTasks.Add(GetPrefabs(prefabFetcher, PvPStaticPrefabKeys.PvPShipDeaths.AllKeys, keyToDeath));

            IDictionary<IPvPPrefabKey, PvPProjectile> keyToProjectile = new ConcurrentDictionary<IPvPPrefabKey, PvPProjectile>();
            retrievePrefabsTasks.Add(GetPrefabs(prefabFetcher, PvPStaticPrefabKeys.PvPProjectiles.AllKeys, keyToProjectile));

            PvPContainer<PvPDroneController> droneContainer = new PvPContainer<PvPDroneController>();
            retrievePrefabsTasks.Add(GetPrefab(prefabFetcher, PvPStaticPrefabKeys.PvPEffects.PvPBuilderDrone, droneContainer));

            PvPContainer<PvPAudioSourceInitialiser> audioSourceContainer = new PvPContainer<PvPAudioSourceInitialiser>();
            retrievePrefabsTasks.Add(GetPrefab(prefabFetcher, PvPStaticPrefabKeys.AudioSource, audioSourceContainer));

            IDictionary<IPvPPrefabKey, PvPBuildableOutlineController> keyToOutline = new ConcurrentDictionary<IPvPPrefabKey, PvPBuildableOutlineController>();
            retrievePrefabsTasks.Add(GetPrefabs(prefabFetcher, PvPStaticPrefabKeys.PvPBuildableOutlines.AllKeys, keyToOutline));

            // Logging.Log(Tags.PREFAB_CACHE_FACTORY, "Pre retrieve all prefabs task");
            await Task.WhenAll(retrievePrefabsTasks);
            // Logging.Log(Tags.PREFAB_CACHE_FACTORY, "After retrieve all prefabs task");

            return
                new PvPPrefabCache(
                    new PvPMultiCache<PvPBuildableWrapper<IPvPBuilding>>(keyToBuilding),
                    new PvPMultiCache<PvPBuildableWrapper<IPvPUnit>>(keyToUnit),
                    new PvPMultiCache<PvPCruiser>(keyToCruiser),
                    new PvPMultiCache<PvPExplosionController>(keyToExplosion),
                    new PvPMultiCache<PvPShipDeathInitialiser>(keyToDeath),
                    new PvPUntypedMultiCache<PvPProjectile>(keyToProjectile),
                    droneContainer.Value,
                    audioSourceContainer.Value,
                    new PvPMultiCache<PvPBuildableOutlineController>(keyToOutline));
        }

        private async Task GetPrefabs<TPrefab>(
            IPvPPrefabFetcher prefabFetcher,
            IList<IPvPPrefabKey> prefabKeys,
            IDictionary<IPvPPrefabKey, TPrefab> keyToPrefab)
                where TPrefab : class, IPvPPrefab
        {
            IEnumerable<Task> prefabTasks = prefabKeys.Select(prefabKey => GetPrefab(prefabFetcher, keyToPrefab, prefabKey));
            await Task.WhenAll(prefabTasks);
        }

        private async Task GetPrefab<TPrefab>(
            IPvPPrefabFetcher prefabFetcher,
            IDictionary<IPvPPrefabKey, TPrefab> keyToPrefab,
            IPvPPrefabKey prefabKey)
                where TPrefab : class, IPvPPrefab
        {
            // Logging.Log(Tags.PREFAB_CACHE_FACTORY, "Pre GetPrefabAsync");
            IPvPPrefabContainer<TPrefab> prefabContainer = await prefabFetcher.GetPrefabAsync<TPrefab>(prefabKey);
            // Logging.Log(Tags.PREFAB_CACHE_FACTORY, "After GetPrefabAsync");

            prefabContainer.Prefab.StaticInitialise(_commonStrings);
            keyToPrefab.Add(prefabKey, prefabContainer.Prefab);
        }

        private async Task GetPrefab<TPrefab>(
            IPvPPrefabFetcher prefabFetcher,
            IPvPPrefabKey prefabKey,
            PvPContainer<TPrefab> prefabContainer)
                where TPrefab : class, IPvPPrefab
        {
            // Logging.Log(Tags.PREFAB_CACHE_FACTORY, "Pre GetPrefabAsync");
            IPvPPrefabContainer<TPrefab> result = await prefabFetcher.GetPrefabAsync<TPrefab>(prefabKey);
            prefabContainer.Value = result.Prefab;
            // Logging.Log(Tags.PREFAB_CACHE_FACTORY, "After GetPrefabAsync");

            prefabContainer.Value.StaticInitialise(_commonStrings);
        }
    }
}