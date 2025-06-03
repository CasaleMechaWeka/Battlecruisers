using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Sorting;
using System.Collections.Generic;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class BuildingMenus : BuildableMenus<IBuilding, BuildingCategory, BuildingsMenuController>
    {
        private BuildingClickHandler _clickHandler;

        public void Initialise(
            IDictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> buildings,
            UIManager uiManager,
            ButtonVisibilityFilters buttonVisibilityFilters,
            IBuildableSorter<IBuilding> buildingSorter,
            SingleSoundPlayer soundPlayer,
            BuildingClickHandler clickHandler)
        {
            // Need these for abstract method called by base.Initialise().  Codesmell :P
            Helper.AssertIsNotNull(clickHandler);

            _clickHandler = clickHandler;

            base.Initialise(buildings, uiManager, buttonVisibilityFilters, buildingSorter, soundPlayer);
        }

        protected override void InitialiseMenu(
            SingleSoundPlayer soundPlayer,
            BuildingsMenuController menu,
            UIManager uiManager,
            ButtonVisibilityFilters buttonVisibilityFilters,
            IList<IBuildableWrapper<IBuilding>> buildings)
        {
            menu.Initialise(soundPlayer, uiManager, buttonVisibilityFilters, buildings, _clickHandler);
        }
    }
}