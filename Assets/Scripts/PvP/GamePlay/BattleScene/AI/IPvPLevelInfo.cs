using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI
{
    public interface IPvPLevelInfo
    {
        IPvPCruiserController AICruiser { get; }
        IPvPCruiserController PlayerCruiser { get; }

        bool CanConstructBuilding(PvPBuildingKey buildingKey);
        IList<PvPBuildingKey> GetAvailableBuildings(PvPBuildingCategory category);
    }
}