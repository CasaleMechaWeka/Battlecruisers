using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Presentables;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class BuildingCategoriesMenu : PresentableController, IMenu
	{
        private IDictionary<BuildingCategory, IBuildingCategoryButton> _categoryToCategoryButtons;

        public void Initialise(
            IList<IBuildingGroup> buildingGroups,
            IUIManager uiManager,
            IBroadcastingFilter<BuildingCategory> shouldBeEnabledFilter)
        {
            base.Initialise();

            Helper.AssertIsNotNull(buildingGroups, uiManager, shouldBeEnabledFilter);

            _categoryToCategoryButtons = new Dictionary<BuildingCategory, IBuildingCategoryButton>();

            IList<BuildingCategoryButton> categoryButtons = GetComponentsInChildren<BuildingCategoryButton>().ToList();
            Assert.IsTrue(buildingGroups.Count <= categoryButtons.Count);

            for (int i = 0; i < buildingGroups.Count; ++i)
            {
                BuildingCategoryButton button = categoryButtons[i];

                if (i < buildingGroups.Count)
                {
                    // Have category for button
                    IBuildingGroup group = buildingGroups[i];
                    button.Initialise(group, uiManager, shouldBeEnabledFilter);
                    _categoryToCategoryButtons.Add(group.BuildingCategory, button);
                }
                else
                {
                    // Have no buildable for button (user has not unlocked it yet)
                    Destroy(button);
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
