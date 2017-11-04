using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cameras;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.Common.BuildingDetails;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.BattleScene
{
    public class UIManagerTests
    {
        private IUIManager _uiManager;

        private ICruiser _playerCruiser, _aiCruiser;
        private ICameraController _cameraController;
        private IBuildMenu _buildMenu;
        private IBuildableDetailsManager _detailsManager;
        private IClickable _background;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _playerCruiser = CreateMockCruiser();
            _aiCruiser = CreateMockCruiser();
            _cameraController = Substitute.For<ICameraController>();
            _buildMenu = Substitute.For<IBuildMenu>();
            _detailsManager = Substitute.For<IBuildableDetailsManager>();
            _background = Substitute.For<IClickable>();

            _uiManager
                = new UIManager(
                    _playerCruiser,
                    _aiCruiser,
                    _cameraController,
                    _buildMenu,
                    _background,
                    _detailsManager);
        }

        private ICruiser CreateMockCruiser()
        {
            IGameObject healthBar = Substitute.For<IGameObject>();
            ISlotWrapper slotWrapper = Substitute.For<ISlotWrapper>();

            ICruiser cruiser = Substitute.For<ICruiser>();

            cruiser.HealthBar.Returns(healthBar);
            cruiser.SlotWrapper.Returns(slotWrapper);

            return cruiser;
        }

        [Test]
        public void InitialUI()
        {
            _uiManager.InitialUI();

            Assert.IsTrue(_playerCruiser.HealthBar.IsVisible);
            Assert.IsFalse(_aiCruiser.HealthBar.IsVisible);

            _detailsManager.Received().HideDetails();
        }

        #region Camera transitions
        [Test]
        public void CameraTransitionStarted_OriginPlayerCruiser()
        {
            CameraTransitionArgs args = new CameraTransitionArgs(origin: CameraState.PlayerCruiser, destination: CameraState.AiCruiser);
            _cameraController.CameraTransitionStarted += Raise.EventWith(_cameraController, args);

            _buildMenu.Received().HideBuildMenu();
            _playerCruiser.SlotWrapper.Received().HideAllSlots();
            _detailsManager.Received().HideDetails();
            Assert.IsFalse(_playerCruiser.HealthBar.IsVisible);
        }

        [Test]
        public void CameraTransitionStarted_OriginAiCruiser()
        {
            CameraTransitionArgs args = new CameraTransitionArgs(origin: CameraState.AiCruiser, destination: CameraState.PlayerCruiser);
            _cameraController.CameraTransitionStarted += Raise.EventWith(_cameraController, args);

            _aiCruiser.SlotWrapper.Received().UnhighlightSlots();
            _detailsManager.Received().HideDetails();
            Assert.IsFalse(_aiCruiser.HealthBar.IsVisible);
        }

        [Test]
        public void CameraTransitionCompleted_DestinationPlayerCruiser()
        {
            CameraTransitionArgs args = new CameraTransitionArgs(origin: CameraState.AiCruiser, destination: CameraState.PlayerCruiser);
            _cameraController.CameraTransitionCompleted += Raise.EventWith(_cameraController, args);

            _buildMenu.Received().ShowBuildMenu();
            Assert.IsTrue(_playerCruiser.HealthBar.IsVisible);
        }

        [Test]
        public void CameraTransitionCompleted_DestinationAiCruiser()
        {
            CameraTransitionArgs args = new CameraTransitionArgs(origin: CameraState.PlayerCruiser, destination: CameraState.AiCruiser);
            _cameraController.CameraTransitionCompleted += Raise.EventWith(_cameraController, args);

            Assert.IsTrue(_aiCruiser.HealthBar.IsVisible);
        }
        #endregion Camera transitions

        [Test]
        public void BackgroundClicked()
        {
            _background.Clicked += Raise.Event();

            _detailsManager.Received().HideDetails();
            _playerCruiser.SlotWrapper.Received().UnhighlightSlots();
            _aiCruiser.SlotWrapper.Received().UnhighlightSlots();
        }

        [Test]
        public void ShowBuildingGroups()
        {
            _uiManager.ShowBuildingGroups();

            _playerCruiser.SlotWrapper.Received().UnhighlightSlots();
            _playerCruiser.SlotWrapper.Received().HideAllSlots();
            _detailsManager.Received().HideDetails();
            _buildMenu.Received().ShowBuildingGroupsMenu();
        }

        [Test]
        public void SelectBuildingGroup()
        {
            BuildingCategory buildingCategory = BuildingCategory.Ultra;
            _uiManager.SelectBuildingGroup(buildingCategory);

            _playerCruiser.SlotWrapper.Received().ShowAllSlots();
            _buildMenu.Received().ShowBuildingGroupMenu(buildingCategory);
        }

        [Test]
        public void SelectBuildingFromMenu()
        {
            IBuilding building = Substitute.For<IBuilding>();
            building.SlotType.Returns(SlotType.Platform);

            IBuildableWrapper<IBuilding> buildingWrapper = Substitute.For<IBuildableWrapper<IBuilding>>();
            buildingWrapper.Buildable.Returns(building);

            _uiManager.SelectBuildingFromMenu(buildingWrapper);

            Assert.AreSame(buildingWrapper, _playerCruiser.SelectedBuildingPrefab);
            _playerCruiser.SlotWrapper.Received().HighlightAvailableSlots(buildingWrapper.Buildable.SlotType);
            _detailsManager.Received().ShowDetails(buildingWrapper.Buildable, allowDelete: false);
        }
    }
}
