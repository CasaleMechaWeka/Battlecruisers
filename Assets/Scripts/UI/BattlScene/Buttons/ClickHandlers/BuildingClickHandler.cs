using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Sound;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons.ClickHandlers
{
    public class BuildingClickHandler : BuildableClickHandler, IBuildingClickHandler
    {
        private readonly IPlayerCruiserFocusHelper _playerCruiserFocusHelper;

        public BuildingClickHandler(
            IPlayerCruiserFocusHelper playerCruiserFocusHelper, 
            IUIManager uiManager, 
            IPrioritisedSoundPlayer soundPlayer)
            : base(uiManager, soundPlayer)
        {
            Assert.IsNotNull(playerCruiserFocusHelper);
            _playerCruiserFocusHelper = playerCruiserFocusHelper;
        }

        // FELIX  Update tests :)
        public void HandleClick(bool canAffordBuildable, IBuildableWrapper<IBuilding> buildingClicked)
        {
            Assert.IsNotNull(buildingClicked);

            if (canAffordBuildable)
            {
                _uiManager.SelectBuildingFromMenu(buildingClicked);

                if (buildingClicked.Buildable.SlotSpecification.SlotType == SlotType.Bow)
                {
                    _playerCruiserFocusHelper.FocusOnPlayerNavalFactoryIfNeeded();
                }
                else
                {
                    _playerCruiserFocusHelper.FocusOnPlayerCruiserIfNeeded();
                }
            }
            else
            {
                PlayUnaffordableSound();
            }
        }
    }
}