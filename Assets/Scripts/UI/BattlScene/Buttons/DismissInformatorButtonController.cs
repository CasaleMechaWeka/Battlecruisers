using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class DismissInformatorButtonController : Togglable, IPointerClickHandler
    {
        private IUIManager _uiManager;
        private FilterToggler _isEnabledToggler;

        public void Initialise(IUIManager uiManager, IBroadcastingFilter shouldBeEnabledFilter)
		{
            Helper.AssertIsNotNull(uiManager, shouldBeEnabledFilter);

            _uiManager = uiManager;
            _isEnabledToggler = new FilterToggler(this, shouldBeEnabledFilter);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _uiManager.HideItemDetails();
        }
    }
}
