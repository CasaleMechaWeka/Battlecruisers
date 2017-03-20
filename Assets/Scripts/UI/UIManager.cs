using BattleCruisers.Cruisers;
using BattleCruisers.Buildings;
using BattleCruisers.Buildings.Factories;
using BattleCruisers.UI;
using BattleCruisers.UI.BuildingDetails;
using BattleCruisers.UI.BuildMenus;
using BattleCruisers.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI
{
	public class UIManager : MonoBehaviour 
	{
		public CameraController cameraController;
		public BackgroundController backgroundController;
		public BuildMenuController buildMenuController;
		public HealthBarController friendlyCruiserHealthBar;
		public HealthBarController enemyCruiserHealthBar;
		public Cruiser friendlyCruiser;
		public BuildableDetailsController buildableDetails;

		public Building SelectedBuilding { get; private set; }

		// Use this for initialization
		void Start () 
		{
			friendlyCruiserHealthBar.gameObject.SetActive(true);
			enemyCruiserHealthBar.gameObject.SetActive(false);

			cameraController.CameraTransitionStarted += OnCameraTransitionStarted;
			cameraController.CameraTransitionCompleted += OnCameraTransitionCompleted;
			backgroundController.BackgroundClicked += OnBackgroundClicked;
		}

		private void OnCameraTransitionStarted(object sender, CameraTransitionArgs e)
		{
			switch (e.Origin)
			{
				case CameraState.FriendlyCruiser:
					buildMenuController.HideBuildMenu();
					buildableDetails.Hide();
					friendlyCruiserHealthBar.gameObject.SetActive(false);
					break;

				case CameraState.EnemyCruiser:
					enemyCruiserHealthBar.gameObject.SetActive(false);
					break;
			}
		}

		private void OnCameraTransitionCompleted(object sender, CameraTransitionArgs e)
		{
			switch (e.Destination)
			{
				case CameraState.FriendlyCruiser:
					buildMenuController.ShowBuildMenu();
					friendlyCruiserHealthBar.gameObject.SetActive(true);
					break;

				case CameraState.EnemyCruiser:
					enemyCruiserHealthBar.gameObject.SetActive(true);
					break;
			}
		}

		private void OnBackgroundClicked(object sender, EventArgs e)
		{
			buildableDetails.Hide();
			friendlyCruiser.UnhighlightSlots();
		}

		public void ShowBuildingGroups()
		{
			Debug.Log("UIManager.ShowBuildingGroups()");
			friendlyCruiser.UnhighlightSlots();
			friendlyCruiser.HideAllSlots();
			buildableDetails.Hide();
			buildMenuController.ShowBuildingGroupsMenu();
		}

		public void SelectBuildingGroup(BuildingCategory buildingCategory)
		{
			Debug.Log("UIManager.SelectBuildingGroup()");
			friendlyCruiser.ShowAllSlots();
			buildMenuController.ShowBuildingGroupMenu(buildingCategory);
		}

		public void SelectBuildingFromMenu(Building building)
		{
			Debug.Log("UIManager.SelectBuildingFromMenu()");
			SelectedBuilding = building;
			friendlyCruiser.HighlightAvailableSlots(building.slotType);
			buildableDetails.ShowBuildableDetails(building, allowDelete: false);
		}

		public void SelectBuilding(Building building, Cruiser buildingParent)
		{
			if (buildingParent == friendlyCruiser)
			{
				SelectBuildingFromFriendlyCruiser(building);
			}
			else
			{
				SelectBuildingFromEnemyCruiser();
			}
		}

		public void SelectBuildingFromFriendlyCruiser(Building building)
		{
			Debug.Log("UIManager.SelectBuildingFromFriendlyCruiser()");
			friendlyCruiser.UnhighlightSlots();
			buildableDetails.ShowBuildableDetails(building, allowDelete: true);
		}

		public void SelectBuildingFromEnemyCruiser()
		{
			throw new NotImplementedException();
		}

		public void ShowFactoryUnits(Factory factory)
		{
			buildMenuController.ShowUnitsMenu(factory);
		}

		// FELIX  Also show details when clicking on real unit (not just unit button in factory panel)
		public void ShowUnitDetails(Unit unit)
		{
			buildableDetails.ShowBuildableDetails(unit, allowDelete: false);
		}

		public void HideBuildableDetails()
		{
			buildableDetails.Hide();
		}
	}
}