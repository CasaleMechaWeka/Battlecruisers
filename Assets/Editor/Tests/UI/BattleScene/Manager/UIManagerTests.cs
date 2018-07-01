using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene.BuildMenus;
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
        private IBuildMenu _buildMenu;
        private IBuildableDetailsManager _detailsManager;
        private IFilter<IBuilding> _shouldBuildingDeleteButtonBeEnabledFilter;

        private IBuilding _building;
        private IFactory _factory;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _playerCruiser = CreateMockCruiser();
            _aiCruiser = CreateMockCruiser();
            _buildMenu = Substitute.For<IBuildMenu>();
            _detailsManager = Substitute.For<IBuildableDetailsManager>();
            _shouldBuildingDeleteButtonBeEnabledFilter = Substitute.For<IFilter<IBuilding>>();

            IManagerArgs managerArgs
                = new ManagerArgs(
                    _playerCruiser,
                    _aiCruiser,
                    _buildMenu,
                    _detailsManager,
                    _shouldBuildingDeleteButtonBeEnabledFilter);
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
            _shouldBuildingDeleteButtonBeEnabledFilter.IsMatch(_building).Returns(true);
            _building.ParentCruiser.Returns(_playerCruiser);

            _uiManager.SelectBuilding(_building);

            _shouldBuildingDeleteButtonBeEnabledFilter.Received().IsMatch(_building);
            _playerCruiser.SlotWrapper.Received().UnhighlightSlots();
            _playerCruiser.SlotWrapper.Received().HighlightBuildingSlot(_building);
            _detailsManager.Received().ShowDetails(_building, allowDelete: true);
        }

        [Test]
        public void SelectBuilding_ParentIsNotPlayerCruiser_DoesNothing()
        {
            _building.ParentCruiser.Returns(_aiCruiser);

            _uiManager.SelectBuilding(_building);

            _playerCruiser.SlotWrapper.DidNotReceive().UnhighlightSlots();
        }

        [Test]
        public void SelectBuilding_ParentIsPlayerCruiser_CameraNotAtPlayerCruiser_DoesNothing()
        {
            _building.ParentCruiser.Returns(_playerCruiser);

            _uiManager.SelectBuilding(_building);

            _playerCruiser.SlotWrapper.DidNotReceive().UnhighlightSlots();
        }

        [Test]
        public void SelectBuilding_ParentIsAiCruiser_CameraAtAiCruiser_ShowsDetails()
        {
            _building.ParentCruiser.Returns(_aiCruiser);

            _uiManager.SelectBuilding(_building);

            _aiCruiser.SlotWrapper.Received().HighlightBuildingSlot(_building);
            _detailsManager.Received().ShowDetails(_building, allowDelete: false);
        }

        [Test]
        public void SelectBuilding_ParentIsNotAiCruiser_DoesNothing()
        {
            _building.ParentCruiser.Returns(_playerCruiser);
            _uiManager.SelectBuilding(_building);
            _aiCruiser.SlotWrapper.DidNotReceive().HighlightBuildingSlot(_building);
        }

        [Test]
        public void SelectBuilding_ParentIsAiCruiser_CameraNotAtAiCruiser_DoesNothing()
        {
            _building.ParentCruiser.Returns(_aiCruiser);

            _uiManager.SelectBuilding(_building);

            _aiCruiser.SlotWrapper.DidNotReceive().HighlightBuildingSlot(_building);
        }
        #endregion SelectBuilding()

        [Test]
        public void ShowFactoryUnits_AtPlayerCruiser_ShowsDetails()
        {
            _uiManager.ShowFactoryUnits(_factory);

            _buildMenu.Received().ShowUnitsMenu(_factory);
        }

        [Test]
        public void ShowFactoryUnits_NotAtPlayerCruiser_DoesNothing()
        {
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
