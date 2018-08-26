using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using System.Collections.Generic;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class UnitMenus : BuildableMenus<IUnit, UnitCategory, NEWUnitsMenuController>
    {
        protected override void InitialiseMenu(
            NEWUnitsMenuController menu, 
            IList<IBuildableWrapper<IUnit>> buildables, 
            IUIManager uiManager, 
            IBroadcastingFilter<IBuildable> shouldBeEnabledFilter)
        {
            menu.Initialise(buildables, uiManager, shouldBeEnabledFilter);
        }
    }
}