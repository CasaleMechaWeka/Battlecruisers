using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons
{
    public abstract class PvPDismissPanelButtonController : PvPElementWithClickSound
    {
        private PvPFilterToggler _isEnabledToggler;
        protected IPvPUIManager _uiManager;

        private Image _closeImage;
        protected override MaskableGraphic Graphic => _closeImage;

        public void Initialise(IPvPSingleSoundPlayer soundPlayer, IPvPUIManager uiManager, IPvPBroadcastingFilter shouldBeEnabledFilter)
        {
            base.Initialise(soundPlayer);

            PvPHelper.AssertIsNotNull(uiManager, shouldBeEnabledFilter);

            _uiManager = uiManager;
            _isEnabledToggler = new PvPFilterToggler(shouldBeEnabledFilter, this);

            _closeImage = transform.FindNamedComponent<Image>("CloseImage");
        }
    }
}
