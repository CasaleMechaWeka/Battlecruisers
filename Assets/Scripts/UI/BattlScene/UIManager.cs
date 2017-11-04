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

namespace BattleCruisers.UI.BattleScene
{
    public class UIManager : IUIManager
	{
		private readonly ICruiser _playerCruiser, _aiCruiser;
        private readonly ICameraController _cameraController;
        private readonly IBuildMenu _buildMenu;
        private readonly IGameObject _playerCruiserHealthBar, _aiCruiserHealthBar;
        private readonly IBuildableDetailsManager _detailsManager;
        private readonly IClickable _background;

        public UIManager(
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            ICameraController cameraController,
            IBuildMenu buildMenu,
            IClickable background,
            IGameObject playerCruiserHealthBar,
            IGameObject aiCruiserHealthBar,
            IBuildableDetailsManager detailsManager)
		{
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, cameraController, buildMenu, 
                background, playerCruiserHealthBar, aiCruiserHealthBar, detailsManager);

			_playerCruiser = playerCruiser;
			_aiCruiser = aiCruiser;
            _cameraController = cameraController;
            _buildMenu = buildMenu;
            _background = background;
            _playerCruiserHealthBar = playerCruiserHealthBar;
            _aiCruiserHealthBar = aiCruiserHealthBar;
            _detailsManager = detailsManager;

			_playerCruiserHealthBar.IsVisible = true;
			_aiCruiserHealthBar.IsVisible = false;
			
			_cameraController.CameraTransitionStarted += OnCameraTransitionStarted;
			_cameraController.CameraTransitionCompleted += OnCameraTransitionCompleted;
			_background.Clicked += OnBackgroundClicked;

            _detailsManager.HideDetails();
		}

		private void OnCameraTransitionStarted(object sender, CameraTransitionArgs e)
		{
			switch (e.Origin)
			{
				case CameraState.PlayerCruiser:
					_buildMenu.HideBuildMenu();
					_playerCruiser.SlotWrapper.HideAllSlots();
                    _detailsManager.HideDetails();
					_playerCruiserHealthBar.IsVisible = false;
					break;

                case CameraState.AiCruiser:
                    _aiCruiser.SlotWrapper.UnhighlightSlots();
                    _detailsManager.HideDetails();
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
            _detailsManager.HideDetails();
			_playerCruiser.SlotWrapper.UnhighlightSlots();
            _aiCruiser.SlotWrapper.UnhighlightSlots();
		}

		public void ShowBuildingGroups()
        {
            Logging.Log(Tags.UI_MANAGER, ".ShowBuildingGroups()");

            _playerCruiser.SlotWrapper.UnhighlightSlots();
            _playerCruiser.SlotWrapper.HideAllSlots();
            _detailsManager.HideDetails();
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
            _detailsManager.ShowDetails(buildingWrapper.Buildable, allowDelete: false);
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
            _detailsManager.ShowDetails(building, allowDelete: true);
		}

		public void SelectBuildingFromEnemyCruiser(Building building)
		{
            _aiCruiser.SlotWrapper.HighlightBuildingSlot(building);
            _detailsManager.ShowDetails(building, allowDelete: false);
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
            _detailsManager.ShowDetails(unit);
		}

        public void ShowCruiserDetails(Cruiser cruiser)
        {
            _detailsManager.ShowDetails(cruiser);
        }
    }
}
