using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings
{
    public interface IPvPBuildingGroupFactory
    {
        IPvPBuildingGroup CreateBuildingGroup(PvPBuildingCategory category, IList<IPvPBuildableWrapper<IPvPBuilding>> buildings);
    }
}
