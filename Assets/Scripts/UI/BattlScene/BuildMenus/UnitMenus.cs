using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils.Sorting;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class UnitMenus : BuildableMenus<IUnit, UnitCategory, NEWUnitsMenuController>
    {
        private IUnitClickHandler _unitClickHandler;

        public void Initialise(
            IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> units,
            IUIManager uiManager,
            IBroadcastingFilter<IBuildable> shouldBeEnabledFilter,
            IBuildableSorter<IUnit> unitSorter,
            IUnitClickHandler unitClickHandler)
        {
            // Need unitClickHandler in abstract method called from parent class Initialise().  Codesmell :(
            Assert.IsNotNull(unitClickHandler);
            _unitClickHandler = unitClickHandler;

            base.Initialise(units, uiManager, shouldBeEnabledFilter, unitSorter);
        }

        protected override void InitialiseMenu(
            NEWUnitsMenuController menu, 
            IList<IBuildableWrapper<IUnit>> buildables, 
            IUIManager uiManager, 
            IBroadcastingFilter<IBuildable> shouldBeEnabledFilter)
        {
            menu.Initialise(buildables, uiManager, shouldBeEnabledFilter, _unitClickHandler);
        }
    }
}