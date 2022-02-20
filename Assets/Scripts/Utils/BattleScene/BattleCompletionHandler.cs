using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using System;
using UnityEngine;

namespace BattleCruisers.Utils.BattleScene
{
    public class BattleCompletionHandler : IBattleCompletionHandler
    {
        private readonly IApplicationModel _applicationModel;
        private readonly ISceneNavigator _sceneNavigator;
        private bool _isCompleted;

        public event EventHandler BattleCompleted;

        public BattleCompletionHandler(
            IApplicationModel applicationModel, 
            ISceneNavigator sceneNavigator)
        {
            Helper.AssertIsNotNull(applicationModel, sceneNavigator);

            _applicationModel = applicationModel;
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

            switch (_applicationModel.Mode)
            {
                case GameMode.Campaign:
                    // Completing the tutorial does not count as a real level, so 
                    // only save battle result if this was not the tutorial.
                    BattleResult battleResult = new BattleResult(_applicationModel.SelectedLevel, wasVictory);
                    _applicationModel.DataProvider.GameModel.LastBattleResult = battleResult;
                    break;

                case GameMode.Skirmish:
                    _applicationModel.UserWonSkirmish = wasVictory;
                    break;
            }

            
            _applicationModel.DataProvider.SaveGame();

            _applicationModel.ShowPostBattleScreen = true;
            TimeBC.Instance.TimeScale = 1;

            if (retryLevel)
            {
                _sceneNavigator.GoToScene(SceneNames.BATTLE_SCENE);
            }
            else
            {
                _sceneNavigator.GoToScene(SceneNames.SCREENS_SCENE);
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

            switch (_applicationModel.Mode)
            {
                case GameMode.Campaign:
                    // Completing the tutorial does not count as a real level, so 
                    // only save battle result if this was not the tutorial.
                    BattleResult battleResult = new BattleResult(_applicationModel.SelectedLevel, wasVictory);
                    _applicationModel.DataProvider.GameModel.LastBattleResult = battleResult;
                    break;

                case GameMode.Skirmish:
                    _applicationModel.UserWonSkirmish = wasVictory;
                    break;
            }

            
            //Debug.Log(_applicationModel.DataProvider.GameModel.LifetimeDestructionScore);

            _applicationModel.ShowPostBattleScreen = true;
            TimeBC.Instance.TimeScale = 1;

            if (retryLevel)
            {
                _sceneNavigator.GoToScene(SceneNames.BATTLE_SCENE);
            }
            else if (wasVictory)
            {
                _applicationModel.DataProvider.GameModel.LifetimeDestructionScore += destructionScore;
                if (_applicationModel.DataProvider.GameModel.BestDestructionScore < destructionScore)
                {
                    _applicationModel.DataProvider.GameModel.BestDestructionScore = destructionScore;
                }
                _applicationModel.DataProvider.SaveGame();
                _sceneNavigator.GoToScene(SceneNames.DESTRUCTION_SCENE);
                
            }
            else{
                _sceneNavigator.GoToScene(SceneNames.SCREENS_SCENE);
            }
        }
    }
}