using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class SelectorPanelController : Panel
    {
        // Keep reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private FilterToggler _helpLabelsVisibilityToggler;
#pragma warning restore CS0414  // Variable is assigned but never used

        public void Initialise(IUIManager uiManager, IButtonVisibilityFilters buttonVisibilityFilters)
        {
            Helper.AssertIsNotNull(uiManager, buttonVisibilityFilters);

            DismissSelectorPanelButtonController dismissButton = GetComponentInChildren<DismissSelectorPanelButtonController>();
            Assert.IsNotNull(dismissButton);
            dismissButton.Initialise(uiManager, buttonVisibilityFilters.DismissButtonVisibilityFilter);

            HelpLabel helpLabel = transform.GetComponentInChildren<HelpLabel>();
            Assert.IsNotNull(helpLabel);
            helpLabel.Initialise();
            _helpLabelsVisibilityToggler = new FilterToggler(helpLabel, buttonVisibilityFilters.HelpLabelsVisibilityFilter);
        }
    }
}