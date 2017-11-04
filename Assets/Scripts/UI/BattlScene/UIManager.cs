using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cameras;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene
{
    // FELIX  Test???
    // 1. Turn everything into interfaces O_o
    // 2. Seprate from MonoBehaviour :P  Don't even need to be MonoBehaviour if
    // I inject EVERYTHING form BattleSceneGod??
    public class UIManager : MonoBehaviour, IUIManager
	{
        // FELIX  Interface!
		private ICruiser _playerCruiser, _aiCruiser;
        private ICameraController _cameraController;
        private IBuildMenu _buildMenu;
        private IGameObject _playerCruiserHealthBar, _aiCruiserHealthBar;
		private BuildableDetailsController _buildableDetails;
		private InBattleCruiserDetailsController _cruiserDetails;
        private IClickable _background;

        public void Initialise(
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            ICameraController cameraController,
            IBuildMenu buildMenu,
            IGameObject playerCruiserHealthBar,
            IGameObject aiCruiserHealthBar,
            IClickable background)
		{
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, cameraController, buildMenu, playerCruiserHealthBar, aiCruiserHealthBar, background);

			_playerCruiser = playerCruiser;
			_aiCruiser = aiCruiser;
            _cameraController = cameraController;
            _buildMenu = buildMenu;
            _playerCruiserHealthBar = playerCruiserHealthBar;
            _aiCruiserHealthBar = aiCruiserHealthBar;
            _background = background;

            _buildableDetails = GetComponentInChildren<BuildableDetailsController>(includeInactive: true);
            Assert.IsNotNull(_buildableDetails);

            _cruiserDetails = GetComponentInChildren<InBattleCruiserDetailsController>(includeInactive: true);
            Assert.IsNotNull(_cruiserDetails);

			_playerCruiserHealthBar.IsVisible = true;
			_aiCruiserHealthBar.IsVisible = false;
			
			_cameraController.CameraTransitionStarted += OnCameraTransitionStarted;
			_cameraController.CameraTransitionCompleted += OnCameraTransitionCompleted;
			_background.Clicked += OnBackgroundClicked;

            HideTargetDetails();
		}

		private void OnCameraTransitionStarted(object sender, CameraTransitionArgs e)
		{
			switch (e.Origin)
			{
				case CameraState.PlayerCruiser:
					_buildMenu.HideBuildMenu();
					_playerCruiser.SlotWrapper.HideAllSlots();
                    HideTargetDetails();
					_playerCruiserHealthBar.IsVisible = false;
					break;

                case CameraState.AiCruiser:
                    _aiCruiser.SlotWrapper.UnhighlightSlots();
                    HideTargetDetails();
					_aiCruiserHealthBar.IsVisible = false;
                    break;
			}
		}

		private void OnCameraTransitionCompleted(object sender, CameraTransitionArgs e)
		{
			switch (e.Destination)
			{
				case CameraState.PlayerCruiser:
					_buildMenu.ShowBuildMenu();
                    _playerCruiserHealthBar.IsVisible = true;
					break;

				case CameraState.AiCruiser:
					_aiCruiserHealthBar.IsVisible = true;
					break;
			}
		}

		private void OnBackgroundClicked(object sender, EventArgs e)
		{
            HideTargetDetails();
			_playerCruiser.SlotWrapper.UnhighlightSlots();
            _aiCruiser.SlotWrapper.UnhighlightSlots();
		}

		public void ShowBuildingGroups()
        {
            Logging.Log(Tags.UI_MANAGER, ".ShowBuildingGroups()");

            _playerCruiser.SlotWrapper.UnhighlightSlots();
            _playerCruiser.SlotWrapper.HideAllSlots();
            HideTargetDetails();
            _buildMenu.ShowBuildingGroupsMenu();
        }

        public void SelectBuildingGroup(BuildingCategory buildingCategory)
		{
			Logging.Log(Tags.UI_MANAGER, ".SelectBuildingGroup()");
			_playerCruiser.SlotWrapper.ShowAllSlots();
			_buildMenu.ShowBuildingGroupMenu(buildingCategory);
		}

		public void SelectBuildingFromMenu(IBuildableWrapper<IBuilding> buildingWrapper)
		{
			Logging.Log(Tags.UI_MANAGER, ".SelectBuildingFromMenu()");
			_playerCruiser.SelectedBuildingPrefab = buildingWrapper;
			_playerCruiser.SlotWrapper.HighlightAvailableSlots(buildingWrapper.Buildable.SlotType);
            ShowBuildableDetails(buildingWrapper.Buildable, allowDelete: false);
		}

		public void SelectBuilding(Building building, ICruiser buildingParent)
		{
			if (ReferenceEquals(buildingParent, _playerCruiser)
				&& _cameraController.State == CameraState.PlayerCruiser)
			{
				SelectBuildingFromFriendlyCruiser(building);
			}
			else if (ReferenceEquals(buildingParent, _aiCruiser)
				&& _cameraController.State == CameraState.AiCruiser)
			{
				SelectBuildingFromEnemyCruiser(building);
			}
		}

		public void SelectBuildingFromFriendlyCruiser(Building building)
		{
			Logging.Log(Tags.UI_MANAGER, "SelectBuildingFromFriendlyCruiser()");

			_playerCruiser.SlotWrapper.UnhighlightSlots();
            _playerCruiser.SlotWrapper.HighlightBuildingSlot(building);
            ShowBuildableDetails(building, allowDelete: true);
		}

		public void SelectBuildingFromEnemyCruiser(Building building)
		{
            _aiCruiser.SlotWrapper.HighlightBuildingSlot(building);
            ShowBuildableDetails(building, allowDelete: false);
		}

		public void ShowFactoryUnits(Factory factory)
		{
			if (_cameraController.State == CameraState.PlayerCruiser)
			{
				_buildMenu.ShowUnitsMenu(factory);
			}
		}

		public void ShowUnitDetails(IUnit unit)
		{
            ShowBuildableDetails(unit, allowDelete: false);
		}

        private void ShowBuildableDetails(IBuildable buildable, bool allowDelete)
        {
            _cruiserDetails.Hide();
            _buildableDetails.ShowBuildableDetails(buildable, allowDelete);
        }

        public void ShowCruiserDetails(Cruiser cruiser)
        {
            _buildableDetails.Hide();
            _cruiserDetails.ShowCruiserDetails(cruiser);
        }

        private void HideTargetDetails()
        {
            _buildableDetails.Hide();
            _cruiserDetails.Hide();
        }
    }
}
