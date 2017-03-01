using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingCategory
{
	Factory, Turret, Support, Tactical, AntiCruiser, Experimental
}

public enum TechLevel
{
	T1, T2, T3
}

// FELIX
// - Image?
public interface IBuilding
{
	string Name { get; }
	string Description { get; }
	int NumOfDronesRequired { get; }
	TechLevel TechLevel { get; }
	BuildingCategory Category { get; }
}

// FELIX  Don't need to extend MonoBehaviour?
public class Building : IBuilding
{
	public string Name { get; private set; }
	public string Description { get; private set; }
	public int NumOfDronesRequired { get; private set; }
	public TechLevel TechLevel { get; private set; }
	public BuildingCategory Category { get; private set; }

	public Building(string name, string description, int numOfDronesRequired, TechLevel techLevel, BuildingCategory category)
	{
		Name = name;
		Description = description;
		NumOfDronesRequired = numOfDronesRequired;
		TechLevel = techLevel;
		Category = category;
	}
}
