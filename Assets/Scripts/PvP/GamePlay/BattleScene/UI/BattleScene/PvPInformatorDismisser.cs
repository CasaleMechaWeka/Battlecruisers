using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI;
using System;

namespace BattleCruisers.Network.Multiplay.MultiplayBattleScene.UI.BattleScene
{
    public class PvPInformatorDismisser
    {
        private readonly IPvPClickableEmitter _background;
        private readonly IPvPUIManager _uiManager;

        public PvPInformatorDismisser(IPvPClickableEmitter background, IPvPUIManager uiManager)
        {
            PvPHelper.AssertIsNotNull(background, uiManager);

            _background = background;
            _uiManager = uiManager;

            _background.Clicked += _background_Clicked;
        }

        private void _background_Clicked(object sender, EventArgs e)
        {
            _uiManager.HideItemDetails();
        }
    }
}