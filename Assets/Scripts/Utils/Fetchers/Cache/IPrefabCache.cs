using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Effects.Deaths;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Projectiles;
using BattleCruisers.UI.Sound.Pools;

namespace BattleCruisers.Utils.Fetchers.Cache
{
    public interface IPrefabCache
    {
        // Single prefab caches
        DroneController Drone { get; }
        AudioSourceInitialiser AudioSource { get; }

        // Multiple prefab caches
        BuildableWrapper<IBuilding> GetBuilding(IPrefabKey key);
        BuildableWrapper<IUnit> GetUnit(IPrefabKey key);
        Cruiser GetCruiser(IPrefabKey key);
        ExplosionController GetExplosion(IPrefabKey key);
        ShipDeathInitialiser GetShipDeath(IPrefabKey key);

        // Multiple untyped prefab caches
        TProjectile GetProjectile<TProjectile>(IPrefabKey prefabKey) where TProjectile : Projectile;
    }
}