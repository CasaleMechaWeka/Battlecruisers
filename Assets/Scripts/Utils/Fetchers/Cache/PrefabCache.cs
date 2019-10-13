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
    public class PrefabCache : IPrefabCache
    {
        private readonly IMultiCache<BuildableWrapper<IBuilding>> _buildings;
        private readonly IMultiCache<BuildableWrapper<IUnit>> _units;
        private readonly IMultiCache<Cruiser> _cruisers;
        private readonly IMultiCache<Explosion> _explosiosn;
        // FELIX  Projectiles :/
        //private readonly IMultiCache<;

        public CountdownController Countdown { get; }
        public DroneController Drone { get; }

        public BuildableWrapper<IBuilding> GetBuilding(IPrefabKey key)
        {
            throw new System.NotImplementedException();
        }

        public BuildableWrapper<IUnit> GetUnit(IPrefabKey key)
        {
            throw new System.NotImplementedException();
        }

        public Cruiser GetCruiser(IPrefabKey key)
        {
            throw new System.NotImplementedException();
        }

        public ExplosionController GetExplosion(IPrefabKey key)
        {
            throw new System.NotImplementedException();
        }

        public TProjectile GetProjectile<TProjectile>(IPrefabKey prefabKey)
        {
            throw new System.NotImplementedException();
        }
    }
}