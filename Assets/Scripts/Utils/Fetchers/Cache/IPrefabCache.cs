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
    public interface IPrefabCache
    {
        // Single prefab caches
        CountdownController Countdown { get; }
        DroneController Drone { get; }

        // Multiple prefab caches
        BuildableWrapper<IBuilding> GetBuilding(IPrefabKey key);
        BuildableWrapper<IUnit> GetUnit(IPrefabKey key);
        Cruiser GetCruiser(IPrefabKey key);
        ExplosionController GetExplosion(IPrefabKey key);

        // Multiple untyped prefab caches
        TProjectile GetProjectile<TProjectile>(IPrefabKey prefabKey) where TProjectile : Projectile;
    }
}