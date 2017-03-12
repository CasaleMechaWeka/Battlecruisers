using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDetailsController : MonoBehaviour 
{
	public BuildingStatsController statsController;

	// FELIX  Change to:  ShowBuilding()
	private Building _building;
	public Building Building
	{
		set
		{
			if (value != _building)
			{
				_building = value;

				if (_building == null)
				{
					gameObject.SetActive(false);
				}
				else
				{
					gameObject.SetActive(true);
					Populate(_building);
				}
			}
		}
	}

	private void Populate(Building building)
	{
		statsController.ShowBuildingStats(building);
	}

	// Use this for initialization
	void Start () 
	{
		// FELIX  TEMP
		gameObject.SetActive(false);	
	}
}
