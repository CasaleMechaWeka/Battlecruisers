using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders
{
    public class PvPInfiniteStaticBuildOrder : IPvPDynamicBuildOrder
    {
        public PvPBuildingKey Current { get; }

        public PvPInfiniteStaticBuildOrder(PvPBuildingKey buildingKey)
        {
            Current = buildingKey;
        }

        public bool MoveNext()
        {
            return true;
        }
    }
}