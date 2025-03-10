using System.Collections.Generic;

namespace BattleCruisers.Buildables.Buildings
{
    public interface IBuildingGroup
	{
		IList<IBuildableWrapper<IBuilding>> Buildings { get; }
		BuildingCategory BuildingCategory { get; }
		string BuildingGroupName { get; }
		string Description { get; }
	}
}
