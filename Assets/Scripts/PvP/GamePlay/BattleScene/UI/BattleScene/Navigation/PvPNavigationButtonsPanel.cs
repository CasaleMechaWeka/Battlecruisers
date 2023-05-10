using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation
{
    public class PvPNavigationButtonsPanel : PvPHighlightable
    {
        private PvPFilterToggler _enabledToggler;

        public PvPCanvasGroupButton overviewButton, playerCruiserButton, aiCruiserButton;

        public void Initialise(IPvPBroadcastingFilter enabledFilter, IPvPCameraFocuser cameraFocuser, IPvPSingleSoundPlayer singleSoundPlayer)
        {
            base.Initialise();

            PvPHelper.AssertIsNotNull(overviewButton, playerCruiserButton, aiCruiserButton);
            PvPHelper.AssertIsNotNull(enabledFilter, cameraFocuser);

            overviewButton.Initialise(singleSoundPlayer, cameraFocuser.FocusOnOverview);
            playerCruiserButton.Initialise(singleSoundPlayer, cameraFocuser.FocusOnPlayerCruiser);
            aiCruiserButton.Initialise(singleSoundPlayer, cameraFocuser.FocusOnAICruiser);

            _enabledToggler = new PvPFilterToggler(enabledFilter, overviewButton, playerCruiserButton, aiCruiserButton);
        }
    }
}