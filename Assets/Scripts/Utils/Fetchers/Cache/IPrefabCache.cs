using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Utils.Timers;

namespace BattleCruisers.Utils.Fetchers.Cache
{
    /// FELIX  Implement, use, test
    public interface IPrefabCache
    {
        // Multiple prefab caches
        BuildableWrapper<IBuilding> GetBuilding(IPrefabKey key);
        BuildableWrapper<IUnit> GetUnit(IPrefabKey key);
        Cruiser GetCruiser(IPrefabKey key);
        ExplosionController GetExplosion(IPrefabKey key);

        // Single prefab caches
        // FELIX  Convert to properties :)
        CountdownController GetCountdown();
        DroneController GetDrone();

        // Multiple untyped prefab caches
        TProjectile GetProjectile<TProjectile>(IPrefabKey prefabKey);
    }
}