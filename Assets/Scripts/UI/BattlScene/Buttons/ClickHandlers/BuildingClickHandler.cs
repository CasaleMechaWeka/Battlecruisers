using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Buttons.ClickHandlers
{
    // FELIX  Test
    public class BuildingClickHandler : IBuildingClickHandler
    {
        private readonly IPlayerCruiserFocusHelper _playerCruiserFocusHelper;
        private readonly IUIManager _uiManager;

        public BuildingClickHandler(IPlayerCruiserFocusHelper playerCruiserFocusHelper, IUIManager uiManager)
        {
            Helper.AssertIsNotNull(playerCruiserFocusHelper, uiManager);

            _playerCruiserFocusHelper = playerCruiserFocusHelper;
            _uiManager = uiManager;
        }

        public void HandleClick(IBuildableWrapper<IBuilding> buildingClicked)
        {
            _playerCruiserFocusHelper.FocusOnPlayerCruiserIfNeeded();
            _uiManager.SelectBuildingFromMenu(buildingClicked);
        }
    }
}