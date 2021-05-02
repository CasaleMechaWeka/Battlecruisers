using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Panels;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class SelectorPanelController : SlidingPanel
    {
        public void Initialise(IUIManager uiManager, IButtonVisibilityFilters buttonVisibilityFilters, ISingleSoundPlayer soundPlayer)
        {
            base.Initialise();

            Helper.AssertIsNotNull(uiManager, buttonVisibilityFilters);

            DismissSelectorPanelButtonController dismissButton = GetComponentInChildren<DismissSelectorPanelButtonController>();
            Assert.IsNotNull(dismissButton);
            dismissButton.Initialise(soundPlayer, uiManager, buttonVisibilityFilters.DismissButtonVisibilityFilter);
        }
    }
}