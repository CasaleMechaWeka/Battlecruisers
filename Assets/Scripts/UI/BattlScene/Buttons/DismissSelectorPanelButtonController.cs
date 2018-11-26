using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    // TUTORIAL  Do I even need to disable this button?
    public class DismissSelectorPanelButtonController : Togglable, IPointerClickHandler
	{
        private IUIManager _uiManager;
        // FELIX  Common base class for all FilterToggler users?
        private FilterToggler _filterToggler;

        // FELIX  Use FilterToggle :D
        public void Initialise(IUIManager uiManager, IBroadcastingFilter shouldBeEnabledFilter)
		{
            Helper.AssertIsNotNull(uiManager, shouldBeEnabledFilter);

            _uiManager = uiManager;
            _filterToggler = new FilterToggler(this, shouldBeEnabledFilter);
		}

        public void OnPointerClick(PointerEventData eventData)
        {
            _uiManager.HideCurrentlyShownMenu();
        }
    }
}
