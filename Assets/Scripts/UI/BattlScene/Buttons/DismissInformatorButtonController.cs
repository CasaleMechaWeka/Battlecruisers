using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class DismissInformatorButtonController : DismissPanelButtonController
    {
        private FilterToggler _helpLabelVisibilityToggler;

        public void Initialise(
            IUIManager uiManager, 
            IBroadcastingFilter shouldBeEnabledFilter, 
            IBroadcastingFilter helpLabelVisibilityFilter)
		{
            base.Initialise(uiManager, shouldBeEnabledFilter);

            Assert.IsNotNull(helpLabelVisibilityFilter);

            HelpLabel helpLabel = GetComponentInChildren<HelpLabel>();
            Assert.IsNotNull(helpLabel);
            helpLabel.Initialise();
            _helpLabelVisibilityToggler = new FilterToggler(helpLabel, helpLabelVisibilityFilter);
        }

        protected override void OnClicked()
        {
            _uiManager.HideItemDetails();
        }
    }
}
