using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using System;
using UnityEngine;

namespace BattleCruisers.Utils.BattleScene
{
    public class BattleCompletionHandler
    {
        private readonly ISceneNavigator _sceneNavigator;
        private bool _isCompleted;

        public event EventHandler BattleCompleted;

        public BattleCompletionHandler(
            ISceneNavigator sceneNavigator)
        {
            Helper.AssertIsNotNull(sceneNavigator);

            _sceneNavigator = sceneNavigator;

            _isCompleted = false;
        }

        public void CompleteBattle(bool wasVictory, bool retryLevel)
        {
            if (_isCompleted)
            {
                // Battle should only be completed once
                return;
            }
            _isCompleted = true;

            BattleCompleted?.Invoke(this, EventArgs.Empty);
            Debug.Log($"Was Victory: {wasVictory}");

            switch (ApplicationModel.Mode)
            {
                case GameMode.SideQuest:
                    BattleResult sideQuestBattleResult = new BattleResult(ApplicationModel.SelectedSideQuestID, wasVictory);
                    DataProvider.GameModel.LastBattleResult = sideQuestBattleResult;
                    break;
                case GameMode.Campaign:
                    // Completing the tutorial does not count as a real level, so 
                    // only save battle result if this was not the tutorial.
                    BattleResult battleResult = new BattleResult(ApplicationModel.SelectedLevel, wasVictory);
                    DataProvider.GameModel.LastBattleResult = battleResult;
                    break;

                case GameMode.Skirmish:
                    ApplicationModel.UserWonSkirmish = wasVictory;
                    break;

                // Coin Battle uses the same post-game as Skirmish right now (return to menu, no next, etc):
                case GameMode.CoinBattle:
                    ApplicationModel.UserWonSkirmish = wasVictory;
                    break;
            }


            DataProvider.SaveGame();

            if (ApplicationModel.Mode == GameMode.CoinBattle)
            {
                ApplicationModel.ShowPostBattleScreen = false;
            }
            else
            {
                ApplicationModel.ShowPostBattleScreen = true;
            }
            TimeBC.Instance.TimeScale = 1;

            if (retryLevel)
            {
                _sceneNavigator.GoToScene(SceneNames.BATTLE_SCENE, true);
            }
            else
            {
                _sceneNavigator.GoToScene(SceneNames.SCREENS_SCENE, true);
            }
        }

        public void CompleteBattle(bool wasVictory, bool retryLevel, long destructionScore)
        {
            if (_isCompleted)
            {
                // Battle should only be completed once
                return;
            }
            _isCompleted = true;

            BattleCompleted?.Invoke(this, EventArgs.Empty);

            switch (ApplicationModel.Mode)
            {
                case GameMode.SideQuest:
                    BattleResult sideQuestBattleResult = new BattleResult(ApplicationModel.SelectedSideQuestID, wasVictory);
                    DataProvider.GameModel.LastBattleResult = sideQuestBattleResult;
                    break;

                case GameMode.Campaign:
                    // Completing the tutorial does not count as a real level, so 
                    // only save battle result if this was not the tutorial.
                    BattleResult battleResult = new BattleResult(ApplicationModel.SelectedLevel, wasVictory);
                    DataProvider.GameModel.LastBattleResult = battleResult;
                    break;

                case GameMode.Skirmish:
                    ApplicationModel.UserWonSkirmish = wasVictory;
                    break;

                // Coin Battle uses the same post-game as Skirmish right now (return to menu, no next, etc):
                case GameMode.CoinBattle:
                    ApplicationModel.UserWonSkirmish = wasVictory;
                    break;
            }


            //Debug.Log(DataProvider.GameModel.LifetimeDestructionScore);
            if (ApplicationModel.Mode == GameMode.CoinBattle)
            {
                ApplicationModel.ShowPostBattleScreen = false;
            }
            else
            {
                ApplicationModel.ShowPostBattleScreen = true;
            }

            TimeBC.Instance.TimeScale = 1;

            if (retryLevel)
            {
                _sceneNavigator.GoToScene(SceneNames.BATTLE_SCENE, true);
            }
            else if (wasVictory)
            {
                //Debug.Log(DataProvider.GameModel.LifetimeDestructionScore + " - before");
                DataProvider.GameModel.LifetimeDestructionScore += destructionScore;
                //Debug.Log(DataProvider.GameModel.LifetimeDestructionScore + " - after");
                if (DataProvider.GameModel.BestDestructionScore < destructionScore)
                {
                    DataProvider.GameModel.BestDestructionScore = destructionScore;
                }
                DataProvider.SaveGame();
                _sceneNavigator.GoToScene(SceneNames.DESTRUCTION_SCENE, true);

            }
            else
            {
                _sceneNavigator.GoToScene(SceneNames.SCREENS_SCENE, true);
            }
        }
    }
}