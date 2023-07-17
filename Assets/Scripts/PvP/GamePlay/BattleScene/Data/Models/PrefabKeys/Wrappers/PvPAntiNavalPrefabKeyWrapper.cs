using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys.Wrappers
{
    public class PvPAntiNavalPrefabKeyWrapper : PvPBasePrefabKeyWrapper
    {
        protected override IPvPDynamicBuildOrder GetBuildOrder(IPvPBuildOrders buildOrders)
        {
            return buildOrders.AntiNavalBuildOrder;
        }
    }
}
