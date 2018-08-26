using BattleCruisers.Buildables;
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
    public abstract class BuildableMenus<TBuildable, TCategories, TBuildableMenuController> : MonoBehaviour
        where TBuildable : class, IBuildable
        where TBuildableMenuController : PresentableController
    {
        private IDictionary<TCategories, PresentableController> _buildableCategoryToPanels;

        public void Initialise(
            IDictionary<TCategories, IList<IBuildableWrapper<TBuildable>>> buildables,
            IUIManager uiManager,
            IBroadcastingFilter<IBuildable> shouldBeEnabledFilter,
            IBuildableSorter<TBuildable> buildableSorter)
        {
            Helper.AssertIsNotNull(buildables, uiManager, shouldBeEnabledFilter, buildableSorter);

            IList<TBuildableMenuController> buildableMenus = GetComponentsInChildren<TBuildableMenuController>().ToList();
            Assert.AreEqual(buildables.Count, buildableMenus.Count);

            _buildableCategoryToPanels = new Dictionary<TCategories, PresentableController>();

            int i = 0;

            foreach (KeyValuePair<TCategories, IList<IBuildableWrapper<TBuildable>>> pair in buildables)
            {
                TBuildableMenuController buildableMenu = buildableMenus[i];
                IList<IBuildableWrapper<TBuildable>> sortedBuildables = buildableSorter.Sort(pair.Value);
                InitialiseMenu(buildableMenu, sortedBuildables, uiManager, shouldBeEnabledFilter);
                _buildableCategoryToPanels.Add(pair.Key, buildableMenu);
                i++;
            }
        }

        protected abstract void InitialiseMenu(
            TBuildableMenuController menu,
            IList<IBuildableWrapper<TBuildable>> buildables,
            IUIManager uiManager,
            IBroadcastingFilter<IBuildable> shouldBeEnabledFilter);

        public PresentableController GetBuildingsPanel(TCategories buildableCategory)
        {
            Assert.IsTrue(_buildableCategoryToPanels.ContainsKey(buildableCategory));
            return _buildableCategoryToPanels[buildableCategory];
        }
    }
}