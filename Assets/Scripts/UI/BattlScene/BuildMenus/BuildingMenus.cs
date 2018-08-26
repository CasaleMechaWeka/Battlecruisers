using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Presentables;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Sorting;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    // FELIX  Avoid duplicate code with UnitMenus?
    public class BuildingMenus : MonoBehaviour
    {
        private IDictionary<BuildingCategory, PresentableController> _buildingCategoryPanels;

        public void Initialise(
            IDictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> buildings,
            IUIManager uiManager,
            IBroadcastingFilter<IBuildable> shouldBeEnabledFilter,
            ISpriteProvider spriteProvider,
            IBuildableSorter<IBuilding> buildingSorter)
        {
            Helper.AssertIsNotNull(buildings, uiManager, shouldBeEnabledFilter, spriteProvider, buildingSorter);

            IList<NEWBuildingsMenuController> buildingMenus = GetComponentsInChildren<NEWBuildingsMenuController>().ToList();
            Assert.AreEqual(buildings.Count, buildingMenus.Count);

            _buildingCategoryPanels = new Dictionary<BuildingCategory, PresentableController>();

            int i = 0;

            foreach (KeyValuePair<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> pair in buildings)
            {
                NEWBuildingsMenuController buildingMenu = buildingMenus[i];
                IList<IBuildableWrapper<IBuilding>> sortedBuildings = buildingSorter.Sort(pair.Value);
                buildingMenu.Initialise(sortedBuildings, uiManager, shouldBeEnabledFilter, spriteProvider);
                _buildingCategoryPanels.Add(pair.Key, buildingMenu);
                i++;
            }
        }

        public PresentableController GetBuildingsPanel(BuildingCategory buildingCategory)
        {
            Assert.IsTrue(_buildingCategoryPanels.ContainsKey(buildingCategory));
            return _buildingCategoryPanels[buildingCategory];
        }
    }
}