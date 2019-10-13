using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Projectiles;
using BattleCruisers.Utils.Timers;

namespace BattleCruisers.Utils.Fetchers.Cache
{
    public class PrefabCache : IPrefabCache
    {
        private readonly IMultiCache<BuildableWrapper<IBuilding>> _buildings;
        private readonly IMultiCache<BuildableWrapper<IUnit>> _units;
        private readonly IMultiCache<Cruiser> _cruisers;
        private readonly IMultiCache<ExplosionController> _explosions;
        private readonly IUntypedMultiCache<Projectile> _projectiles;

        public CountdownController Countdown { get; }
        public DroneController Drone { get; }

        public PrefabCache(
            IMultiCache<BuildableWrapper<IBuilding>> buildings, 
            IMultiCache<BuildableWrapper<IUnit>> units, 
            IMultiCache<Cruiser> cruisers, 
            IMultiCache<ExplosionController> explosions, 
            IUntypedMultiCache<Projectile> projectiles, 
            CountdownController countdown, 
            DroneController drone)
        {
            Helper.AssertIsNotNull(buildings, units, cruisers, explosions, projectiles, countdown, drone);

            _buildings = buildings;
            _units = units;
            _cruisers = cruisers;
            _explosions = explosions;
            _projectiles = projectiles;
            Countdown = countdown;
            Drone = drone;
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

        public TProjectile GetProjectile<TProjectile>(IPrefabKey prefabKey) where TProjectile : Projectile
        {
            return _projectiles.GetPrefab<TProjectile>(prefabKey);
        }
    }
}