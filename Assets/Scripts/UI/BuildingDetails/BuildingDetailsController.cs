using BattleCruisers.Buildings;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BuildingDetails
{
	public class BuildingDetailsController : MonoBehaviour 
	{
		private Building _building;
		private bool _allowDelete;
		// FELIX  Inject?
		private SpriteFetcher _spriteFetcher;

		public BuildingStatsController statsController;
		public Text buildingName;
		public Text buildingDescription;
		public Image buildingImage;
		public Image slotImage;
		public Button deleteBuildingButton;

		// Use this for initialization
		void Start () 
		{
			_spriteFetcher = new SpriteFetcher();
			_allowDelete = false;
			Hide();
		}

		public void ShowBuildingDetails(Building building, bool allowDelete)
		{
			Assert.IsNotNull(building);

			_building = building;
			_allowDelete = allowDelete;
			gameObject.SetActive(true);

			statsController.ShowBuildingStats(_building);
			buildingName.text = _building.buildableName;
			buildingDescription.text = _building.description;
			buildingImage.sprite = _building.Sprite;
			slotImage.sprite = _spriteFetcher.GetSlotSprite(_building.slotType);

			deleteBuildingButton.gameObject.SetActive(allowDelete);
			if (allowDelete)
			{
				deleteBuildingButton.onClick.AddListener(DeleteBuilding);
			}
		}

		public void DeleteBuilding()
		{
			Assert.IsTrue(_allowDelete);
			Assert.IsNotNull(_building);

			_building.InitiateDelete();
			Hide();
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}
