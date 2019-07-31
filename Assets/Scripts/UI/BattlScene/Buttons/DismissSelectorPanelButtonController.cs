using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class DismissSelectorPanelButtonController : DismissPanelButtonController
    {
        public override void Initialise(IUIManager uiManager, IBroadcastingFilter shouldBeEnabledFilter)
		{
            base.Initialise(uiManager, shouldBeEnabledFilter);
		}

        protected override void OnClicked()
        {
            _uiManager.HideCurrentlyShownMenu();
        }
    }
}
