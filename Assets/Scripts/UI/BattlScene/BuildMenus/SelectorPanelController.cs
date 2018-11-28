using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class SelectorPanelController : Panel
    {
        public void Initialise(IUIManager uiManager, IBroadcastingFilter shouldDismissButtonBeEnabledFilter)
        {
            DismissSelectorPanelButtonController dismissButton = GetComponentInChildren<DismissSelectorPanelButtonController>();
            Assert.IsNotNull(dismissButton);
            dismissButton.Initialise(uiManager, shouldDismissButtonBeEnabledFilter);
        }
    }
}