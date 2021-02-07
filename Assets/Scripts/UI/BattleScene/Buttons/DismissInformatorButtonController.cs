using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound.Players;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class DismissInformatorButtonController : DismissPanelButtonController
    {
        private FilterToggler _helpLabelVisibilityToggler;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IUIManager uiManager, 
            IBroadcastingFilter shouldBeEnabledFilter, 
            IBroadcastingFilter helpLabelVisibilityFilter)
		{
            base.Initialise(soundPlayer, uiManager, shouldBeEnabledFilter);

            Assert.IsNotNull(helpLabelVisibilityFilter);

            HelpLabel helpLabel = GetComponentInChildren<HelpLabel>();
            Assert.IsNotNull(helpLabel);
            helpLabel.Initialise();
            _helpLabelVisibilityToggler = new FilterToggler(helpLabelVisibilityFilter, helpLabel);
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _uiManager.HideItemDetails();
        }
    }
}
