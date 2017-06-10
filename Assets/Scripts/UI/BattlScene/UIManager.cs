using BattleCruisers.Cameras;
using BattleCruisers.Cruisers;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.BuildingDetails;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene
{
	public class UIManager : MonoBehaviour 
	{
		private Cruiser _playerCruiser, _aiCruiser;

		public CameraController cameraController;
		public BackgroundController backgroundController;
		public BuildMenuController buildMenuController;
		public HealthBarController playerCruiserHealthBar, aiCruiserHealthBar;
		public BuildableDetailsController buildableDetails;

		public void Initialise(Cruiser playerCruiser, Cruiser aiCruiser)
		{
			_playerCruiser = playerCruiser;
			_aiCruiser = aiCruiser;

			playerCruiserHealthBar.gameObject.SetActive(true);
			aiCruiserHealthBar.gameObject.SetActive(false);
			
			cameraController.CameraTransitionStarted += OnCameraTransitionStarted;
			cameraController.CameraTransitionCompleted += OnCameraTransitionCompleted;
			backgroundController.BackgroundClicked += OnBackgroundClicked;
		}

		private void OnCameraTransitionStarted(object sender, CameraTransitionArgs e)
		{
			switch (e.Origin)
			{
				case CameraState.PlayerCruiser:
					buildMenuController.HideBuildMenu();
					_playerCruiser.HideAllSlots();
					buildableDetails.Hide();
					playerCruiserHealthBar.gameObject.SetActive(false);
					break;

				case CameraState.AiCruiser:
					aiCruiserHealthBar.gameObject.SetActive(false);
					break;
			}
		}

		private void OnCameraTransitionCompleted(object sender, CameraTransitionArgs e)
		{
			switch (e.Destination)
			{
				case CameraState.PlayerCruiser:
					buildMenuController.ShowBuildMenu();
					playerCruiserHealthBar.gameObject.SetActive(true);
					break;

				case CameraState.AiCruiser:
					aiCruiserHealthBar.gameObject.SetActive(true);
					break;
			}
		}

		private void OnBackgroundClicked(object sender, EventArgs e)
		{
			buildableDetails.Hide();
			_playerCruiser.UnhighlightSlots();
		}

		public void ShowBuildingGroups()
		{
			Logging.Log(Tags.UI_MANAGER, ".ShowBuildingGroups()");
			_playerCruiser.UnhighlightSlots();
			_playerCruiser.HideAllSlots();
			buildableDetails.Hide();
			buildMenuController.ShowBuildingGroupsMenu();
		}

		public void SelectBuildingGroup(BuildingCategory buildingCategory)
		{
			Logging.Log(Tags.UI_MANAGER, ".SelectBuildingGroup()");
			_playerCruiser.ShowAllSlots();
			buildMenuController.ShowBuildingGroupMenu(buildingCategory);
		}

		public void SelectBuildingFromMenu(BuildingWrapper buildingWrapper)
		{
			Logging.Log(Tags.UI_MANAGER, ".SelectBuildingFromMenu()");
			_playerCruiser.SelectedBuildingPrefab = buildingWrapper;
			_playerCruiser.HighlightAvailableSlots(buildingWrapper.Building.slotType);
			buildableDetails.ShowBuildableDetails(buildingWrapper.Building, allowDelete: false);
		}

		public void SelectBuilding(Building building, ICruiser buildingParent)
		{
			if (System.Object.ReferenceEquals(buildingParent, _playerCruiser)
				&& cameraController.State == CameraState.PlayerCruiser)
			{
				SelectBuildingFromFriendlyCruiser(building);
			}
			else if (System.Object.ReferenceEquals(buildingParent, _aiCruiser)
				&& cameraController.State == CameraState.AiCruiser)
			{
				SelectBuildingFromEnemyCruiser(building);
			}
		}

		public void SelectBuildingFromFriendlyCruiser(Building building)
		{
			Logging.Log(Tags.UI_MANAGER, "SelectBuildingFromFriendlyCruiser()");
			_playerCruiser.UnhighlightSlots();
			buildableDetails.ShowBuildableDetails(building, allowDelete: true);
		}

		public void SelectBuildingFromEnemyCruiser(Building building)
		{
			buildableDetails.ShowBuildableDetails(building, allowDelete: false);
		}

		public void ShowFactoryUnits(Factory factory)
		{
			if (cameraController.State == CameraState.PlayerCruiser)
			{
				buildMenuController.ShowUnitsMenu(factory);
			}
		}

		public void ShowUnitDetails(Unit unit)
		{
			buildableDetails.ShowBuildableDetails(unit, allowDelete: false);
		}
	}
}