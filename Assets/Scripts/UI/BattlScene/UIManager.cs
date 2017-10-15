using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cameras;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene
{
    public class UIManager : MonoBehaviour, IUIManager
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
					_playerCruiser.SlotWrapper.HideAllSlots();
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
			_playerCruiser.SlotWrapper.UnhighlightSlots();
		}

		public void ShowBuildingGroups()
		{
			Logging.Log(Tags.UI_MANAGER, ".ShowBuildingGroups()");
			_playerCruiser.SlotWrapper.UnhighlightSlots();
			_playerCruiser.SlotWrapper.HideAllSlots();
			buildableDetails.Hide();
			buildMenuController.ShowBuildingGroupsMenu();
		}

		public void SelectBuildingGroup(BuildingCategory buildingCategory)
		{
			Logging.Log(Tags.UI_MANAGER, ".SelectBuildingGroup()");
			_playerCruiser.SlotWrapper.ShowAllSlots();
			buildMenuController.ShowBuildingGroupMenu(buildingCategory);
		}

		public void SelectBuildingFromMenu(IBuildableWrapper<IBuilding> buildingWrapper)
		{
			Logging.Log(Tags.UI_MANAGER, ".SelectBuildingFromMenu()");
			_playerCruiser.SelectedBuildingPrefab = buildingWrapper;
			_playerCruiser.SlotWrapper.HighlightAvailableSlots(buildingWrapper.Buildable.SlotType);
			buildableDetails.ShowBuildableDetails(buildingWrapper.Buildable, allowDelete: false);
		}

		public void SelectBuilding(Building building, ICruiser buildingParent)
		{
			if (ReferenceEquals(buildingParent, _playerCruiser)
				&& cameraController.State == CameraState.PlayerCruiser)
			{
				SelectBuildingFromFriendlyCruiser(building);
			}
			else if (ReferenceEquals(buildingParent, _aiCruiser)
				&& cameraController.State == CameraState.AiCruiser)
			{
				SelectBuildingFromEnemyCruiser(building);
			}
		}

		public void SelectBuildingFromFriendlyCruiser(Building building)
		{
			Logging.Log(Tags.UI_MANAGER, "SelectBuildingFromFriendlyCruiser()");
			_playerCruiser.SlotWrapper.UnhighlightSlots();
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

		public void ShowUnitDetails(IUnit unit)
		{
			buildableDetails.ShowBuildableDetails(unit, allowDelete: false);
		}

        public void ShowCruiserDetails(ICruiser cruiser)
        {
            // FELIX
            Debug.Log("UIManager.ShowCruiserDetails()");
        }
    }
}