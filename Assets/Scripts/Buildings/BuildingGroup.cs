using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// FELIX  Extract common superclass with UnitGroup?
public class BuildingGroup : MonoBehaviour
{
	public Building[] Buildings { get; private set; }
	public BuildingCategory buildingCategory;
	public string buildingName;
	public string description;

	private int MAX_NUM_OF_BUILDINGS = 5;

	void Awake()
	{
		Buildings = GetComponentsInChildren<Building>();
		Debug.Log($"Buildings.Length: {Buildings.Length}");

		if (Buildings.Length > MAX_NUM_OF_BUILDINGS)
		{
			throw new InvalidProgramException();
		}

		// FELIX  Throw exc if 0 buildings?
		if (Buildings.Length > 0)
		{
			buildingCategory = Buildings[0].category;

			for (int i = 1; i < Buildings.Length; ++i)
			{
				if (Buildings[i].category != buildingCategory)
				{
					throw new InvalidProgramException();
				}
			}
		}
	}
}
