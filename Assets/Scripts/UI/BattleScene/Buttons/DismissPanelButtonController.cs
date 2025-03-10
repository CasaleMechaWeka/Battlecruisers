using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public abstract class DismissPanelButtonController : ElementWithClickSound
    {
        private FilterToggler _isEnabledToggler;
        protected IUIManager _uiManager;

        private Image _closeImage;
        protected override MaskableGraphic Graphic => _closeImage;

        public void Initialise(ISingleSoundPlayer soundPlayer, IUIManager uiManager, IBroadcastingFilter shouldBeEnabledFilter)
        {
            base.Initialise(soundPlayer);

            Helper.AssertIsNotNull(uiManager, shouldBeEnabledFilter);

            _uiManager = uiManager;
            _isEnabledToggler = new FilterToggler(shouldBeEnabledFilter, this);

            _closeImage = transform.FindNamedComponent<Image>("CloseImage");
        }
    }
}
