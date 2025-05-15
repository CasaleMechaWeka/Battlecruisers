using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationButtonsPanel : Highlightable
    {
        private FilterToggler _enabledToggler;

        public CanvasGroupButton overviewButton, playerCruiserButton, aiCruiserButton;

        public void Initialise(IBroadcastingFilter enabledFilter, ICameraFocuser cameraFocuser, SingleSoundPlayer singleSoundPlayer)
        {
            base.Initialise();

            Helper.AssertIsNotNull(overviewButton, playerCruiserButton, aiCruiserButton);
            Helper.AssertIsNotNull(enabledFilter, cameraFocuser);

            overviewButton.Initialise(singleSoundPlayer, cameraFocuser.FocusOnOverview);
            playerCruiserButton.Initialise(singleSoundPlayer, cameraFocuser.FocusOnLeftCruiser);
            aiCruiserButton.Initialise(singleSoundPlayer, cameraFocuser.FocusOnRightCruiser);

            _enabledToggler = new FilterToggler(enabledFilter, overviewButton, playerCruiserButton, aiCruiserButton);
        }
    }
}