using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using System;

namespace BattleCruisers.UI.BattleScene
{
    public class MainMenuManager : IMainMenuManager
    {
        private readonly IPauseGameManager _pauseGameManager;
        private readonly IModalMenu _modalMenu;
        private readonly IBattleCompletionHandler _battleCompletionHandler;

        public MainMenuManager(
            IPauseGameManager pauseGameManager,
            IModalMenu modalMenu,
            IBattleCompletionHandler battleCompletionHandler)
        {
            Helper.AssertIsNotNull(pauseGameManager, modalMenu, battleCompletionHandler);

            _pauseGameManager = pauseGameManager;
            _modalMenu = modalMenu;
            _battleCompletionHandler = battleCompletionHandler;
        }

        public void ShowMenu()
        {
            _pauseGameManager.PauseGame();
            _modalMenu.ShowMenu(this);
        }

        public void DismissMenu()
        {
            _pauseGameManager.ResumeGame();
            _modalMenu.HideMenu();
        }

        public void QuitGame()
        {
            _battleCompletionHandler.CompleteBattle(wasVictory: false);
            _modalMenu.HideMenu();
        }
    }
}