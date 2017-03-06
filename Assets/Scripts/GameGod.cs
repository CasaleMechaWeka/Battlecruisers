using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Initialises everything :D
/// </summary>
public class GameGod : MonoBehaviour 
{
	public BuildMenuController buildMenuController;

	void Awake()
	{
		InitializeBuildMenuController();
	}

	// FELIX  Don't hardcode
	private void InitializeBuildMenuController()
	{
		Building[] factories = 
		{
			new Building("Naval Factory", "Makes ships", 2, TechLevel.T1, BuildingCategory.Factory, SlotType.Platform),
			new Building("Aircraft Factory", "Makes aircraft", 2, TechLevel.T1, BuildingCategory.Factory, SlotType.Bow)
		};
		BuildingGroup factoriesGroup = new BuildingGroup(
			factories, 
			BuildingCategory.Factory,
			"Factories",
			"Buildings that produce units");

		Building[] turrets = 
		{
			new Building("Solid Shooter", "Small turret good against weak ships", 2, TechLevel.T1, BuildingCategory.Turret, SlotType.Deck),
			new Building("Big Bad Blaster", "Strong turret good against all ships", 6, TechLevel.T2, BuildingCategory.Turret, SlotType.Deck)
		};
		BuildingGroup turretsGroup = new BuildingGroup(
			turrets, 
			BuildingCategory.Turret,
			"Turrets",
			"Buildings that defent your cruieser");

		BuildingGroup[] buildingGroups = new BuildingGroup[]
		{
			factoriesGroup, turretsGroup
		};

		buildMenuController.BuildingGroups = buildingGroups;
	}

	// Use this for initialization
	void Start () 
	{
				
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
