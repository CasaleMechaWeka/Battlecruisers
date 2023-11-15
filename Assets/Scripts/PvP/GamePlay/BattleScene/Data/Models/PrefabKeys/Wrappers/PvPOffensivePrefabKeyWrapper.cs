using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys.Wrappers
{
    public class PvPOffensivePrefabKeyWrapper : PvPBasePrefabKeyWrapper
    {
        protected override IPvPDynamicBuildOrder GetBuildOrder(IPvPBuildOrders buildOrders)
        {
            return buildOrders.OffensiveBuildOrder;
        }
    }
}
