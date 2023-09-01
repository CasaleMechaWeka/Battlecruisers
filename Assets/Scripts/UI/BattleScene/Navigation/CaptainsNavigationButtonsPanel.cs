using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class CaptainsNavigationButtonsPanel : Highlightable
    {
        private FilterToggler _enabledToggler;

        public CanvasGroupButton playerCruiserButton, aiCruiserButton;

        public void Initialise(IBroadcastingFilter enabledFilter, ICameraFocuser cameraFocuser, ISingleSoundPlayer singleSoundPlayer)
        {
            base.Initialise();

            Helper.AssertIsNotNull(playerCruiserButton, aiCruiserButton);
            Helper.AssertIsNotNull(enabledFilter, cameraFocuser);

            playerCruiserButton.Initialise(singleSoundPlayer, cameraFocuser.FocusOnPlayerCruiser);
            aiCruiserButton.Initialise(singleSoundPlayer, cameraFocuser.FocusOnAICruiser);

            _enabledToggler = new FilterToggler(enabledFilter, playerCruiserButton, aiCruiserButton);
        }
    }
}
