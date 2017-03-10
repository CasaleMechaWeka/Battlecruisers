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
		IList<BuildingKey> buildingKeys = new List<BuildingKey>();

		// Factories
		buildingKeys.Add(new BuildingKey(BuildingCategory.Factory, "AirFactory"));
		buildingKeys.Add(new BuildingKey(BuildingCategory.Factory, "EngineeringBay"));

		// Turrets
		buildingKeys.Add(new BuildingKey(BuildingCategory.Tactical, "Shield"));

		// Tactical
		// FELIX

		PrefabFetcher prefabFetcher = new PrefabFetcher();

		BuildingLoadout buildingLoadout = new BuildingLoadout(prefabFetcher, buildingKeys);
		buildMenuController.BuildingGroups = buildingLoadout.BuildingGroups;
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
