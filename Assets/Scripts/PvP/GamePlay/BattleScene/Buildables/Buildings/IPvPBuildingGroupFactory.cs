using BattleCruisers.Buildables.Buildings;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings
{
    public interface IPvPBuildingGroupFactory
    {
        IPvPBuildingGroup CreateBuildingGroup(BuildingCategory category, IList<IPvPBuildableWrapper<IPvPBuilding>> buildings);
    }
}
