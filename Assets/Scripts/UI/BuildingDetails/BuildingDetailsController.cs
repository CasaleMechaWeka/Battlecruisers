using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDetailsController : MonoBehaviour 
{
	// FELIX  Inject?
	private SpriteFetcher _spriteFetcher;

	public BuildingStatsController statsController;
	public Text buildingName;
	public Text buildingDescription;
	public Image buildingImage;
	public Image slotImage;

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
		buildingName.text = building.buildingName;
		buildingDescription.text = building.description;
		buildingImage.sprite = building.BuildingSprite;
		slotImage.sprite = _spriteFetcher.GetSlotSprite(building.slotType);
	}

	// Use this for initialization
	void Start () 
	{
		_spriteFetcher = new SpriteFetcher();

		// FELIX  TEMP
		gameObject.SetActive(false);	
	}
}
