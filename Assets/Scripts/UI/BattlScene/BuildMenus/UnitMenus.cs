using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Presentables;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Sorting;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    // FELIX  Avoid duplicate code with BulidingMenus?
    public class UnitMenus : MonoBehaviour
    {
        private IDictionary<UnitCategory, PresentableController> _unitGroupPanels;

        public void Initialise(
            IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> units,
            IUIManager uiManager,
            IBroadcastingFilter<IBuildable> shouldBeEnabledFilter,
            IUnitClickHandler unitClickHandler,
            IBuildableSorter<IUnit> unitSorter)
        {
            Helper.AssertIsNotNull(units, uiManager, shouldBeEnabledFilter, unitClickHandler);

            IList<NEWUnitsMenuController> unitMenus = GetComponentsInChildren<NEWUnitsMenuController>().ToList();
            Assert.AreEqual(units.Count, unitMenus.Count);

            _unitGroupPanels = new Dictionary<UnitCategory, PresentableController>();

            int i = 0;

            foreach (KeyValuePair<UnitCategory, IList <IBuildableWrapper<IUnit>>> pair in units)
            {
                NEWUnitsMenuController unitMenu = unitMenus[i];
                IList<IBuildableWrapper<IUnit>> sortedUnits = unitSorter.Sort(pair.Value);
                unitMenu.Initialise(sortedUnits, uiManager, shouldBeEnabledFilter, unitClickHandler);
                _unitGroupPanels.Add(pair.Key, unitMenu);
                i++;
            }
        }

        public PresentableController GetUnitsPanel(UnitCategory unitCategory)
        {
            Assert.IsTrue(_unitGroupPanels.ContainsKey(unitCategory));
            return _unitGroupPanels[unitCategory];
        }
    }
}