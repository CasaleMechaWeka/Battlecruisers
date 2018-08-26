using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using System.Collections.Generic;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class UnitMenus : BuildableMenus<IUnit, UnitCategory, UnitsMenuController>
    {
        protected override void InitialiseMenu(
            UnitsMenuController menu, 
            IUIManager uiManager,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IList<IBuildableWrapper<IUnit>> buildables)
        {
            menu.Initialise(uiManager, buttonVisibilityFilters, buildables);
        }
    }
}