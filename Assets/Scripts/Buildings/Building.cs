using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingCategory
{
	Factory, Turret, Support, Tactical, AntiCruiser, Experimental
}

public class Building : MonoBehaviour
{
	public string name;
	public string description;
	public int numOfDronesRequired;
	public BuildingCategory category;
	public SlotType slotType;
}
