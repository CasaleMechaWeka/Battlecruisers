using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class SelectorPanelController : MonoBehaviour, IPanel
    {
        public void Initialise(IUIManager uiManager, IBroadcastingFilter shouldDismissButtonBeEnabledFilter)
        {
            DismissSelectorPanelButtonController dismissButton = GetComponentInChildren<DismissSelectorPanelButtonController>();
            Assert.IsNotNull(dismissButton);
            dismissButton.Initialise(uiManager, shouldDismissButtonBeEnabledFilter);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}