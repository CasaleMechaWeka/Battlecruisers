using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene;
using System;
using Unity.Services.Analytics;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.MainMenu
{
    public class PvPMainMenuManager : PvPModalManager, IPvPMainMenuManager
    {
        private readonly IPvPModalMenu _modalMenu;
        private readonly IPvPBattleCompletionHandler _battleCompletionHandler;

        public bool IsShown => _modalMenu.IsVisible.Value;

        public event EventHandler Dismissed;

        public PvPMainMenuManager(
            IPvPNavigationPermitterManager navigationPermitterManager,
            IPvPPauseGameManager pauseGameManager,
            IPvPModalMenu modalMenu,
            IPvPBattleCompletionHandler battleCompletionHandler)
            : base(navigationPermitterManager, pauseGameManager)
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
            _pauseGameManager.ResumeGame();
            _battleCompletionHandler.CompleteBattle(wasVictory: false, retryLevel: false);
            Dismissed?.Invoke(this, EventArgs.Empty);
            string logName = "Battle_Quit";
#if LOG_ANALYTICS
    Debug.Log("Analytics: " + logName);
#endif
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            try
            {
                AnalyticsService.Instance.CustomData("Battle", applicationModel.DataProvider.GameModel.Analytics(applicationModel.Mode.ToString(), logName, applicationModel.UserWonSkirmish));
                AnalyticsService.Instance.Flush();
            }
            catch
            (ConsentCheckException ex)
            {
                Debug.Log(ex.Message);
            }

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
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            try
            {
                AnalyticsService.Instance.CustomData("Battle", applicationModel.DataProvider.GameModel.Analytics(applicationModel.Mode.ToString(), logName, applicationModel.UserWonSkirmish));
                AnalyticsService.Instance.Flush();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }

        }

        public void ShowSettings()
        {
            _modalMenu.ShowSettings();
        }
    }
}