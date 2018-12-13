using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Presentables;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class BuildingCategoriesMenu : PresentableController, IBuildingCategoriesMenu
    {
        private IDictionary<BuildingCategory, IBuildingCategoryButton> _categoryToCategoryButtons;

        public void Initialise(
            IUIManager uiManager,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IList<IBuildingGroup> buildingGroups)
        {
            base.Initialise();

            Helper.AssertIsNotNull(uiManager, buttonVisibilityFilters, buildingGroups);

            _categoryToCategoryButtons = new Dictionary<BuildingCategory, IBuildingCategoryButton>();

            IList<BuildingCategoryButtonNEW> categoryButtons = GetComponentsInChildren<BuildingCategoryButtonNEW>().ToList();
            Assert.IsTrue(buildingGroups.Count <= categoryButtons.Count);

            for (int i = 0; i < buildingGroups.Count; ++i)
            {
                BuildingCategoryButtonNEW button = categoryButtons[i];
                IBuildingGroup group = buildingGroups[i];

                if (group.Buildings.Count != 0)
                {
                    // Have category for button
                    button.Initialise(group.BuildingCategory, uiManager, buttonVisibilityFilters.CategoryButtonVisibilityFilter);
                    _categoryToCategoryButtons.Add(group.BuildingCategory, button);
                }
                else
                {
                    // Have no buildable for button (user has not unlocked it yet)
                    Destroy(button.gameObject);
                }
            }
        }

        public IBuildingCategoryButton GetCategoryButton(BuildingCategory category)
        {
            Assert.IsTrue(_categoryToCategoryButtons.ContainsKey(category));
            return _categoryToCategoryButtons[category];
        }
    }
}
