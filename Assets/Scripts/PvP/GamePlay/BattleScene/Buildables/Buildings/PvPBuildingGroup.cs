using System;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings
{
    public class PvPBuildingGroup : IPvPBuildingGroup
    {
        public IList<IPvPBuildableWrapper<IPvPBuilding>> Buildings { get; }
        public PvPBuildingCategory BuildingCategory { get; }
        public string BuildingGroupName { get; }
        public string Description { get; }

        public PvPBuildingGroup(
            PvPBuildingCategory buildingCategory,
            IList<IPvPBuildableWrapper<IPvPBuilding>> buildings,
            string groupName,
            string description)
        {
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
