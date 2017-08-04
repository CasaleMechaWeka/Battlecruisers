using System.Collections.Generic;

namespace BattleCruisers.Buildables.Buildings
{
    public interface IBuildingGroupFactory
    {
        IBuildingGroup CreateBuildingGroup(BuildingCategory category, IList<IBuildableWrapper<IBuilding>> buildings);
    }
}
