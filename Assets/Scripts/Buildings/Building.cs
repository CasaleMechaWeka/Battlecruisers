using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	public Vector3 Size { get { return _renderer.bounds.size; } }

	void Awake()
	{
		_renderer = GetComponent<Renderer>();
		_renderer.enabled = false;
	}

	public void ShowBuilding()
	{
		_renderer.enabled = true;

		// FELIX  Position!
	}
}
