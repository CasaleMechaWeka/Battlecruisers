using BattleCruisers.Buildables;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Sorting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public abstract class BuildableMenus<TBuildable, TCategories, TMenu> : MonoBehaviour, IBuildableMenus<TCategories>
        where TBuildable : class, IBuildable
        where TMenu : MonoBehaviour, IBuildablesMenu
    {
        private IDictionary<TCategories, IBuildablesMenu> _buildableCategoryToMenus;

        public IReadOnlyCollection<IBuildablesMenu> Menus { get; private set; }

        public void Initialise(
            IDictionary<TCategories, IList<IBuildableWrapper<TBuildable>>> buildables,
            IUIManager uiManager,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IBuildableSorter<TBuildable> buildableSorter)
        {
            Helper.AssertIsNotNull(buildables, uiManager, buttonVisibilityFilters, buildableSorter);

            IList<TMenu> buildableMenus = GetComponentsInChildren<TMenu>().ToList();
            Assert.AreEqual(buildables.Count, buildableMenus.Count);

            _buildableCategoryToMenus = new Dictionary<TCategories, IBuildablesMenu>();

            int i = 0;

            foreach (KeyValuePair<TCategories, IList<IBuildableWrapper<TBuildable>>> pair in buildables)
            {
                TMenu buildableMenu = buildableMenus[i];

                if (pair.Value.Count != 0)
                {
                    IList<IBuildableWrapper<TBuildable>> sortedBuildables = buildableSorter.Sort(pair.Value);
                    InitialiseMenu(buildableMenu, uiManager, buttonVisibilityFilters, sortedBuildables);
                    _buildableCategoryToMenus.Add(pair.Key, buildableMenu);
                    buildableMenu.IsVisible = false;
                }
                else
                {
                    Destroy(buildableMenu.gameObject);
                }

                i++;
            }

            Menus = new ReadOnlyCollection<IBuildablesMenu>(_buildableCategoryToMenus.Values.ToList());
        }

        protected abstract void InitialiseMenu(
            TMenu menu,
            IUIManager uiManager,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IList<IBuildableWrapper<TBuildable>> buildables);

        public IBuildablesMenu GetBuildablesMenu(TCategories buildableCategory)
        {
            Assert.IsTrue(_buildableCategoryToMenus.ContainsKey(buildableCategory));
            return _buildableCategoryToMenus[buildableCategory];
        }
    }
}