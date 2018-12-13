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
    public class UIManagerNEWTests
    {
        private UIManager _uiManager;

        private ICruiser _playerCruiser, _aiCruiser;
        private IBuildMenu _buildMenu;
        private IItemDetailsManager _detailsManager;

        private IBuilding _building;
        private IFactory _factory;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _playerCruiser = CreateMockCruiser();
            _aiCruiser = CreateMockCruiser();
            _buildMenu = Substitute.For<IBuildMenu>();
            _detailsManager = Substitute.For<IItemDetailsManager>();

            ManagerArgs managerArgs
                = new ManagerArgs(
                    _playerCruiser,
                    _aiCruiser,
                    _buildMenu,
                    _detailsManager);
            _uiManager = new UIManager();
            _uiManager.Initialise(managerArgs);

            _building = Substitute.For<IBuilding>();
            SlotSpecification slotSpecification = new SlotSpecification(SlotType.Platform, default(BuildingFunction), default(bool));
            _building.SlotSpecification.Returns(slotSpecification);

            _factory = Substitute.For<IFactory>();
        }

        private ICruiser CreateMockCruiser()
        {
            ICruiser cruiser = Substitute.For<ICruiser>();

			ISlotHighlighter slotHighlighter = Substitute.For<ISlotHighlighter>();
            cruiser.SlotHighlighter.Returns(slotHighlighter);

            return cruiser;
        }

        [Test]
        public void HideItemDetails()
        {
            _uiManager.HideItemDetails();
            Received_HideItemDetails();
        }

        [Test]
        public void HideCurrentlyShownMenu()
        {
            _uiManager.HideCurrentlyShownMenu();

            _playerCruiser.SlotHighlighter.Received().UnhighlightSlots();
            _detailsManager.Received().HideDetails();
            _buildMenu.Received().HideCurrentlyShownMenu();
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
            _playerCruiser.SlotHighlighter.Received().HighlightAvailableSlots(buildingWrapper.Buildable.SlotSpecification.SlotType);
            _detailsManager.Received().ShowDetails(buildingWrapper.Buildable);
        }

        #region SelectBuilding()
        [Test]
        public void SelectBuilding_ParentIsPlayerCruiser()
        {
            _building.ParentCruiser.Returns(_playerCruiser);

            _uiManager.SelectBuilding(_building);

            Received_HideItemDetails();

            _playerCruiser.SlotHighlighter.Received().HighlightBuildingSlot(_building);
            _detailsManager.Received().ShowDetails(_building);

            _aiCruiser.SlotHighlighter.DidNotReceive().HighlightBuildingSlot(_building);
        }

        [Test]
        public void SelectBuilding_ParentIsAiCruiser_ShowsDetails()
        {
            _building.ParentCruiser.Returns(_aiCruiser);

            _uiManager.SelectBuilding(_building);

            Received_HideItemDetails();

            _aiCruiser.SlotHighlighter.Received().HighlightBuildingSlot(_building);
            _detailsManager.Received().ShowDetails(_building);

            _playerCruiser.SlotHighlighter.DidNotReceive().HighlightBuildingSlot(_building);
        }
        #endregion SelectBuilding()

        #region ShowFactoryUnits
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
        #endregion ShowFactoryUnits

        [Test]
        public void ShowUnitDetails()
        {
            IUnit unit = Substitute.For<IUnit>();

            _uiManager.ShowUnitDetails(unit);

            Received_HideItemDetails();
            _detailsManager.Received().ShowDetails(unit);
        }

        [Test]
        public void ShowCruiserDetails()
        {
            _uiManager.ShowCruiserDetails(_playerCruiser);

            Received_HideItemDetails();
            _detailsManager.Received().ShowDetails(_playerCruiser);
        }

        private void Received_HideItemDetails()
        {
            _detailsManager.Received().HideDetails();
            _playerCruiser.SlotHighlighter.Received().UnhighlightSlots();
            _aiCruiser.SlotHighlighter.Received().UnhighlightSlots();
        }
    }
}
