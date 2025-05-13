using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons
{
    public abstract class PvPDismissPanelButtonController : PvPElementWithClickSound
    {
        private FilterToggler _isEnabledToggler;
        protected PvPUIManager _uiManager;

        private Image _closeImage;
        protected override MaskableGraphic Graphic => _closeImage;

        public void Initialise(ISingleSoundPlayer soundPlayer, PvPUIManager uiManager, IBroadcastingFilter shouldBeEnabledFilter)
        {
            base.Initialise(soundPlayer);

            PvPHelper.AssertIsNotNull(uiManager, shouldBeEnabledFilter);

            _uiManager = uiManager;
            _isEnabledToggler = new FilterToggler(shouldBeEnabledFilter, this);

            _closeImage = transform.FindNamedComponent<Image>("CloseImage");
        }
    }
}
