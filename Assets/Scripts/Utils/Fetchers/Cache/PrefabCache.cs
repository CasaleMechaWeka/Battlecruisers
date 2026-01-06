using System;
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

        public static bool IsInitialized => _units != null && _buildings != null && _cruisers != null;

        public static BuildableWrapper<IBuilding> GetBuilding(IPrefabKey key)
        {
            if (_buildings == null)
                throw new System.InvalidOperationException("PrefabCache has not been initialized. Call PrefabCache.CreatePrefabCacheAsync() first and await it.");
            return _buildings.GetPrefab(key);
        }
        
        public static BuildableWrapper<IUnit> GetUnit(IPrefabKey key)
        {
            if (_units == null)
                throw new System.InvalidOperationException("PrefabCache has not been initialized. Call PrefabCache.CreatePrefabCacheAsync() first and await it.");
            return _units.GetPrefab(key);
        }
        
        public static Cruiser GetCruiser(IPrefabKey key)
        {
            if (_cruisers == null)
                throw new System.InvalidOperationException("PrefabCache has not been initialized. Call PrefabCache.CreatePrefabCacheAsync() first and await it.");
            return _cruisers.GetPrefab(key);
        }
        public static ExplosionController GetExplosion(IPrefabKey key) => _explosions.GetPrefab(key);
        public static ShipDeathInitialiser GetShipDeath(IPrefabKey key) => _shipDeaths.GetPrefab(key);
        public static CaptainExo GetCaptainExo(IPrefabKey key) => _captains.GetPrefab(key);
        public static Bodykit GetBodykit(IPrefabKey key) => _bodykits.GetPrefab(key);
        public static VariantPrefab GetVariant(IPrefabKey key) => _variants.GetPrefab(key);
        public static Prefab GetProjectile(IPrefabKey key) => _projectiles.GetPrefab(key);
        public static DroneController Drone { get; private set; }

        public static async Task CreatePrefabCacheAsync()
        {
            IDictionary<IPrefabKey, VariantPrefab> keysToVariants               = new ConcurrentDictionary<IPrefabKey, VariantPrefab>();
            IDictionary<IPrefabKey, BuildableWrapper<IBuilding>> keysToBuilding = new ConcurrentDictionary<IPrefabKey, BuildableWrapper<IBuilding>>();
            IDictionary<IPrefabKey, CaptainExo> keysToCaptains                  = new ConcurrentDictionary<IPrefabKey, CaptainExo>();
            IDictionary<IPrefabKey, Cruiser> keysToCruiser                      = new ConcurrentDictionary<IPrefabKey, Cruiser>();
            IDictionary<IPrefabKey, Bodykit> keysToBodykits                     = new ConcurrentDictionary<IPrefabKey, Bodykit>();
            IDictionary<IPrefabKey, ShipDeathInitialiser> keysToShipDeaths      = new ConcurrentDictionary<IPrefabKey, ShipDeathInitialiser>();
            IDictionary<IPrefabKey, BuildableWrapper<IUnit>> keysToUnits        = new ConcurrentDictionary<IPrefabKey, BuildableWrapper<IUnit>>();
            IDictionary<IPrefabKey, Prefab> keysToProjectile                    = new ConcurrentDictionary<IPrefabKey, Prefab>();
            IDictionary<IPrefabKey, ExplosionController> keysToExplosion        = new ConcurrentDictionary<IPrefabKey, ExplosionController>();
            Container<DroneController> DroneContainer                           = new Container<DroneController>();

            IList<Task> retrievePrefabsTasks = new List<Task>
            {
                GetPrefabs(StaticPrefabKeys.Variants.AllKeys, keysToVariants),
                GetPrefabs(StaticPrefabKeys.Buildings.AllKeys, keysToBuilding),
                GetPrefabs(StaticPrefabKeys.CaptainExos.AllKeys, keysToCaptains),
                GetPrefabs(StaticPrefabKeys.Hulls.AllKeys, keysToCruiser),
                GetPrefabs(StaticPrefabKeys.BodyKits.AllKeys, keysToBodykits),
                GetPrefabs(StaticPrefabKeys.ShipDeaths.AllKeys, keysToShipDeaths),
                GetPrefabs(StaticPrefabKeys.Units.AllKeys, keysToUnits),
                GetPrefabs(StaticPrefabKeys.Projectiles.AllKeys, keysToProjectile),
                GetPrefabs(StaticPrefabKeys.Explosions.AllKeys, keysToExplosion),
                GetPrefab(StaticPrefabKeys.Effects.BuilderDrone, DroneContainer)
            };

            Logging.Log(Tags.PREFAB_CACHE_FACTORY, "Pre retrieve all prefabs task");
            await Task.WhenAll(retrievePrefabsTasks);

            _buildings = new MultiCache<BuildableWrapper<IBuilding>>(keysToBuilding);
            _units = new MultiCache<BuildableWrapper<IUnit>>(keysToUnits);
            _units = new MultiCache<BuildableWrapper<IUnit>>(keysToUnits);
            _cruisers = new MultiCache<Cruiser>(keysToCruiser);
            _projectiles = new MultiCache<Prefab>(keysToProjectile);
            _shipDeaths = new MultiCache<ShipDeathInitialiser>(keysToShipDeaths);
            _explosions = new MultiCache<ExplosionController>(keysToExplosion);
            _bodykits = new MultiCache<Bodykit>(keysToBodykits);
            _captains = new MultiCache<CaptainExo>(keysToCaptains);
            _variants = new MultiCache<VariantPrefab>(keysToVariants);

            Logging.Log(Tags.PREFAB_CACHE_FACTORY, "After retrieve all prefabs task");
            Drone = DroneContainer.Value;
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

            try
            {
                Logging.Log(Tags.PREFAB_CACHE_FACTORY, $"Initializing prefab: {prefabKey}");
                prefabContainer.Prefab.StaticInitialise();
                Logging.Log(Tags.PREFAB_CACHE_FACTORY, $"Successfully initialized prefab: {prefabKey}");
            }
            catch (Exception ex)
            {
                Logging.Log(Tags.PREFAB_CACHE_FACTORY, $"FAILED to initialize prefab: {prefabKey}. Error: {ex.Message}\nStackTrace: {ex.StackTrace}");
                throw; // Re-throw to maintain original error behavior
            }
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