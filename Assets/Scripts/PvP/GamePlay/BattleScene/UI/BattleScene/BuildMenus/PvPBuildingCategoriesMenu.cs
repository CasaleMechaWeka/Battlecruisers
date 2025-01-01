using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Presentables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.Sound.Players;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public class PvPBuildingCategoriesMenu : PvPPresentableController, IPvPBuildingCategoriesMenu
    {
        private IDictionary<BuildingCategory, IPvPBuildingCategoryButton> _categoryToCategoryButtons;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IPvPUIManager uiManager,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IList<IPvPBuildingGroup> buildingGroups)
        {
            base.Initialise();

            PvPHelper.AssertIsNotNull(soundPlayer, uiManager, buttonVisibilityFilters, buildingGroups);

            _categoryToCategoryButtons = new Dictionary<BuildingCategory, IPvPBuildingCategoryButton>();

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

        public IPvPBuildingCategoryButton GetCategoryButton(BuildingCategory category)
        {
            Assert.IsTrue(_categoryToCategoryButtons.ContainsKey(category));
            return _categoryToCategoryButtons[category];
        }
    }
}
