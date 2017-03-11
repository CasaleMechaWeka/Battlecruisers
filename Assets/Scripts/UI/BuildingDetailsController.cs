using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDetailsController : MonoBehaviour 
{
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

	}

	// Use this for initialization
	void Start () 
	{
		gameObject.SetActive(false);	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
