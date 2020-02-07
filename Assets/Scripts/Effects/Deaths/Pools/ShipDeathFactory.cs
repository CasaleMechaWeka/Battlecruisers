using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;

namespace BattleCruisers.Effects.Deaths.Pools
{
    // FELIX  Test :)
    public class ShipDeathFactory : IPoolableFactory<IShipDeath, Vector3>
    {
        private readonly IPrefabFactory _prefabFactory;
        private readonly ShipDeathKey _shipDeathKey;

        public ShipDeathFactory(IPrefabFactory prefabFactory, ShipDeathKey shipDeathKey)
        {
            Helper.AssertIsNotNull(prefabFactory, shipDeathKey);

            _prefabFactory = prefabFactory;
            _shipDeathKey = shipDeathKey;
        }

        public IShipDeath CreateItem()
        {
            return _prefabFactory.CreateShipDeath(_shipDeathKey);
        }

        public override string ToString()
        {
            return $"{nameof(ShipDeathFactory)} {_shipDeathKey}";
        }
    }
}