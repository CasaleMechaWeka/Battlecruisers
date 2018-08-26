using BattleCruisers.Buildables;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Sorting;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public abstract class BuildableMenus<TBuildable, TCategories, TMenu> : MonoBehaviour
        where TBuildable : class, IBuildable
        where TMenu : IBuildablesMenu
    {
        private IDictionary<TCategories, IBuildablesMenu> _buildableCategoryToPanels;

        public void Initialise(
            IDictionary<TCategories, IList<IBuildableWrapper<TBuildable>>> buildables,
            IUIManager uiManager,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IBuildableSorter<TBuildable> buildableSorter)
        {
            Helper.AssertIsNotNull(buildables, uiManager, buttonVisibilityFilters, buildableSorter);

            IList<TMenu> buildableMenus = GetComponentsInChildren<TMenu>().ToList();
            Assert.AreEqual(buildables.Count, buildableMenus.Count);

            _buildableCategoryToPanels = new Dictionary<TCategories, IBuildablesMenu>();

            int i = 0;

            foreach (KeyValuePair<TCategories, IList<IBuildableWrapper<TBuildable>>> pair in buildables)
            {
                TMenu buildableMenu = buildableMenus[i];
                IList<IBuildableWrapper<TBuildable>> sortedBuildables = buildableSorter.Sort(pair.Value);
                InitialiseMenu(buildableMenu, uiManager, buttonVisibilityFilters, sortedBuildables);
                _buildableCategoryToPanels.Add(pair.Key, buildableMenu);
                i++;
            }
        }

        protected abstract void InitialiseMenu(
            TMenu menu,
            IUIManager uiManager,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IList<IBuildableWrapper<TBuildable>> buildables);

        // FELIX  Rename panel to menu
        public IBuildablesMenu GetBuildablesPanel(TCategories buildableCategory)
        {
            Assert.IsTrue(_buildableCategoryToPanels.ContainsKey(buildableCategory));
            return _buildableCategoryToPanels[buildableCategory];
        }
    }
}