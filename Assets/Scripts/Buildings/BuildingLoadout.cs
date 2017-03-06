using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingLoadout : MonoBehaviour 
{
	public BuildingGroup[] BuildingGroups { get; private set; }

	// User needs to be able to build at least one building
	private const int MIN_NUM_OF_BUILDING_GROUPS = 1;
	// Currently only support 6 types of buildings, so the UI is optimsed for this.  Ie, there is no space for more!
	private const int MAX_NUM_OF_BUILDING_GROUPS = 6;

	void Awake()
	{
		BuildingGroups = GetComponentsInChildren<BuildingGroup>();
		Debug.Log($"BuildingGroups.Length: {BuildingGroups.Length}");

		if (BuildingGroups.Length < MIN_NUM_OF_BUILDING_GROUPS
			|| BuildingGroups.Length > MAX_NUM_OF_BUILDING_GROUPS)
		{
			throw new InvalidProgramException();
		}
	}
}
