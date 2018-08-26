using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Presentables;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public abstract class Menu : PresentableController, IMenu
    {
        public void Initialise(
            IUIManager uiManager,
            IButtonVisibilityFilters buttonVisibilityFilters)
        {
            base.Initialise();

            Helper.AssertIsNotNull(uiManager, buttonVisibilityFilters);

            BackButtonController backButton = GetComponentInChildren<BackButtonController>();
            Assert.IsNotNull(backButton);
            backButton.Initialise(uiManager, buttonVisibilityFilters.BackButtonVisibilityFilter);
        }
    }
}