using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.Sorting;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class UnitMenus : BuildableMenus<IUnit, UnitCategory, UnitsMenuController>
    {
        private IUnitClickHandler _clickHandler;

        public void Initialise(
            IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> buildables,
            IUIManager uiManager,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IBuildableSorter<IUnit> buildableSorter,
            ISingleSoundPlayer soundPlayer,
            IUnitClickHandler clickHandler)
        {
            // Need this for abstract method called by base.Initialise().  Codesmell :P
            Assert.IsNotNull(clickHandler);
            _clickHandler = clickHandler;

            base.Initialise(buildables, uiManager, buttonVisibilityFilters, buildableSorter, soundPlayer);
        }

        protected override void InitialiseMenu(
            ISingleSoundPlayer soundPlayer,
            UnitsMenuController menu, 
            IUIManager uiManager,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IList<IBuildableWrapper<IUnit>> buildables)
        {
            menu.Initialise(soundPlayer, uiManager, buttonVisibilityFilters, buildables, _clickHandler);
        }
    }
}