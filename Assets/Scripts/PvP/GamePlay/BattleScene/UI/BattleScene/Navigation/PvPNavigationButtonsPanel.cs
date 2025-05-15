using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound.Players;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation
{
    public class PvPNavigationButtonsPanel : Highlightable
    {
        private FilterToggler _enabledToggler;

        public PvPCanvasGroupButton overviewButton, leftPlayerCruiserButton, rightPlayerCruiserButton;
        public PvPCanvasGroupButton leftCaptainButton, rightCaptainButton;

        public PvPCanvasGroupButton heckleButton, mainMenuButton;

        public void Initialise(IBroadcastingFilter enabledFilter, ICameraFocuser cameraFocuser, SingleSoundPlayer singleSoundPlayer)
        {
            base.Initialise();

            PvPHelper.AssertIsNotNull(overviewButton, leftPlayerCruiserButton, rightPlayerCruiserButton);
            PvPHelper.AssertIsNotNull(leftCaptainButton, rightCaptainButton);
            PvPHelper.AssertIsNotNull(heckleButton, mainMenuButton);
            PvPHelper.AssertIsNotNull(enabledFilter, cameraFocuser);

            overviewButton.Initialise(singleSoundPlayer, cameraFocuser.FocusOnOverview);
            leftPlayerCruiserButton.Initialise(singleSoundPlayer, cameraFocuser.FocusOnLeftCruiser);
            rightPlayerCruiserButton.Initialise(singleSoundPlayer, cameraFocuser.FocusOnRightCruiser);

            leftCaptainButton.Initialise(singleSoundPlayer, cameraFocuser.FocusOnLeftCruiser);
            rightCaptainButton.Initialise(singleSoundPlayer, cameraFocuser.FocusOnRightCruiser);

            _enabledToggler = new FilterToggler(enabledFilter, overviewButton, leftPlayerCruiserButton, rightPlayerCruiserButton, leftCaptainButton, rightCaptainButton, heckleButton/*, mainMenuButton*/);
        }
    }
}