using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using BattleCruisers.Drones;

namespace BattleCruisers.UI.BuildingDetails
{
	public class BuildableDetailsController : MonoBehaviour 
	{
		private IDroneManager _droneManager;
		private ISpriteFetcher _spriteFetcher;
		private Buildable _buildable;
		private bool _allowDelete;
		
		public BuildableStatsController statsController;
		public Text buildableName;
		public Text buildableDescription;
		public Image buildableImage;
		public Image slotImage;
		public Button deleteButton;
		public Button toggleDroneButton;

		void Start () 
		{
			_spriteFetcher = new SpriteFetcher();
			_allowDelete = false;
			Hide();
		}

		public void Initialise(IDroneManager droneManager, ISpriteFetcher spriteFetcher)
		{
			_droneManager = droneManager;
			_spriteFetcher = spriteFetcher;
		}

		public void ShowBuildableDetails(Buildable buildable, bool allowDelete)
		{
			Assert.IsNotNull(buildable);

			if (_buildable != null)
			{
				CleanUp();
			}

			_buildable = buildable;
			_allowDelete = allowDelete;
			gameObject.SetActive(true);

			statsController.ShowBuildableStats(_buildable);
			buildableName.text = _buildable.buildableName;
			buildableDescription.text = _buildable.description;
			buildableImage.sprite = _buildable.Sprite;

			bool hasSlot = _buildable.slotType != SlotType.None;
			if (hasSlot)
			{
				slotImage.sprite = _spriteFetcher.GetSlotSprite((SlotType)_buildable.slotType);
			}
			slotImage.gameObject.SetActive(hasSlot);

			// Delete buildable button
			deleteButton.gameObject.SetActive(allowDelete);
			if (allowDelete)
			{
				deleteButton.onClick.AddListener(DeleteBuildable);
			}

			// Toggle drone button
			bool showDroneToggleButton = buildable.DroneConsumer != null;
			toggleDroneButton.gameObject.SetActive(showDroneToggleButton);
			if (showDroneToggleButton)
			{
				toggleDroneButton.onClick.AddListener(ToggleBuildableDrones);
			}
		}

		public void DeleteBuildable()
		{
			Assert.IsTrue(_allowDelete);
			Assert.IsNotNull(_buildable);

			_buildable.InitiateDelete();
			Hide();
		}

		public void ToggleBuildableDrones()
		{
			_droneManager.ToggleDroneConsumerFocus(_buildable.DroneConsumer);
		}

		public void Hide()
		{
			CleanUp();
			gameObject.SetActive(false);
		}

		private void CleanUp()
		{
			deleteButton.onClick.RemoveAllListeners();
			toggleDroneButton.onClick.RemoveAllListeners();
		}
	}
}
