using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public abstract class DismissPanelButtonController : ClickableTogglable
    {
        private FilterToggler _isEnabledToggler;
        protected IUIManager _uiManager;

        private Image _closeImage;
        protected override MaskableGraphic Graphic => _closeImage;

        public virtual void Initialise(IUIManager uiManager, IBroadcastingFilter shouldBeEnabledFilter)
        {
            base.Initialise();

            Helper.AssertIsNotNull(uiManager, shouldBeEnabledFilter);

            _uiManager = uiManager;
            _isEnabledToggler = new FilterToggler(this, shouldBeEnabledFilter);

            _closeImage = transform.FindNamedComponent<Image>("CloseImage");
        }
    }
}
