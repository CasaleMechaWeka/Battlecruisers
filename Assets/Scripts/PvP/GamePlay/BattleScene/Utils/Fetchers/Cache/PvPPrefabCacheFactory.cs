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
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IPvPPrefabCache> CreatePrefabCacheAsync(IPrefabFetcher prefabFetcher)
        {
            Assert.IsNotNull(prefabFetcher);

            IList<Task> retrievePrefabsTasks = new List<Task>();

            IDictionary<IPrefabKey, PvPBuildableWrapper<IPvPBuilding>> keyToBuilding = new ConcurrentDictionary<IPrefabKey, PvPBuildableWrapper<IPvPBuilding>>();
            retrievePrefabsTasks.Add(GetPrefabs(prefabFetcher, PvPStaticPrefabKeys.PvPBuildings.AllKeys, keyToBuilding));

            IDictionary<IPrefabKey, PvPBuildableWrapper<IPvPUnit>> keyToUnit = new ConcurrentDictionary<IPrefabKey, PvPBuildableWrapper<IPvPUnit>>();
            retrievePrefabsTasks.Add(GetPrefabs(prefabFetcher, PvPStaticPrefabKeys.PvPUnits.AllKeys, keyToUnit));

            IDictionary<IPrefabKey, PvPCruiser> keyToCruiser = new ConcurrentDictionary<IPrefabKey, PvPCruiser>();
            retrievePrefabsTasks.Add(GetPrefabs(prefabFetcher, PvPStaticPrefabKeys.PvPHulls.AllKeys, keyToCruiser));

            IDictionary<IPrefabKey, PvPExplosionController> keyToExplosion = new ConcurrentDictionary<IPrefabKey, PvPExplosionController>();
            retrievePrefabsTasks.Add(GetPrefabs(prefabFetcher, PvPStaticPrefabKeys.PvPExplosions.AllKeys, keyToExplosion));

            IDictionary<IPrefabKey, PvPShipDeathInitialiser> keyToDeath = new ConcurrentDictionary<IPrefabKey, PvPShipDeathInitialiser>();
            retrievePrefabsTasks.Add(GetPrefabs(prefabFetcher, PvPStaticPrefabKeys.PvPShipDeaths.AllKeys, keyToDeath));

            IDictionary<IPrefabKey, PvPProjectile> keyToProjectile = new ConcurrentDictionary<IPrefabKey, PvPProjectile>();
            retrievePrefabsTasks.Add(GetPrefabs(prefabFetcher, PvPStaticPrefabKeys.PvPProjectiles.AllKeys, keyToProjectile));

            Container<PvPDroneController> droneContainer = new Container<PvPDroneController>();
            retrievePrefabsTasks.Add(GetPrefab(prefabFetcher, PvPStaticPrefabKeys.PvPEffects.PvPBuilderDrone, droneContainer));

            Container<PvPAudioSourceInitialiser> audioSourceContainer = new Container<PvPAudioSourceInitialiser>();
            retrievePrefabsTasks.Add(GetPrefab(prefabFetcher, PvPStaticPrefabKeys.AudioSource, audioSourceContainer));

            IDictionary<IPrefabKey, PvPBuildableOutlineController> keyToOutline = new ConcurrentDictionary<IPrefabKey, PvPBuildableOutlineController>();
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
            IPrefabFetcher prefabFetcher,
            IList<IPrefabKey> prefabKeys,
            IDictionary<IPrefabKey, TPrefab> keyToPrefab)
                where TPrefab : class, IPrefab
        {
            IEnumerable<Task> prefabTasks = prefabKeys.Select(prefabKey => GetPrefab(prefabFetcher, keyToPrefab, prefabKey));
            await Task.WhenAll(prefabTasks);
        }

        private async Task GetPrefab<TPrefab>(
            IPrefabFetcher prefabFetcher,
            IDictionary<IPrefabKey, TPrefab> keyToPrefab,
            IPrefabKey prefabKey)
                where TPrefab : class, IPrefab
        {
            // Logging.Log(Tags.PREFAB_CACHE_FACTORY, "Pre GetPrefabAsync");
            IPrefabContainer<TPrefab> prefabContainer = await prefabFetcher.GetPrefabAsync<TPrefab>(prefabKey);
            // Logging.Log(Tags.PREFAB_CACHE_FACTORY, "After GetPrefabAsync");
            prefabContainer.Prefab.StaticInitialise(_commonStrings);
            keyToPrefab.Add(prefabKey, prefabContainer.Prefab);
        }

        private async Task GetPrefab<TPrefab>(
            IPrefabFetcher prefabFetcher,
            IPrefabKey prefabKey,
            Container<TPrefab> prefabContainer)
                where TPrefab : class, IPrefab
        {
            // Logging.Log(Tags.PREFAB_CACHE_FACTORY, "Pre GetPrefabAsync");
            IPrefabContainer<TPrefab> result = await prefabFetcher.GetPrefabAsync<TPrefab>(prefabKey);
            prefabContainer.Value = result.Prefab;
            // Logging.Log(Tags.PREFAB_CACHE_FACTORY, "After GetPrefabAsync");

            prefabContainer.Value.StaticInitialise(_commonStrings);
        }
    }
}