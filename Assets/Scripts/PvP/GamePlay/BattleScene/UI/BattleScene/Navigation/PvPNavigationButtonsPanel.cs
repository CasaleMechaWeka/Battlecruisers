using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation
{
    public class PvPNavigationButtonsPanel : PvPHighlightable
    {
        private PvPFilterToggler _enabledToggler;

        public PvPCanvasGroupButton overviewButton, leftPlayerCruiserButton, rightPlayerCruiserButton;
        public PvPCanvasGroupButton leftCaptainButton, rightCaptainButton;

        public void Initialise(IPvPBroadcastingFilter enabledFilter, IPvPCameraFocuser cameraFocuser, IPvPSingleSoundPlayer singleSoundPlayer)
        {
            base.Initialise();

            PvPHelper.AssertIsNotNull(overviewButton, leftPlayerCruiserButton, rightPlayerCruiserButton);
            PvPHelper.AssertIsNotNull(leftCaptainButton, rightCaptainButton);
            PvPHelper.AssertIsNotNull(enabledFilter, cameraFocuser);

            overviewButton.Initialise(singleSoundPlayer, cameraFocuser.FocusOnOverview);
            leftPlayerCruiserButton.Initialise(singleSoundPlayer, cameraFocuser.FocusOnLeftPlayerCruiser);
            rightPlayerCruiserButton.Initialise(singleSoundPlayer, cameraFocuser.FocusOnRightPlayerCruiser);

            leftCaptainButton.Initialise(singleSoundPlayer, cameraFocuser.FocusOnLeftPlayerCruiser);
            rightCaptainButton.Initialise(singleSoundPlayer, cameraFocuser.FocusOnRightPlayerCruiser);

            _enabledToggler = new PvPFilterToggler(enabledFilter, overviewButton, leftPlayerCruiserButton, rightPlayerCruiserButton, leftCaptainButton, rightCaptainButton);
        }
    }
}