using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using System;

namespace BattleCruisers.UI.BattleScene
{
    // FELIX  Test :)
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
            _pauseGameManager.ResumeGame();
            _modalMenu.ShowMenu(OnModalMenuDismissed);
        }

        private void OnModalMenuDismissed(UserAction userAction)
        {
            switch (userAction)
            {
                case UserAction.Dismissed:
                    _pauseGameManager.ResumeGame();
                    break;

                case UserAction.Quit:
                    _battleCompletionHandler.CompleteBattle(wasVictory: false);
                    break;

                default:
                    throw new ArgumentException();
            }
        }
    }
}