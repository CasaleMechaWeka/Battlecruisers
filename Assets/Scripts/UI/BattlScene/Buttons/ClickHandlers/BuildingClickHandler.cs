using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Sound;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons.ClickHandlers
{
    // FELIX  Handle tutorial???
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

        public void HandleClick(bool canAffordBuildable, IBuildableWrapper<IBuilding> buildingClicked)
        {
            Assert.IsNotNull(buildingClicked);

            if (canAffordBuildable)
            {
                _playerCruiserFocusHelper.FocusOnPlayerCruiserIfNeeded();
                _uiManager.SelectBuildingFromMenu(buildingClicked);
            }
            else
            {
                PlayUnaffordableSound();
            }
        }
    }
}