using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildingGroup
{
	IList<IBuilding> Buildings { get; }
	BuildingCategory BuildingCategory { get; }
	string Name { get; }
	string Description { get; }
}

// FELIX  Extract common superclass with UnitGroup?
public class BuildingGroup
{
	public IList<IBuilding> Buildings { get; private set; }
	public BuildingCategory BuildingCategory { get; private set; }
	public string Name { get; private set; }
	public string Description { get; private set; }

	public BuildingGroup(IList<IBuilding> buildings, BuildingCategory buildingCategory, string name, string description)
	{
		Buildings = buildings;
		BuildingCategory = buildingCategory;
		Name = name;
		Description = description;
	}
}
