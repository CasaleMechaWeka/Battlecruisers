using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings
{
    public interface IPvPBuildingGroup
    {
        IList<IPvPBuildableWrapper<IPvPBuilding>> Buildings { get; }
        BuildingCategory BuildingCategory { get; }
        string BuildingGroupName { get; }
        string Description { get; }
    }
}
