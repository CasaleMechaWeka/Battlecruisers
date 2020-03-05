using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Sound;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.BattleScene.Manager
{
    public class UIManagerTests
    {
        private UIManager _uiManager;

        private ICruiser _playerCruiser, _aiCruiser;
        private IBuildMenu _buildMenu;
        private IItemDetailsManager _detailsManager;
        private IPrioritisedSoundPlayer _soundPlayer;
        private IBuilding _building;
        private IFactory _factory;
        private IUnit _unit;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _playerCruiser = CreateMockCruiser();
            _aiCruiser = CreateMockCruiser();
            _buildMenu = Substitute.For<IBuildMenu>();
            _detailsManager = Substitute.For<IItemDetailsManager>();
            _soundPlayer = Substitute.For<IPrioritisedSoundPlayer>();

            ManagerArgs managerArgs
                = new ManagerArgs(
                    _playerCruiser,
                    _aiCruiser,
                    _buildMenu,
                    _detailsManager,
                    _soundPlayer);
            _uiManager = new UIManager();
            _uiManager.Initialise(managerArgs);

            _building = Substitute.For<IBuilding>();
            SlotSpecification slotSpecification = new SlotSpecification(SlotType.Platform, default, default);
            _building.SlotSpecification.Returns(slotSpecification);

            _factory = Substitute.For<IFactory>();

            _unit = Substitute.For<IUnit>();
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
        public void SelectBuildingFromMenu_SlotsAvailable()
        {
            IBuildableWrapper<IBuilding> buildingWrapper = Substitute.For<IBuildableWrapper<IBuilding>>();
            buildingWrapper.Buildable.Returns(_building);
            _playerCruiser.SlotHighlighter.HighlightAvailableSlots(buildingWrapper.Buildable.SlotSpecification).Returns(true);

            _uiManager.SelectBuildingFromMenu(buildingWrapper);

            Assert.AreSame(buildingWrapper, _playerCruiser.SelectedBuildingPrefab);
            _playerCruiser.SlotHighlighter.Received().HighlightAvailableSlots(buildingWrapper.Buildable.SlotSpecification);
            _detailsManager.Received().ShowDetails(buildingWrapper.Buildable);
            _soundPlayer.DidNotReceiveWithAnyArgs().PlaySound(default);
        }

        [Test]
        public void SelectBuildingFromMenu_NoSlotsAvailable()
        {
            IBuildableWrapper<IBuilding> buildingWrapper = Substitute.For<IBuildableWrapper<IBuilding>>();
            buildingWrapper.Buildable.Returns(_building);
            _playerCruiser.SlotHighlighter.HighlightAvailableSlots(buildingWrapper.Buildable.SlotSpecification).Returns(false);

            _uiManager.SelectBuildingFromMenu(buildingWrapper);

            Assert.AreSame(buildingWrapper, _playerCruiser.SelectedBuildingPrefab);
            _playerCruiser.SlotHighlighter.Received().HighlightAvailableSlots(buildingWrapper.Buildable.SlotSpecification);
            _detailsManager.Received().ShowDetails(buildingWrapper.Buildable);
            _soundPlayer.Received().PlaySound(PrioritisedSoundKeys.Events.Cruiser.NoBuildingSlotsLeft);
        }


        #region SelectBuilding()
        [Test]
        public void SelectBuilding()
        {
            _uiManager.SelectBuilding(_building);
            Received_HideItemDetails();
            _detailsManager.Received().ShowDetails(_building);
        }

        [Test]
        public void SelectBuilding_Destroyed_HidesDetails()
        {
            // Select building
            _building.ParentCruiser.Returns(_playerCruiser);
            _uiManager.SelectBuilding(_building);
            ClearHideItemDetails();

            // Building destroyed
            _building.Destroyed += Raise.EventWith(new DestroyedEventArgs(_building));
            Received_HideItemDetails();
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
            _uiManager.ShowUnitDetails(_unit);

            Received_HideItemDetails();
            _detailsManager.Received().ShowDetails(_unit);
        }

        [Test]
        public void ShowUnitDetails_Destroyed_HidesDetails()
        {
            // Show unit
            _uiManager.ShowUnitDetails(_unit);
            ClearHideItemDetails();

            // Unit destroyed
            _unit.Destroyed += Raise.EventWith(new DestroyedEventArgs(_unit));
            Received_HideItemDetails();
        }

        [Test]
        public void ShowCruiserDetails()
        {
            _uiManager.ShowCruiserDetails(_playerCruiser);

            Received_HideItemDetails();
            _detailsManager.Received().ShowDetails(_playerCruiser);
        }

        [Test]
        public void ShowCruiserDetails_Destroyed_HidesDetails()
        {
            // Show cruiser
            _uiManager.ShowCruiserDetails(_playerCruiser);
            ClearHideItemDetails();

            // Cruiser destroyed
            _playerCruiser.Destroyed += Raise.EventWith(new DestroyedEventArgs(_playerCruiser));
            Received_HideItemDetails();
        }

        private void Received_HideItemDetails()
        {
            _detailsManager.Received().HideDetails();
            _playerCruiser.SlotHighlighter.Received().UnhighlightSlots();
            _aiCruiser.SlotHighlighter.Received().UnhighlightSlots();
        }

        private void ClearHideItemDetails()
        {
            _detailsManager.ClearReceivedCalls();
            _playerCruiser.SlotHighlighter.ClearReceivedCalls();
            _aiCruiser.SlotHighlighter.ClearReceivedCalls();
        }
    }
}
