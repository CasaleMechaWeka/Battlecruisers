using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Deaths.Pools
{
    public class ShipDeathPoolProvider : IShipDeathPoolProvider
    {
        public Pool<IPoolable<Vector3>, Vector3> ArchonPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> AttackBoatPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> AttackRIBPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> FrigatePool { get; }
        public Pool<IPoolable<Vector3>, Vector3> DestroyerPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> SiegeDestroyerPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> GlassCannoneerPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> GunBoatPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> TurtlePool { get; }

        public ShipDeathPoolProvider(PrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);

            AttackBoatPool = CreateShipDeathPool(prefabFactory, StaticPrefabKeys.ShipDeaths.AttackBoat);
            AttackRIBPool = CreateShipDeathPool(prefabFactory, StaticPrefabKeys.ShipDeaths.AttackRIB);
            FrigatePool = CreateShipDeathPool(prefabFactory, StaticPrefabKeys.ShipDeaths.Frigate);
            DestroyerPool = CreateShipDeathPool(prefabFactory, StaticPrefabKeys.ShipDeaths.Destroyer);
            SiegeDestroyerPool = CreateShipDeathPool(prefabFactory, StaticPrefabKeys.ShipDeaths.SiegeDestroyer);
            ArchonPool = CreateShipDeathPool(prefabFactory, StaticPrefabKeys.ShipDeaths.Archon);
            GlassCannoneerPool = CreateShipDeathPool(prefabFactory, StaticPrefabKeys.ShipDeaths.GlassCannoneer);
            GunBoatPool = CreateShipDeathPool(prefabFactory, StaticPrefabKeys.ShipDeaths.GunBoat);
            TurtlePool = CreateShipDeathPool(prefabFactory, StaticPrefabKeys.ShipDeaths.Turtle);
        }

        private Pool<IPoolable<Vector3>, Vector3> CreateShipDeathPool(PrefabFactory prefabFactory, ShipDeathKey shipDeathKey)
        {
            return
                new Pool<IPoolable<Vector3>, Vector3>(
                    new ShipDeathFactory(
                        prefabFactory,
                        shipDeathKey));
        }

        public void SetInitialCapacity()
        {
            AttackBoatPool.AddCapacity(InitialCapacity.ATTACK_BOAT);
            AttackRIBPool.AddCapacity(InitialCapacity.ATTACK_RIB);
            FrigatePool.AddCapacity(InitialCapacity.FRIGATE);
            DestroyerPool.AddCapacity(InitialCapacity.DESTROYFER);
            SiegeDestroyerPool.AddCapacity(InitialCapacity.SIEGEDESTROYER);
            ArchonPool.AddCapacity(InitialCapacity.ARCHON);
            GlassCannoneerPool.AddCapacity(InitialCapacity.GLASSCANNONEER);
            GunBoatPool.AddCapacity(InitialCapacity.GUNBOAT);
            TurtlePool.AddCapacity(InitialCapacity.TURTLE);
        }
    }
}