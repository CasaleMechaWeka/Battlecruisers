using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Effects.Deaths;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Projectiles;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.Sound.Pools;

namespace BattleCruisers.Utils.Fetchers.Cache
{
    public class PrefabCache
    {
        private readonly MultiCache<BuildableWrapper<IBuilding>> _buildings;
        private readonly MultiCache<BuildableWrapper<IUnit>> _units;
        private readonly MultiCache<Cruiser> _cruisers;
        private readonly MultiCache<ExplosionController> _explosions;
        private readonly MultiCache<ShipDeathInitialiser> _shipDeaths;
        private readonly MultiCache<CaptainExo> _captains;
        private readonly MultiCache<Bodykit> _bodykits;
        private readonly MultiCache<VariantPrefab> _variants;
        private readonly UntypedMultiCache<Projectile> _projectiles;

        public DroneController Drone { get; }
        public AudioSourceInitialiser AudioSource { get; }

        public PrefabCache(
            MultiCache<BuildableWrapper<IBuilding>> buildings,
            MultiCache<BuildableWrapper<IUnit>> units,
            MultiCache<Cruiser> cruisers,
            MultiCache<ExplosionController> explosions,
            MultiCache<ShipDeathInitialiser> shipDeaths,
            MultiCache<CaptainExo> captains,
            MultiCache<Bodykit> bodykits,
            MultiCache<VariantPrefab> variants,
            UntypedMultiCache<Projectile> projectiles,
            DroneController drone,
            AudioSourceInitialiser audioSource)
        {
            Helper.AssertIsNotNull(buildings, units, cruisers, explosions, shipDeaths, projectiles, drone, audioSource, captains, bodykits, variants);

            _buildings = buildings;
            _units = units;
            _cruisers = cruisers;
            _explosions = explosions;
            _shipDeaths = shipDeaths;
            _captains = captains;
            _projectiles = projectiles;
            _bodykits = bodykits;
            _variants = variants;
            Drone = drone;
            AudioSource = audioSource;
        }

        public BuildableWrapper<IBuilding> GetBuilding(IPrefabKey key)
        {
            return _buildings.GetPrefab(key);
        }

        public BuildableWrapper<IUnit> GetUnit(IPrefabKey key)
        {
            return _units.GetPrefab(key);
        }

        public Cruiser GetCruiser(IPrefabKey key)
        {
            return _cruisers.GetPrefab(key);
        }

        public ExplosionController GetExplosion(IPrefabKey key)
        {
            return _explosions.GetPrefab(key);
        }

        public ShipDeathInitialiser GetShipDeath(IPrefabKey key)
        {
            return _shipDeaths.GetPrefab(key);
        }

        public TProjectile GetProjectile<TProjectile>(IPrefabKey prefabKey) where TProjectile : Projectile
        {
            return _projectiles.GetPrefab<TProjectile>(prefabKey);
        }

        public CaptainExo GetCaptainExo(IPrefabKey key)
        {
            return _captains.GetPrefab(key);
        }

        public Bodykit GetBodykit(IPrefabKey key)
        {
            return _bodykits.GetPrefab(key);
        }

        public VariantPrefab GetVariant(IPrefabKey key)
        {
            return _variants.GetPrefab(key);
        }
    }
}