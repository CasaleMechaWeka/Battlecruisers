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
// - Support building taking >1 slot?
//public interface Building
//{
//	string Name { get; }
//	string Description { get; }
//	int NumOfDronesRequired { get; }
//	BuildingCategory Category { get; }
//	SlotType SlotType { get; }
//}

// FELIX  Don't need to extend MonoBehaviour?
public class Building : MonoBehaviour
{
	public string Name { get; private set; }
	public string Description { get; private set; }
	public int NumOfDronesRequired { get; private set; }
	public BuildingCategory Category { get; private set; }
	public SlotType SlotType { get; private set; }

	public Building(string name, string description, int numOfDronesRequired, BuildingCategory category, SlotType slotType)
	{
		Name = name;
		Description = description;
		NumOfDronesRequired = numOfDronesRequired;
		Category = category;
		SlotType = slotType;
	}
}
