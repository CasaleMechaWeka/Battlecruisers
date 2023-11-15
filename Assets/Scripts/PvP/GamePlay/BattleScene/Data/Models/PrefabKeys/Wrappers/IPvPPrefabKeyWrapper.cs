using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys.Wrappers
{
    public interface IPvPPrefabKeyWrapper
    {
        bool HasKey { get; }
        PvPBuildingKey Key { get; }

        void Initialise(IPvPBuildOrders buildOrders);
    }
}
