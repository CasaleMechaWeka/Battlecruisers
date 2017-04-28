using BattleCruisers.Cruisers;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BuildingDetails;
using BattleCruisers.UI.BuildMenus;
using BattleCruisers.UI.ProgressBars;
using BattleCruisers.Utils;
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
		public Cruiser enemyCruiser;
		public BuildableDetailsController buildableDetails;

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
			Logging.Log(Tags.UI_MANAGER, ".ShowBuildingGroups()");
			friendlyCruiser.UnhighlightSlots();
			friendlyCruiser.HideAllSlots();
			buildableDetails.Hide();
			buildMenuController.ShowBuildingGroupsMenu();
		}

		public void SelectBuildingGroup(BuildingCategory buildingCategory)
		{
			Logging.Log(Tags.UI_MANAGER, ".SelectBuildingGroup()");
			friendlyCruiser.ShowAllSlots();
			buildMenuController.ShowBuildingGroupMenu(buildingCategory);
		}

		public void SelectBuildingFromMenu(Building building)
		{
			Logging.Log(Tags.UI_MANAGER, ".SelectBuildingFromMenu()");
			friendlyCruiser.SelectedBuildingPrefab = building;
			friendlyCruiser.HighlightAvailableSlots(building.slotType);
			buildableDetails.ShowBuildableDetails(building, allowDelete: false);
		}

		public void SelectBuilding(Building building, ICruiser buildingParent)
		{
			if (System.Object.ReferenceEquals(buildingParent, friendlyCruiser)
				&& cameraController.State == CameraState.FriendlyCruiser)
			{
				SelectBuildingFromFriendlyCruiser(building);
			}
			else if (System.Object.ReferenceEquals(buildingParent, enemyCruiser)
				&& cameraController.State == CameraState.EnemyCruiser)
			{
				SelectBuildingFromEnemyCruiser(building);
			}
		}

		public void SelectBuildingFromFriendlyCruiser(Building building)
		{
			Logging.Log(Tags.UI_MANAGER, "SelectBuildingFromFriendlyCruiser()");
			friendlyCruiser.UnhighlightSlots();
			buildableDetails.ShowBuildableDetails(building, allowDelete: true);
		}

		public void SelectBuildingFromEnemyCruiser(Building building)
		{
			buildableDetails.ShowBuildableDetails(building, allowDelete: false);
		}

		public void ShowFactoryUnits(Factory factory)
		{
			buildMenuController.ShowUnitsMenu(factory);
		}

		public void ShowUnitDetails(Unit unit)
		{
			buildableDetails.ShowBuildableDetails(unit, allowDelete: false);
		}
	}
}