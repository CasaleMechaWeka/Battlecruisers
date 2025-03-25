using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using System;

namespace BattleCruisers.UI.BattleScene.MainMenu
{
    public class MainMenuManager : ModalManager, IMainMenuManager
    {
        private readonly IModalMenu _modalMenu;
        private readonly BattleCompletionHandler _battleCompletionHandler;

        public bool IsShown => _modalMenu.IsVisible.Value;

        public event EventHandler Dismissed;

        public MainMenuManager(
            INavigationPermitterManager navigationPermitterManager,
            PauseGameManager pauseGameManager,
            IModalMenu modalMenu,
            BattleCompletionHandler battleCompletionHandler)
            : base(navigationPermitterManager, pauseGameManager)
        {
            Helper.AssertIsNotNull(modalMenu, battleCompletionHandler);

            _modalMenu = modalMenu;
            _battleCompletionHandler = battleCompletionHandler;
        }

        public void ShowMenu()
        {
            base.ShowModal();
            _modalMenu.ShowMenu();
        }

        public void DismissMenu()
        {
            base.HideModal();
            _modalMenu.HideMenu();
            Dismissed?.Invoke(this, EventArgs.Empty);
        }

        public void QuitGame()
        {
            // Need to resume game to get music back
            _pauseGameManager.ResumeGame();
            _battleCompletionHandler.CompleteBattle(wasVictory: false, retryLevel: false);
            Dismissed?.Invoke(this, EventArgs.Empty);
        }

        public void RetryLevel()
        {
            // Need to resume game to get music back
            _pauseGameManager.ResumeGame();
            _battleCompletionHandler.CompleteBattle(wasVictory: false, retryLevel: true);
            Dismissed?.Invoke(this, EventArgs.Empty);
            string logName = "Battle_Retry_InGame";
#if LOG_ANALYTICS
    Debug.Log("Analytics: " + logName);
#endif
        }

        public void ShowSettings()
        {
            _modalMenu.ShowSettings();
        }
    }
}