using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;

namespace BattleCruisers.Effects.Deaths.Pools
{
    public class ShipDeathFactory : IPoolableFactory<IPoolable<Vector3>, Vector3>
    {
        private readonly ShipDeathKey _shipDeathKey;

        public ShipDeathFactory(ShipDeathKey shipDeathKey)
        {
            Helper.AssertIsNotNull(shipDeathKey);

            _shipDeathKey = shipDeathKey;
        }

        public IPoolable<Vector3> CreateItem()
        {
            return PrefabFactory.CreateShipDeath(_shipDeathKey);
        }

        public override string ToString()
        {
            return $"{nameof(ShipDeathFactory)} {_shipDeathKey}";
        }
    }
}