using BattleCruisers.UI.BattleScene.MainMenu;
using BattleCruisers.UI.Loading;
using System;

namespace BattleCruisers.Utils.BattleScene.Lifetime
{
    public class LifetimeManager
    {
        private readonly LifetimeEventBroadcaster _lifetimeEvents;
        private readonly IMainMenuManager _mainMenuManager;

        public LifetimeManager(LifetimeEventBroadcaster lifetimeEvents, IMainMenuManager mainMenuManager)
        {
            Helper.AssertIsNotNull(lifetimeEvents, mainMenuManager);

            _lifetimeEvents = lifetimeEvents;
            _mainMenuManager = mainMenuManager;

            _lifetimeEvents.IsPaused.ValueChanged += IsPaused_ValueChanged;
        }

        private void IsPaused_ValueChanged(object sender, EventArgs e)
        {
            if (_lifetimeEvents.IsPaused.Value)
            {
                // only show menu if the loading screen is inactive
                // otherwise, the game can be locked up by having both active at once
                if (LoadingScreenController.Instance == null || !LoadingScreenController.Instance.isActiveAndEnabled)
                {
                    _mainMenuManager.ShowMenu();
                }
            }
        }
    }
}