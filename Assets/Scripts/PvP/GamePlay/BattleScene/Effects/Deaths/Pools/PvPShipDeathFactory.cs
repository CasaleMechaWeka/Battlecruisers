using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths.Pools
{
    public class PvPShipDeathFactory : IPvPPoolableFactory<IPvPShipDeath, Vector3>
    {
        private readonly IPvPPrefabFactory _prefabFactory;
        private readonly PvPShipDeathKey _shipDeathKey;

        public PvPShipDeathFactory(IPvPPrefabFactory prefabFactory, PvPShipDeathKey shipDeathKey)
        {
            PvPHelper.AssertIsNotNull(prefabFactory, shipDeathKey);

            _prefabFactory = prefabFactory;
            _shipDeathKey = shipDeathKey;
        }

        public IPvPShipDeath CreateItem()
        {
            return _prefabFactory.CreateShipDeath(_shipDeathKey);
        }

        public override string ToString()
        {
            return $"{nameof(PvPShipDeathFactory)} {_shipDeathKey}";
        }
    }
}