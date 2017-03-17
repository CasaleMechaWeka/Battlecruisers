using BattleCruisers.Cruisers;
using BattleCruisers.Buildings;
using BattleCruisers.UI;
using BattleCruisers.UI.BuildMenus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleCruisers.UI.BuildingDetails;

namespace BattleCruisers.UI
{
	public class UIManager : MonoBehaviour 
	{
		public CameraController cameraController;
		public BuildMenuController buildMenuController;
		public HealthBarController friendlyCruiserHealthBar;
		public HealthBarController enemyCruiserHealthBar;
		public Cruiser friendlyCruiser;
		public BuildingDetailsController buildingDetails;

		public Building SelectedBuilding { get; private set; }

		// Use this for initialization
		void Start () 
		{
			friendlyCruiserHealthBar.gameObject.SetActive(true);
			enemyCruiserHealthBar.gameObject.SetActive(false);

			cameraController.CameraTransitionStarted += OnCameraTransitionStarted;
			cameraController.CameraTransitionCompleted += OnCameraTransitionCompleted;
		}

		private void OnCameraTransitionStarted(object sender, CameraTransitionArgs e)
		{
			switch (e.Origin)
			{
				case CameraState.FriendlyCruiser:
					buildMenuController.HideBuildMenu();
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

		public void ShowBuildingGroups()
		{
			Debug.Log("UIManager.ShowBuildingGroups()");
			friendlyCruiser.UnhighlightSlots();
			friendlyCruiser.HideAllSlots();
			buildingDetails.Hide();
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
			buildingDetails.ShowBuildingDetails(building, allowDelete: false);
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
			buildingDetails.ShowBuildingDetails(building, allowDelete: true);
		}

		public void SelectBuildingFromEnemyCruiser()
		{
			throw new NotImplementedException();
		}
	}
}