using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Common.BuildableDetails;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.BattleScene.Manager
{
    public class UIManagerTests
    {
        private IUIManager _uiManager;

        private ICruiser _playerCruiser, _aiCruiser;
        private ICameraController _cameraController;
        private IBuildMenu _buildMenu;
        private IBuildableDetailsManager _detailsManager;
        private IActivenessDecider<IBuilding> _buildingDeleteButtonActivenessDecider;

        private IBuilding _building;
        private IFactory _factory;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _playerCruiser = CreateMockCruiser();
            _aiCruiser = CreateMockCruiser();
            _cameraController = Substitute.For<ICameraController>();
            _buildMenu = Substitute.For<IBuildMenu>();
            _detailsManager = Substitute.For<IBuildableDetailsManager>();
            _buildingDeleteButtonActivenessDecider = Substitute.For<IActivenessDecider<IBuilding>>();

            IManagerArgs managerArgs
                = new ManagerArgs(
                    _playerCruiser,
                    _aiCruiser,
                    _cameraController,
                    _buildMenu,
                    _detailsManager,
                    _buildingDeleteButtonActivenessDecider);
            _uiManager = new UIManager(managerArgs);

            _building = Substitute.For<IBuilding>();
            _building.SlotType.Returns(SlotType.Platform);

            _factory = Substitute.For<IFactory>();
        }

        private ICruiser CreateMockCruiser()
        {
            ICruiser cruiser = Substitute.For<ICruiser>();

			ISlotWrapper slotWrapper = Substitute.For<ISlotWrapper>();
            cruiser.SlotWrapper.Returns(slotWrapper);

            return cruiser;
        }

        [Test]
        public void InitialUI()
        {
            _uiManager.InitialUI();
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
        }

        [Test]
        public void CameraTransitionStarted_OriginAiCruiser()
        {
            CameraTransitionArgs args = new CameraTransitionArgs(origin: CameraState.AiCruiser, destination: CameraState.PlayerCruiser);
            _cameraController.CameraTransitionStarted += Raise.EventWith(_cameraController, args);

            _aiCruiser.SlotWrapper.Received().UnhighlightSlots();
            _detailsManager.Received().HideDetails();
        }

        [Test]
        public void CameraTransitionCompleted_DestinationPlayerCruiser()
        {
            CameraTransitionArgs args = new CameraTransitionArgs(origin: CameraState.AiCruiser, destination: CameraState.PlayerCruiser);
            _cameraController.CameraTransitionCompleted += Raise.EventWith(_cameraController, args);

            _buildMenu.Received().ShowBuildMenu();
        }

        [Test]
        public void CameraTransitionCompleted_DestinationAiCruiser()
        {
            CameraTransitionArgs args = new CameraTransitionArgs(origin: CameraState.PlayerCruiser, destination: CameraState.AiCruiser);
            _cameraController.CameraTransitionCompleted += Raise.EventWith(_cameraController, args);
        }
        #endregion Camera transitions

        [Test]
        public void HideItemDetails()
        {
            _uiManager.HideItemDetails();

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
            IBuildableWrapper<IBuilding> buildingWrapper = Substitute.For<IBuildableWrapper<IBuilding>>();
            buildingWrapper.Buildable.Returns(_building);

            _uiManager.SelectBuildingFromMenu(buildingWrapper);

            Assert.AreSame(buildingWrapper, _playerCruiser.SelectedBuildingPrefab);
            _playerCruiser.SlotWrapper.Received().HighlightAvailableSlots(buildingWrapper.Buildable.SlotType);
            _detailsManager.Received().ShowDetails(buildingWrapper.Buildable, allowDelete: false);
        }

        #region SelectBuilding()
        [Test]
        public void SelectBuilding_ParentIsPlayerCruiser_CameraAtPlayerCruiser_ShowsDetails()
        {
            _cameraController.State.Returns(CameraState.PlayerCruiser);
            _buildingDeleteButtonActivenessDecider.ShouldBeEnabled(_building).Returns(true);

            _uiManager.SelectBuilding(_building, _playerCruiser);

            _buildingDeleteButtonActivenessDecider.Received().ShouldBeEnabled(_building);
            _playerCruiser.SlotWrapper.Received().UnhighlightSlots();
            _playerCruiser.SlotWrapper.Received().HighlightBuildingSlot(_building);
            _detailsManager.Received().ShowDetails(_building, allowDelete: true);
        }

        [Test]
        public void SelectBuilding_ParentIsNotPlayerCruiser_DoesNothing()
        {
            _buildingDeleteButtonActivenessDecider.ShouldBeEnabled(_building).Returns(false);

            _uiManager.SelectBuilding(_building, _aiCruiser);

            _buildingDeleteButtonActivenessDecider.Received().ShouldBeEnabled(_building);
            _playerCruiser.SlotWrapper.DidNotReceive().UnhighlightSlots();
        }

        [Test]
        public void SelectBuilding_ParentIsPlayerCruiser_CameraNotAtPlayerCruiser_DoesNothing()
        {
            _cameraController.State.Returns(CameraState.Overview);

            _uiManager.SelectBuilding(_building, _playerCruiser);

            _playerCruiser.SlotWrapper.DidNotReceive().UnhighlightSlots();
        }

        [Test]
        public void SelectBuilding_ParentIsAiCruiser_CameraAtAiCruiser_ShowsDetails()
        {
            _cameraController.State.Returns(CameraState.AiCruiser);

            _uiManager.SelectBuilding(_building, _aiCruiser);

            _aiCruiser.SlotWrapper.Received().HighlightBuildingSlot(_building);
            _detailsManager.Received().ShowDetails(_building, allowDelete: false);
        }

        [Test]
        public void SelectBuilding_ParentIsNotAiCruiser_DoesNothing()
        {
            _uiManager.SelectBuilding(_building, _playerCruiser);
            _aiCruiser.SlotWrapper.DidNotReceive().HighlightBuildingSlot(_building);
        }

        [Test]
        public void SelectBuilding_ParentIsAiCruiser_CameraNotAtAiCruiser_DoesNothing()
        {
            _cameraController.State.Returns(CameraState.Overview);

            _uiManager.SelectBuilding(_building, _aiCruiser);

            _aiCruiser.SlotWrapper.DidNotReceive().HighlightBuildingSlot(_building);
        }
        #endregion SelectBuilding()

        [Test]
        public void ShowFactoryUnits_AtPlayerCruiser_ShowsDetails()
        {
            _cameraController.State.Returns(CameraState.PlayerCruiser);

            _uiManager.ShowFactoryUnits(_factory);

            _buildMenu.Received().ShowUnitsMenu(_factory);
        }

        [Test]
        public void ShowFactoryUnits_NotAtPlayerCruiser_DoesNothing()
        {
            _cameraController.State.Returns(CameraState.Overview);

            _uiManager.ShowFactoryUnits(_factory);

            _buildMenu.DidNotReceive().ShowUnitsMenu(_factory);
        }

        [Test]
        public void ShowUnitDetails()
        {
            IUnit unit = Substitute.For<IUnit>();

            _uiManager.ShowUnitDetails(unit);

            _detailsManager.Received().ShowDetails(unit);
        }

        [Test]
        public void ShowCruiserDetails()
        {
            _uiManager.ShowCruiserDetails(_playerCruiser);
            _detailsManager.Received().ShowDetails(_playerCruiser);
        }
    }
}
