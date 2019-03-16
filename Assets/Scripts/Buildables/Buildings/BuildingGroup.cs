using System;
using System.Collections.Generic;

namespace BattleCruisers.Buildables.Buildings
{
    public class BuildingGroup : IBuildingGroup
	{
        public IList<IBuildableWrapper<IBuilding>> Buildings { get; }
		public BuildingCategory BuildingCategory { get; }
		public string BuildingGroupName { get; }
		public string Description { get; }

		private int MIN_NUM_OF_BUILDINGS = 0;
		private int MAX_NUM_OF_BUILDINGS = 5;

		public BuildingGroup(
            BuildingCategory buildingCategory,
			IList<IBuildableWrapper<IBuilding>> buildings,
			string groupName,
			string description)
		{
			if (buildings.Count < MIN_NUM_OF_BUILDINGS || buildings.Count > MAX_NUM_OF_BUILDINGS)
			{
				throw new ArgumentException("Invalid building count: " + buildings.Count);
			}

            BuildingCategory = buildingCategory;
			Buildings = buildings;
			BuildingGroupName = groupName;
			Description = description;

			// Check building category matches this group's category
			for (int i = 1; i < buildings.Count; ++i)
			{
				if (buildings[i].Buildable.Category != BuildingCategory)
				{
					throw new ArgumentException();
				}
			}
		}
	}
}
