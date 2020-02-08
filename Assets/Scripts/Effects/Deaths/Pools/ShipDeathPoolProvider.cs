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
        public IPool<IShipDeath, Vector3> ArchonPool { get; }
        public IPool<IShipDeath, Vector3> AttackBoatPool { get; }
        public IPool<IShipDeath, Vector3> FrigatePool { get; }
        public IPool<IShipDeath, Vector3> DestroyerPool { get; }

        public ShipDeathPoolProvider(IPrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);

            AttackBoatPool = CreateShipDeathPool(prefabFactory, StaticPrefabKeys.ShipDeaths.AttackBoat);
            FrigatePool = CreateShipDeathPool(prefabFactory, StaticPrefabKeys.ShipDeaths.Frigate);
            DestroyerPool = CreateShipDeathPool(prefabFactory, StaticPrefabKeys.ShipDeaths.Destroyer);
            ArchonPool = CreateShipDeathPool(prefabFactory, StaticPrefabKeys.ShipDeaths.Archon);
        }

        private IPool<IShipDeath, Vector3> CreateShipDeathPool(IPrefabFactory prefabFactory, ShipDeathKey shipDeathKey)
        {
            return
                new Pool<IShipDeath, Vector3>(
                    new ShipDeathFactory(
                        prefabFactory,
                        shipDeathKey));
        }

        public void SetInitialCapacity()
        {
            AttackBoatPool.AddCapacity(InitialCapacity.ATTACK_BOAT);
            FrigatePool.AddCapacity(InitialCapacity.FRIGATE);
            DestroyerPool.AddCapacity(InitialCapacity.DESTROYFER);
            ArchonPool.AddCapacity(InitialCapacity.ARCHON);
        }
    }
}