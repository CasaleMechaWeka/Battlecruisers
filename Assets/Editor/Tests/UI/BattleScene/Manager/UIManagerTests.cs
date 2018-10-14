using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.Manager;
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

            IManagerArgs managerArgs
                = new ManagerArgs(
                    _playerCruiser,
                    _aiCruiser,
                    _buildMenu,
                    _detailsManager);
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
            _buildMenu.Received().ShowBuildMenu();
        }

        [Test]
        public void HideItemDetails()
        {
            _uiManager.HideItemDetails();
            Expect_HideItemDetails();
        }

        [Test]
        public void ShowBuildingGroups()
        {
            _uiManager.ShowBuildingGroups();

            _playerCruiser.SlotWrapper.Received().UnhighlightSlots();
            _detailsManager.Received().HideDetails();
            _buildMenu.Received().ShowBuildingGroupsMenu();
        }

        [Test]
        public void SelectBuildingGroup()
        {
            BuildingCategory buildingCategory = BuildingCategory.Ultra;
            _uiManager.SelectBuildingGroup(buildingCategory);

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
            _detailsManager.Received().ShowDetails(buildingWrapper.Buildable);
        }

        #region SelectBuilding()
        [Test]
        public void SelectBuilding_ParentIsPlayerCruiser()
        {
            _building.ParentCruiser.Returns(_playerCruiser);

            _uiManager.SelectBuilding(_building);

            Expect_HideItemDetails();

            _playerCruiser.SlotWrapper.Received().HighlightBuildingSlot(_building);
            _detailsManager.Received().ShowDetails(_building);

            _aiCruiser.SlotWrapper.DidNotReceive().HighlightBuildingSlot(_building);
        }

        [Test]
        public void SelectBuilding_ParentIsAiCruiser_ShowsDetails()
        {
            _building.ParentCruiser.Returns(_aiCruiser);

            _uiManager.SelectBuilding(_building);

            Expect_HideItemDetails();

            _aiCruiser.SlotWrapper.Received().HighlightBuildingSlot(_building);
            _detailsManager.Received().ShowDetails(_building);

            _playerCruiser.SlotWrapper.DidNotReceive().HighlightBuildingSlot(_building);
        }
        #endregion SelectBuilding()

        [Test]
        public void ShowFactoryUnits_PlayerCruiserFactory_ShowsDetails()
        {
            _factory.ParentCruiser.Returns(_playerCruiser);
            _uiManager.ShowFactoryUnits(_factory);
            _buildMenu.Received().ShowUnitsMenu(_factory);
        }

        [Test]
        public void ShowFactoryUnits_NotPlayerCruiserFactory_DoesNothing()
        {
            _factory.ParentCruiser.Returns(_aiCruiser);
            _uiManager.ShowFactoryUnits(_factory);
            _buildMenu.DidNotReceive().ShowUnitsMenu(_factory);
        }

        [Test]
        public void ShowUnitDetails()
        {
            IUnit unit = Substitute.For<IUnit>();

            _uiManager.ShowUnitDetails(unit);

            Expect_HideItemDetails();
            _detailsManager.Received().ShowDetails(unit);
        }

        [Test]
        public void ShowCruiserDetails()
        {
            _uiManager.ShowCruiserDetails(_playerCruiser);

            Expect_HideItemDetails();
            _detailsManager.Received().ShowDetails(_playerCruiser);
        }

        private void Expect_HideItemDetails()
        {
            _detailsManager.Received().HideDetails();
            _playerCruiser.SlotWrapper.Received().UnhighlightSlots();
            _aiCruiser.SlotWrapper.Received().UnhighlightSlots();
        }
    }
}
