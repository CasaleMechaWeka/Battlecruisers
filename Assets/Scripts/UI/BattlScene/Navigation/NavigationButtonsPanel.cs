using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationButtonsPanel : MonoBehaviour
    {
        private FilterToggler _enabledToggler;

        public CanvasGroupButton overviewButton, playerCruiserButton, aiCruiserButton;

        public void Initialise(IBroadcastingFilter enabledFilter, ICameraFocuser cameraFocuser, ISingleSoundPlayer singleSoundPlayer)
        {
            Helper.AssertIsNotNull(overviewButton, playerCruiserButton, aiCruiserButton);
            Helper.AssertIsNotNull(enabledFilter, cameraFocuser);

            overviewButton.Initialise(singleSoundPlayer, clickAction: cameraFocuser.FocusOnOverview);
            playerCruiserButton.Initialise(singleSoundPlayer, clickAction: cameraFocuser.FocusOnPlayerCruiser);
            aiCruiserButton.Initialise(singleSoundPlayer, clickAction: cameraFocuser.FocusOnAICruiser);

            _enabledToggler = new FilterToggler(enabledFilter, overviewButton, playerCruiserButton, aiCruiserButton);
        }
    }
}