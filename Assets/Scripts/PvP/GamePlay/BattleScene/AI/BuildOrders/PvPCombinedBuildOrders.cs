using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders
{
    public class PvPCombinedBuildOrders : IPvPDynamicBuildOrder
    {
        private readonly IList<IPvPDynamicBuildOrder> _buildOrders;

        public PvPBuildingKey Current { get; private set; }

        public PvPCombinedBuildOrders(IList<IPvPDynamicBuildOrder> buildOrders)
        {
            Assert.IsNotNull(buildOrders);
            Assert.IsTrue(buildOrders.Count > 0);

            _buildOrders = buildOrders;
        }

        public bool MoveNext()
        {
            IPvPDynamicBuildOrder validBuildOrder = _buildOrders.FirstOrDefault(buildOrder => buildOrder.MoveNext());
            Current = validBuildOrder?.Current;
            return Current != null;
        }
    }
}
