using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.BattleScene.Buttons.ClickHandlers
{
    public class BuildingClickHandlerTests
    {
        private IBuildingClickHandler _clickHandler;
        private IUIManager _uiManager;
        private IPrioritisedSoundPlayer _eventSoundPlayer;
        private ISingleSoundPlayer _uiSoundPlayer;
        private IPlayerCruiserFocusHelper _playerCruiserFocusHelper;
        private IAudioClipWrapper _selectedSound;
        private IBuildableWrapper<IBuilding> _building;

        [SetUp]
        public void TestSetup()
        {
            _uiManager = Substitute.For<IUIManager>();
            _eventSoundPlayer = Substitute.For<IPrioritisedSoundPlayer>();
            _uiSoundPlayer = Substitute.For<ISingleSoundPlayer>();
            _playerCruiserFocusHelper = Substitute.For<IPlayerCruiserFocusHelper>();
            _selectedSound = Substitute.For<IAudioClipWrapper>();

            _clickHandler = new BuildingClickHandler(_uiManager, _eventSoundPlayer, _uiSoundPlayer, _playerCruiserFocusHelper, _selectedSound);

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
            _building.Buildable.SlotSpecification.SlotType.Returns(SlotType.Deck);

            _clickHandler.HandleClick(canAffordBuilding, _building);

            _uiSoundPlayer.Received().PlaySound(_selectedSound);
            _playerCruiserFocusHelper.Received().FocusOnPlayerCruiserIfNeeded();
            _uiManager.Received().SelectBuildingFromMenu(_building);
        }

        [Test]
        public void HandleClick_CanAffordBuliding_UpdatesUI_BowSlot_FocusesOnPlayerNavalFactory()
        {
            bool canAffordBuilding = true;
            _building.Buildable.SlotSpecification.SlotType.Returns(SlotType.Bow);

            _clickHandler.HandleClick(canAffordBuilding, _building);

            _uiSoundPlayer.Received().PlaySound(_selectedSound);
            _playerCruiserFocusHelper.Received().FocusOnPlayerBowSlotIfNeeded();
            _uiManager.Received().SelectBuildingFromMenu(_building);
        }

        [Test]
        public void HandleClick_CanAffordBuliding_UpdatesUI_AntiShipBuilding_FocusesOnPlayerNavalFactory()
        {
            bool canAffordBuilding = true;
            _building.Buildable.SlotSpecification.SlotType.Returns(SlotType.Deck);
            _building.Buildable.SlotSpecification.BuildingFunction.Returns(BuildingFunction.AntiShip);

            _clickHandler.HandleClick(canAffordBuilding, _building);

            _uiSoundPlayer.Received().PlaySound(_selectedSound);
            _playerCruiserFocusHelper.Received().FocusOnPlayerBowSlotIfNeeded();
            _uiManager.Received().SelectBuildingFromMenu(_building);
        }

        [Test]
        public void HandleClick_CannotAffordBuilding_UpdatesUI_PlaysInsufficientFundsSound()
        {
            bool canAffordBuilding = false;

            _clickHandler.HandleClick(canAffordBuilding, _building);

            _uiManager.Received().SelectBuilding(_building.Buildable);
            _uiSoundPlayer.Received().PlaySound(_selectedSound);
            _eventSoundPlayer.Received().PlaySound(PrioritisedSoundKeys.Events.Drones.NotEnoughDronesToBuild);
        }
    }
}