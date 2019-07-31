using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class DismissSelectorPanelButtonController : ClickableTogglable
	{
        private IUIManager _uiManager;
        private FilterToggler _isEnabledToggler;

        private Image _closeImage;
        protected override MaskableGraphic Graphic => _closeImage;

        public void Initialise(IUIManager uiManager, IBroadcastingFilter shouldBeEnabledFilter)
		{
            base.Initialise();

            Helper.AssertIsNotNull(uiManager, shouldBeEnabledFilter);

            _uiManager = uiManager;
            _isEnabledToggler = new FilterToggler(this, shouldBeEnabledFilter);

            _closeImage = transform.FindNamedComponent<Image>("CloseImage");
		}

        protected override void OnClicked()
        {
            _uiManager.HideCurrentlyShownMenu();
        }
    }
}
