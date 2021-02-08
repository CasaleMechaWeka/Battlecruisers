using BattleCruisers.UI.BattleScene.MainMenu;
using System;

namespace BattleCruisers.Utils.BattleScene.Lifetime
{
    public class LifetimeManager
    {
        private readonly ILifetimeEventBroadcaster _lifetimeEvents;
        private readonly IMainMenuManager _mainMenuManager;

        public LifetimeManager(ILifetimeEventBroadcaster lifetimeEvents, IMainMenuManager mainMenuManager)
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
                _mainMenuManager.ShowMenu();
            }
        }
    }
}