using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings
{
    public interface IPvPBuildingGroup
    {
        IList<IPvPBuildableWrapper<IPvPBuilding>> Buildings { get; }
        PvPBuildingCategory BuildingCategory { get; }
        string BuildingGroupName { get; }
        string Description { get; }
    }
}
