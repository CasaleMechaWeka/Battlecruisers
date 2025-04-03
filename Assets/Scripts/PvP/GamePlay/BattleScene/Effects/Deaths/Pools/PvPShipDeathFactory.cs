using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths.Pools
{
    public class PvPShipDeathFactory : IPoolableFactory<IPoolable<Vector3>, Vector3>
    {
        private readonly PvPShipDeathKey _shipDeathKey;

        public PvPShipDeathFactory(PvPShipDeathKey shipDeathKey)
        {
            PvPHelper.AssertIsNotNull(shipDeathKey);

            _shipDeathKey = shipDeathKey;
        }

        public IPoolable<Vector3> CreateItem()
        {
            return PvPPrefabFactory.CreateShipDeath(_shipDeathKey);
        }

        public override string ToString()
        {
            return $"{nameof(PvPShipDeathFactory)} {_shipDeathKey}";
        }
    }
}