using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.Sound.Players;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation
{
    public class PvPNavigationButtonsPanel : PvPHighlightable
    {
        private PvPFilterToggler _enabledToggler;

        public PvPCanvasGroupButton overviewButton, leftPlayerCruiserButton, rightPlayerCruiserButton;
        public PvPCanvasGroupButton leftCaptainButton, rightCaptainButton;

        public PvPCanvasGroupButton heckleButton, mainMenuButton;

        public void Initialise(IPvPBroadcastingFilter enabledFilter, IPvPCameraFocuser cameraFocuser, ISingleSoundPlayer singleSoundPlayer)
        {
            base.Initialise();

            PvPHelper.AssertIsNotNull(overviewButton, leftPlayerCruiserButton, rightPlayerCruiserButton);
            PvPHelper.AssertIsNotNull(leftCaptainButton, rightCaptainButton);
            PvPHelper.AssertIsNotNull(heckleButton, mainMenuButton);
            PvPHelper.AssertIsNotNull(enabledFilter, cameraFocuser);

            overviewButton.Initialise(singleSoundPlayer, cameraFocuser.FocusOnOverview);
            leftPlayerCruiserButton.Initialise(singleSoundPlayer, cameraFocuser.FocusOnLeftPlayerCruiser);
            rightPlayerCruiserButton.Initialise(singleSoundPlayer, cameraFocuser.FocusOnRightPlayerCruiser);

            leftCaptainButton.Initialise(singleSoundPlayer, cameraFocuser.FocusOnLeftPlayerCruiser);
            rightCaptainButton.Initialise(singleSoundPlayer, cameraFocuser.FocusOnRightPlayerCruiser);

            _enabledToggler = new PvPFilterToggler(enabledFilter, overviewButton, leftPlayerCruiserButton, rightPlayerCruiserButton, leftCaptainButton, rightCaptainButton, heckleButton/*, mainMenuButton*/);
        }
    }
}