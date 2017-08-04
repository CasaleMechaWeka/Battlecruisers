using System;
using System.Collections.Generic;

namespace BattleCruisers.Buildables.Buildings
{
    public class BuildingGroupFactory : IBuildingGroupFactory
	{
		public IBuildingGroup CreateBuildingGroup(BuildingCategory category, IList<IBuildableWrapper<IBuilding>> buildings)
		{
			return new BuildingGroup(buildings, GetGroupName(category), GetGroupDescription(category));
		}

		private string GetGroupName(BuildingCategory category)
		{
			switch (category)
			{
				case BuildingCategory.Factory:
					return "Factories";
				case BuildingCategory.Tactical:
					return "Tactical";
				case BuildingCategory.Defence:
					return "Defence";
				case BuildingCategory.Offence:
					return "Offence";
				case BuildingCategory.Ultra:
					return "Ultras";
				default:
					throw new ArgumentException();
			}
		}

		private string GetGroupDescription(BuildingCategory category)
		{
			switch (category)
			{
				case BuildingCategory.Factory:
					return "Buildings that produce units";
				case BuildingCategory.Tactical:
					return "Specialised buildings";
				case BuildingCategory.Defence:
					return "Defensive buildings to protect your cruiser";
				case BuildingCategory.Offence:
					return "Offensive buildings to destroy the enemy cruiser";
				case BuildingCategory.Ultra:
					return "Ridiculously awesome creations meant to end to game";
				default:
					throw new ArgumentException();
			}
		}
	}
}
