using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Sound;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.BattleScene.Buttons.ClickHandlers
{
    public class BuildingClickHandlerTests
    {
        private IBuildingClickHandler _clickHandler;
        private IPlayerCruiserFocusHelper _playerCruiserFocusHelper;
        private IUIManager _uiManager;
        private IPrioritisedSoundPlayer _soundPlayer;
        private IBuildableWrapper<IBuilding> _building;

        [SetUp]
        public void TestSetup()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _playerCruiserFocusHelper = Substitute.For<IPlayerCruiserFocusHelper>();
            _uiManager = Substitute.For<IUIManager>();
            _soundPlayer = Substitute.For<IPrioritisedSoundPlayer>();

            _clickHandler = new BuildingClickHandler(_playerCruiserFocusHelper, _uiManager, _soundPlayer);

            _building = Substitute.For<IBuildableWrapper<IBuilding>>();
        }

        [Test]
        public void HandleClick_BuildingNull_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _clickHandler.HandleClick(canAffordBuildable: true, buildingClicked: null));
        }

        [Test]
        public void HandleClick_CanAffordBuliding_UpdatesUI_NotBowSlot_FocusesOnCruiser()
        {
            bool canAffordBuilding = true;
            SlotSpecification nonBowSlotSpecification = new SlotSpecification(SlotType.Deck, default, default);
            _building.Buildable.SlotSpecification.Returns(nonBowSlotSpecification);

            _clickHandler.HandleClick(canAffordBuilding, _building);

            _playerCruiserFocusHelper.Received().FocusOnPlayerCruiserIfNeeded();
            _uiManager.Received().SelectBuildingFromMenu(_building);
        }

        [Test]
        public void HandleClick_CanAffordBuliding_UpdatesUI_BowSlot_FocusesOnPlayerNavalFactory()
        {
            bool canAffordBuilding = true;
            SlotSpecification bowSlotSpecification = new SlotSpecification(SlotType.Bow, default, default);
            _building.Buildable.SlotSpecification.Returns(bowSlotSpecification);

            _clickHandler.HandleClick(canAffordBuilding, _building);

            _playerCruiserFocusHelper.Received().FocusOnPlayerBowSlotIfNeeded();
            _uiManager.Received().SelectBuildingFromMenu(_building);
        }

        [Test]
        public void HandleClick_CannotAffordBuilding_PlaysInsufficientFundsSound()
        {
            bool canAffordBuilding = false;
            _clickHandler.HandleClick(canAffordBuilding, _building);
            _soundPlayer.Received().PlaySound(PrioritisedSoundKeys.Events.Drones.NotEnoughDronesToBuild);
        }
    }
}