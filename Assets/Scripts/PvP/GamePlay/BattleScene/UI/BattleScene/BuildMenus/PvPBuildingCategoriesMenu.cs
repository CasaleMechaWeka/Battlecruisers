using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Presentables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.Sound.Players;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public class PvPBuildingCategoriesMenu : PvPPresentableController, IBuildingCategoriesMenu
    {
        private IDictionary<BuildingCategory, IBuildingCategoryButton> _categoryToCategoryButtons;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            PvPUIManager uiManager,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IList<IPvPBuildingGroup> buildingGroups)
        {
            base.Initialise();

            PvPHelper.AssertIsNotNull(soundPlayer, uiManager, buttonVisibilityFilters, buildingGroups);

            _categoryToCategoryButtons = new Dictionary<BuildingCategory, IBuildingCategoryButton>();

            IList<PvPBuildingCategoryButton> categoryButtons = GetComponentsInChildren<PvPBuildingCategoryButton>().ToList();
            Assert.IsTrue(buildingGroups.Count <= categoryButtons.Count);

            for (int i = 0; i < buildingGroups.Count; ++i)
            {
                PvPBuildingCategoryButton button = categoryButtons[i];
                IPvPBuildingGroup group = buildingGroups[i];

                if (group.Buildings.Count != 0)
                {
                    // Have category for button
                    button
                        .Initialise(
                            soundPlayer,
                            group.BuildingCategory,
                            uiManager,
                            buttonVisibilityFilters.CategoryButtonVisibilityFilter);
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
