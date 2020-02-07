using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Deaths.Pools
{
    // FELIX  use, add other ships :)
    public class ShipDeathPoolProvider : IShipDeathPoolProvider
    {
        public IPool<IShipDeath, Vector3> ArchonPool { get; }

        public ShipDeathPoolProvider(IPrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);

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
            ArchonPool.AddCapacity(InitialCapacity.ARCHON);
        }
    }
}