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
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Pools;
using System.Runtime.InteropServices;

namespace BattleCruisers.Utils.Fetchers.Cache
{
    public class PrefabCache : IPrefabCache
    {
        private readonly IMultiCache<BuildableWrapper<IBuilding>> _buildings;
        private readonly IMultiCache<BuildableWrapper<IUnit>> _units;
        private readonly IMultiCache<Cruiser> _cruisers;
        private readonly IMultiCache<ExplosionController> _explosions;
        private readonly IMultiCache<ShipDeathInitialiser> _shipDeaths;
        private readonly IMultiCache<CaptainExo> _captains;
        private readonly IUntypedMultiCache<Projectile> _projectiles;

        public DroneController Drone { get; }
        public AudioSourceInitialiser AudioSource { get; }

        public PrefabCache(
            IMultiCache<BuildableWrapper<IBuilding>> buildings, 
            IMultiCache<BuildableWrapper<IUnit>> units, 
            IMultiCache<Cruiser> cruisers, 
            IMultiCache<ExplosionController> explosions, 
            IMultiCache<ShipDeathInitialiser> shipDeaths,
            IMultiCache<CaptainExo> captains,
            IUntypedMultiCache<Projectile> projectiles, 
            DroneController drone,
            AudioSourceInitialiser audioSource)
        {
            Helper.AssertIsNotNull(buildings, units, cruisers, explosions, shipDeaths, projectiles, drone, audioSource, captains);

            _buildings = buildings;
            _units = units;
            _cruisers = cruisers;
            _explosions = explosions;
            _shipDeaths = shipDeaths;
            _captains = captains;
            _projectiles = projectiles;
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
    }
}