using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FELIX  Have a nicer way of creating these?  (No hardcoded strings?)  Similar to Resources.resw in UWP?
public class BuildingGroupFactory
{
	public BuildingGroup CreateBuildingGroup(BuildingCategory category, IList<Building> buildings)
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
			case BuildingCategory.Turret:
				return "Turrets";
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
			case BuildingCategory.Turret:
				return "Defensive buildings to protect your cruiser";
			default:
				throw new ArgumentException();
		}
	}
}
