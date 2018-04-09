using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.Cameras;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene
{
    public class UIManager : IUIManager
	{
		private readonly ICruiser _playerCruiser, _aiCruiser;
        private readonly ICameraController _cameraController;
        private readonly IBuildMenu _buildMenu;
        private readonly IBuildableDetailsManager _detailsManager;
        private readonly IClickable _background;

        public UIManager(
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            ICameraController cameraController,
            IBuildMenu buildMenu,
            IClickable background,
            IBuildableDetailsManager detailsManager)
		{
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, cameraController, buildMenu, background, detailsManager);

			_playerCruiser = playerCruiser;
			_aiCruiser = aiCruiser;
            _cameraController = cameraController;
            _buildMenu = buildMenu;
            _background = background;
            _detailsManager = detailsManager;
   			
			_cameraController.CameraTransitionStarted += OnCameraTransitionStarted;
			_cameraController.CameraTransitionCompleted += OnCameraTransitionCompleted;
			_background.Clicked += OnBackgroundClicked;
        }

        /// <summary>
        /// Not in constructor because of circular dependency between UIManager
        /// and cruisers.  Hence need to wait until both the UIManager and
        /// cruisers are setup before executing this method.
        /// </summary>
        public void InitialUI()
        {
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
					break;

                case CameraState.AiCruiser:
                    _aiCruiser.SlotWrapper.UnhighlightSlots();
                    _detailsManager.HideDetails();
                    break;
			}
		}

		private void OnCameraTransitionCompleted(object sender, CameraTransitionArgs e)
		{
            if (e.Destination == CameraState.PlayerCruiser)
            {
				_buildMenu.ShowBuildMenu();
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

		public void SelectBuilding(IBuilding building, ICruiser buildingParent)
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

		private void SelectBuildingFromFriendlyCruiser(IBuilding building)
		{
			_playerCruiser.SlotWrapper.UnhighlightSlots();
            _playerCruiser.SlotWrapper.HighlightBuildingSlot(building);
            _detailsManager.ShowDetails(building, allowDelete: true);
		}

		private void SelectBuildingFromEnemyCruiser(IBuilding building)
		{
            _aiCruiser.SlotWrapper.HighlightBuildingSlot(building);
            _detailsManager.ShowDetails(building, allowDelete: false);
		}

		public void ShowFactoryUnits(IFactory factory)
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

        public void ShowCruiserDetails(ICruiser cruiser)
        {
            _detailsManager.ShowDetails(cruiser);
        }
    }
}
