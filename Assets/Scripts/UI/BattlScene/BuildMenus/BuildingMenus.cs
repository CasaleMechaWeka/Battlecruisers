using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Sorting;
using System.Collections.Generic;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class BuildingMenus : BuildableMenus<IBuilding, BuildingCategory, BuildingsMenuController>
    {
        private ISpriteProvider _spriteProvider;
        private IPlayerCruiserFocusHelper _playerCruiserFocusHelper;

        public void Initialise(
            IDictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> buildings,
            IUIManager uiManager,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IBuildableSorter<IBuilding> buildingSorter,
            ISpriteProvider spriteProvider,
            IPlayerCruiserFocusHelper playerCruiserFocusHelper)
        {
            // Need these for abstract method called by base.Initialise().  Codesmell :P
            Helper.AssertIsNotNull(spriteProvider, playerCruiserFocusHelper);

            _spriteProvider = spriteProvider;
            _playerCruiserFocusHelper = playerCruiserFocusHelper;

            base.Initialise(buildings, uiManager, buttonVisibilityFilters, buildingSorter);
        }

        protected override void InitialiseMenu(
            BuildingsMenuController menu, 
            IUIManager uiManager,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IList<IBuildableWrapper<IBuilding>> buildables)
        {
            menu.Initialise(uiManager, buttonVisibilityFilters, buildables, _spriteProvider, _playerCruiserFocusHelper);
        }
    }
}