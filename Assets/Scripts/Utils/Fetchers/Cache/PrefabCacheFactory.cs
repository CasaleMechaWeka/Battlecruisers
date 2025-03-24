using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Effects.Deaths;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Projectiles;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.Sound.Pools;
using BattleCruisers.Utils.DataStrctures;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleCruisers.Utils.Fetchers.Cache
{
    /// <summary>
    /// Retrieving an addressable (such as a prefab) is an async operation.  To avoid async
    /// spread, this all prefabs are loaded at once (at the level start).  This means
    /// consuming code can remain synchronous.  Loading all prefabs takes about 1 seconds.
    /// </summary>
    /// PERF:  Only load prefabs required for level (ie, only 2 hulls, only unlocked buildables)
    public class PrefabCacheFactory
    {
        public async Task<PrefabCache> CreatePrefabCacheAsync()
        {
            IList<Task> retrievePrefabsTasks = new List<Task>();

            IDictionary<IPrefabKey, VariantPrefab> keyToVariants = new ConcurrentDictionary<IPrefabKey, VariantPrefab>();
            retrievePrefabsTasks.Add(GetPrefabs(StaticPrefabKeys.Variants.AllKeys, keyToVariants));

            IDictionary<IPrefabKey, BuildableWrapper<IBuilding>> keyToBuilding = new ConcurrentDictionary<IPrefabKey, BuildableWrapper<IBuilding>>();
            retrievePrefabsTasks.Add(GetPrefabs(StaticPrefabKeys.Buildings.AllKeys, keyToBuilding));

            IDictionary<IPrefabKey, CaptainExo> keyToCaptains = new ConcurrentDictionary<IPrefabKey, CaptainExo>();
            retrievePrefabsTasks.Add(GetPrefabs(StaticPrefabKeys.CaptainExos.AllKeys, keyToCaptains));

            IDictionary<IPrefabKey, Cruiser> keyToCruiser = new ConcurrentDictionary<IPrefabKey, Cruiser>();
            retrievePrefabsTasks.Add(GetPrefabs(StaticPrefabKeys.Hulls.AllKeys, keyToCruiser));

            IDictionary<IPrefabKey, Bodykit> keyToBodykits = new ConcurrentDictionary<IPrefabKey, Bodykit>();
            retrievePrefabsTasks.Add(GetPrefabs(StaticPrefabKeys.BodyKits.AllKeys, keyToBodykits));

            IDictionary<IPrefabKey, ShipDeathInitialiser> keyToDeath = new ConcurrentDictionary<IPrefabKey, ShipDeathInitialiser>();
            retrievePrefabsTasks.Add(GetPrefabs(StaticPrefabKeys.ShipDeaths.AllKeys, keyToDeath));

            IDictionary<IPrefabKey, BuildableWrapper<IUnit>> keyToUnit = new ConcurrentDictionary<IPrefabKey, BuildableWrapper<IUnit>>();
            retrievePrefabsTasks.Add(GetPrefabs(StaticPrefabKeys.Units.AllKeys, keyToUnit));

            IDictionary<IPrefabKey, Projectile> keyToProjectile = new ConcurrentDictionary<IPrefabKey, Projectile>();
            retrievePrefabsTasks.Add(GetPrefabs(StaticPrefabKeys.Projectiles.AllKeys, keyToProjectile));

            IDictionary<IPrefabKey, ExplosionController> keyToExplosion = new ConcurrentDictionary<IPrefabKey, ExplosionController>();
            retrievePrefabsTasks.Add(GetPrefabs(StaticPrefabKeys.Explosions.AllKeys, keyToExplosion));

            Container<DroneController> DroneContainer = new Container<DroneController>();
            retrievePrefabsTasks.Add(GetPrefab(StaticPrefabKeys.Effects.BuilderDrone, DroneContainer));

            Container<AudioSourceInitialiser> AudioSourceContainer = new Container<AudioSourceInitialiser>();
            retrievePrefabsTasks.Add(GetPrefab(StaticPrefabKeys.AudioSource, AudioSourceContainer));

            Logging.Log(Tags.PREFAB_CACHE_FACTORY, "Pre retrieve all prefabs task");
            await Task.WhenAll(retrievePrefabsTasks);
            Logging.Log(Tags.PREFAB_CACHE_FACTORY, "After retrieve all prefabs task");

            return
                new PrefabCache(
                    new MultiCache<BuildableWrapper<IBuilding>>(keyToBuilding),
                    new MultiCache<BuildableWrapper<IUnit>>(keyToUnit),
                    new MultiCache<Cruiser>(keyToCruiser),
                    new MultiCache<ExplosionController>(keyToExplosion),
                    new MultiCache<ShipDeathInitialiser>(keyToDeath),
                    new MultiCache<CaptainExo>(keyToCaptains),
                    new MultiCache<Bodykit>(keyToBodykits),
                    new MultiCache<VariantPrefab>(keyToVariants),
                    new UntypedMultiCache<Projectile>(keyToProjectile),
                    DroneContainer.Value,
                    AudioSourceContainer.Value);
        }


        private async Task GetPrefabs<TPrefab>(
            IList<IPrefabKey> prefabKeys,
            IDictionary<IPrefabKey, TPrefab> keyToPrefab)
                where TPrefab : class, IPrefab
        {
            IEnumerable<Task> prefabTasks = prefabKeys.Select(prefabKey => GetPrefab(keyToPrefab, prefabKey));
            await Task.WhenAll(prefabTasks);
        }

        private async Task GetPrefab<TPrefab>(
            IDictionary<IPrefabKey, TPrefab> keyToPrefab,
            IPrefabKey prefabKey)
                where TPrefab : class, IPrefab
        {
            Logging.Log(Tags.PREFAB_CACHE_FACTORY, "Pre GetPrefabAsync");
            PrefabContainer<TPrefab> prefabContainer = await PrefabFetcher.GetPrefabAsync<TPrefab>(prefabKey);
            Logging.Log(Tags.PREFAB_CACHE_FACTORY, "After GetPrefabAsync");

            prefabContainer.Prefab.StaticInitialise();
            if (!keyToPrefab.ContainsKey(prefabKey))
                keyToPrefab.Add(prefabKey, prefabContainer.Prefab);
        }

        private async Task GetPrefab<TPrefab>(
            IPrefabKey prefabKey,
            Container<TPrefab> prefabContainer)
                where TPrefab : class, IPrefab
        {
            Logging.Log(Tags.PREFAB_CACHE_FACTORY, "Pre GetPrefabAsync");
            PrefabContainer<TPrefab> result = await PrefabFetcher.GetPrefabAsync<TPrefab>(prefabKey);
            prefabContainer.Value = result.Prefab;
            Logging.Log(Tags.PREFAB_CACHE_FACTORY, "After GetPrefabAsync");

            prefabContainer.Value.StaticInitialise();
        }
    }
}