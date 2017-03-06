using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingCategory
{
	Factory, Turret, Support, Tactical, AntiCruiser, Experimental
}

public class Building : MonoBehaviour
{
	private Renderer _renderer;

	public string buildingName;
	public string description;
	public int numOfDronesRequired;
	public BuildingCategory category;
	public SlotType slotType;

	void Awake()
	{
		_renderer = GetComponent<Renderer>();
		_renderer.enabled = false;
	}
}
