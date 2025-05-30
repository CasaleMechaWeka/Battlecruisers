using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene;
using BattleCruisers.UI.BattleScene.MainMenu;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils.BattleScene;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.MainMenu
{
    public class PvPMainMenuManager : PvPModalManager, IMainMenuManager
    {
        private readonly IModalMenu _modalMenu;
        private readonly PvPBattleCompletionHandler _battleCompletionHandler;

        public bool IsShown => _modalMenu.IsVisible.Value;

        public event EventHandler Dismissed;

        public PvPMainMenuManager(
            NavigationPermitterManager navigationPermitterManager,
            PauseGameManager pauseGameManager,
            IModalMenu modalMenu,
            PvPBattleCompletionHandler battleCompletionHandler)
            : base(navigationPermitterManager, pauseGameManager)
        {
            PvPHelper.AssertIsNotNull(modalMenu, battleCompletionHandler);

            _modalMenu = modalMenu;
            _battleCompletionHandler = battleCompletionHandler;
        }

        public PvPMainMenuManager(
            // IPvPNavigationPermitterManager navigationPermitterManager,
            // IPvPPauseGameManager pauseGameManager,
            IModalMenu modalMenu,
            PvPBattleCompletionHandler battleCompletionHandler)
            : base()
        {
            PvPHelper.AssertIsNotNull(modalMenu, battleCompletionHandler);

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
            PvPBattleSceneGodClient.Instance.WasLeftMatch = true;
            _pauseGameManager.ResumeGame();
            _battleCompletionHandler.CompleteBattle(wasVictory: false, retryLevel: false);
            Dismissed?.Invoke(this, EventArgs.Empty);
            /*
            string logName = "Battle_Quit";
#if LOG_ANALYTICS
    Debug.Log("Analytics: " + logName);
#endif
            ApplicationModel applicationModel = ApplicationModel;
            try
            {
                AnalyticsService.Instance.CustomData("Battle", DataProvider.GameModel.Analytics(ApplicationModel.Mode.ToString(), logName, ApplicationModel.UserWonSkirmish));
                AnalyticsService.Instance.Flush();
            }
            catch
            (ConsentCheckException ex)
            {
                Debug.Log(ex.Message);
            }
            */
        }

        public void RetryLevel()
        {
            // Need to resume game to get music back
            _pauseGameManager.ResumeGame();
            _battleCompletionHandler.CompleteBattle(wasVictory: false, retryLevel: true);
            Dismissed?.Invoke(this, EventArgs.Empty);
            /*
            string logName = "Battle_Retry_InGame";
#if LOG_ANALYTICS
    Debug.Log("Analytics: " + logName);
#endif
            ApplicationModel applicationModel = ApplicationModel;
            try
            {
                AnalyticsService.Instance.CustomData("Battle", DataProvider.GameModel.Analytics(ApplicationModel.Mode.ToString(), logName, ApplicationModel.UserWonSkirmish));
                AnalyticsService.Instance.Flush();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
            */
        }

        public void ShowSettings()
        {
            _modalMenu.ShowSettings();
        }
    }
}