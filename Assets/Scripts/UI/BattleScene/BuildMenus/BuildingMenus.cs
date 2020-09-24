using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.Sorting;
using System.Collections.Generic;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class BuildingMenus : BuildableMenus<IBuilding, BuildingCategory, BuildingsMenuController>
    {
        private ISpriteProvider _spriteProvider;
        private IBuildingClickHandler _clickHandler;

        public void Initialise(
            IDictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> buildings,
            IUIManager uiManager,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IBuildableSorter<IBuilding> buildingSorter,
            
            ISpriteProvider spriteProvider,
            ISingleSoundPlayer soundPlayer,
            IBuildingClickHandler clickHandler)
        {
            // Need these for abstract method called by base.Initialise().  Codesmell :P
            Helper.AssertIsNotNull(spriteProvider, clickHandler);

            _spriteProvider = spriteProvider;
            _clickHandler = clickHandler;

            base.Initialise(buildings, uiManager, buttonVisibilityFilters, buildingSorter, soundPlayer);
        }

        protected override void InitialiseMenu(
            ISingleSoundPlayer soundPlayer,
            BuildingsMenuController menu, 
            IUIManager uiManager,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IList<IBuildableWrapper<IBuilding>> buildables)
        {
            menu.Initialise(soundPlayer, uiManager, buttonVisibilityFilters, buildables, _spriteProvider, _clickHandler);
        }
    }
}