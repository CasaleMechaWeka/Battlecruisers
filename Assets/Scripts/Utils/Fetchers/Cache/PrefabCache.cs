using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Effects.Deaths;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Effects.Explosions;
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
    /// consuming code can remain synchronous.  Loading all prefabs takes about 2 seconds.
    /// </summary>
    /// PERF:  Only load prefabs required for level (ie, only 2 hulls, only unlocked buildables)
    public static class PrefabCache
    {
        private static MultiCache<BuildableWrapper<IBuilding>> _buildings;
        private static MultiCache<BuildableWrapper<IUnit>> _units;
        private static MultiCache<Cruiser> _cruisers;
        private static MultiCache<ExplosionController> _explosions;
        private static MultiCache<ShipDeathInitialiser> _shipDeaths;
        private static MultiCache<CaptainExo> _captains;
        private static MultiCache<Bodykit> _bodykits;
        private static MultiCache<VariantPrefab> _variants;
        private static MultiCache<Prefab> _projectiles;

        public static BuildableWrapper<IBuilding> GetBuilding(IPrefabKey key) => _buildings.GetPrefab(key);
        public static BuildableWrapper<IUnit> GetUnit(IPrefabKey key) => _units.GetPrefab(key);
        public static Cruiser GetCruiser(IPrefabKey key) => _cruisers.GetPrefab(key);
        public static ExplosionController GetExplosion(IPrefabKey key) => _explosions.GetPrefab(key);
        public static ShipDeathInitialiser GetShipDeath(IPrefabKey key) => _shipDeaths.GetPrefab(key);
        public static CaptainExo GetCaptainExo(IPrefabKey key) => _captains.GetPrefab(key);
        public static Bodykit GetBodykit(IPrefabKey key) => _bodykits.GetPrefab(key);
        public static VariantPrefab GetVariant(IPrefabKey key) => _variants.GetPrefab(key);
        public static Prefab GetProjectile(IPrefabKey key) => _projectiles.GetPrefab(key);
        public static DroneController Drone { get; private set; }
        public static AudioSourceInitialiser AudioSource { get; private set; }

        public static async Task CreatePrefabCacheAsync()
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

            IDictionary<IPrefabKey, Prefab> keyToProjectile = new ConcurrentDictionary<IPrefabKey, Prefab>();
            retrievePrefabsTasks.Add(GetPrefabs(StaticPrefabKeys.Projectiles.AllKeys, keyToProjectile));

            IDictionary<IPrefabKey, ExplosionController> keyToExplosion = new ConcurrentDictionary<IPrefabKey, ExplosionController>();
            retrievePrefabsTasks.Add(GetPrefabs(StaticPrefabKeys.Explosions.AllKeys, keyToExplosion));

            Container<DroneController> DroneContainer = new Container<DroneController>();
            retrievePrefabsTasks.Add(GetPrefab(StaticPrefabKeys.Effects.BuilderDrone, DroneContainer));

            Container<AudioSourceInitialiser> AudioSourceContainer = new Container<AudioSourceInitialiser>();
            retrievePrefabsTasks.Add(GetPrefab(StaticPrefabKeys.AudioSource, AudioSourceContainer));


            Logging.Log(Tags.PREFAB_CACHE_FACTORY, "Pre retrieve all prefabs task");
            await Task.WhenAll(retrievePrefabsTasks);

            _buildings = new MultiCache<BuildableWrapper<IBuilding>>(keyToBuilding);
            _units = new MultiCache<BuildableWrapper<IUnit>>(keyToUnit);
            _units = new MultiCache<BuildableWrapper<IUnit>>(keyToUnit);
            _cruisers = new MultiCache<Cruiser>(keyToCruiser);
            _projectiles = new MultiCache<Prefab>(keyToProjectile);
            _shipDeaths = new MultiCache<ShipDeathInitialiser>(keyToDeath);
            _explosions = new MultiCache<ExplosionController>(keyToExplosion);
            _bodykits = new MultiCache<Bodykit>(keyToBodykits);
            _captains = new MultiCache<CaptainExo>(keyToCaptains);
            _variants = new MultiCache<VariantPrefab>(keyToVariants);

            Logging.Log(Tags.PREFAB_CACHE_FACTORY, "After retrieve all prefabs task");
            Drone = DroneContainer.Value;
            AudioSource = AudioSourceContainer.Value;
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
            Logging.Log(Tags.PREFAB_CACHE_FACTORY, "Pre GetPrefabAsync");
            PrefabContainer<TPrefab> prefabContainer = await PrefabFetcher.GetPrefabAsync<TPrefab>(prefabKey);
            Logging.Log(Tags.PREFAB_CACHE_FACTORY, "After GetPrefabAsync");

            prefabContainer.Prefab.StaticInitialise();
            if (!keyToPrefab.ContainsKey(prefabKey))
                keyToPrefab.Add(prefabKey, prefabContainer.Prefab);
        }

        private static async Task GetPrefab<TPrefab>(
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