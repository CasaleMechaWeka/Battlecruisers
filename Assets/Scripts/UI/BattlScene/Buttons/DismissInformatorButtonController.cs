using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class DismissInformatorButtonController : Togglable, IPointerClickHandler
    {
        private IUIManager _uiManager;
        private FilterToggler _isEnabledToggler, _helpLabelVisibilityToggler;

        public void Initialise(
            IUIManager uiManager, 
            IBroadcastingFilter shouldBeEnabledFilter, 
            IBroadcastingFilter helpLabelVisibilityFilter)
		{
            Helper.AssertIsNotNull(uiManager, shouldBeEnabledFilter, helpLabelVisibilityFilter);

            _uiManager = uiManager;
            _isEnabledToggler = new FilterToggler(this, shouldBeEnabledFilter);

            HelpLabel helpLabel = GetComponentInChildren<HelpLabel>();
            Assert.IsNotNull(helpLabel);
            helpLabel.Initialise();
            _helpLabelVisibilityToggler = new FilterToggler(helpLabel, helpLabelVisibilityFilter);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _uiManager.HideItemDetails();
        }
    }
}
