using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene
{
    public class PvPBattleCompletionHandler : IPvPBattleCompletionHandler
    {
        private readonly IApplicationModel _applicationModel;
        private readonly ISceneNavigator _sceneNavigator;
        private PvPBattleSceneGodTunnel _battleSceneGodTunnel;
        private bool _isCompleted;

        public event EventHandler BattleCompleted;

        public PvPBattleCompletionHandler(
            IApplicationModel applicationModel,
            ISceneNavigator sceneNavigator,
            PvPBattleSceneGodTunnel battleSceneGodTunnel)
        {
            PvPHelper.AssertIsNotNull(applicationModel, sceneNavigator);

            _applicationModel = applicationModel;
            _sceneNavigator = sceneNavigator;

            _isCompleted = false;
            _battleSceneGodTunnel = battleSceneGodTunnel;

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
            _battleSceneGodTunnel.BattleCompleted.Value = Tunnel_BattleCompletedState.Completed;

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
            PvPTimeBC.Instance.TimeScale = 1;

            if (retryLevel)
            {
                _sceneNavigator.GoToScene(PvPSceneNames.BATTLE_SCENE, true);
            }
            else
            {
                _sceneNavigator.GoToScene(PvPSceneNames.SCREENS_SCENE, true);
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
            _battleSceneGodTunnel.BattleCompleted.Value = Tunnel_BattleCompletedState.Completed;

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
            PvPTimeBC.Instance.TimeScale = 1;

            if (retryLevel)
            {
                _sceneNavigator.GoToScene(PvPSceneNames.BATTLE_SCENE, true);
            }
            else if (wasVictory)
            {
                //Debug.Log(_applicationModel.DataProvider.GameModel.LifetimeDestructionScore + " - before");
                _applicationModel.DataProvider.GameModel.LifetimeDestructionScore += destructionScore;
                //Debug.Log(_applicationModel.DataProvider.GameModel.LifetimeDestructionScore + " - after");
                if (_applicationModel.DataProvider.GameModel.BestDestructionScore < destructionScore)
                {
                    _applicationModel.DataProvider.GameModel.BestDestructionScore = destructionScore;
                }
                _applicationModel.DataProvider.SaveGame();
                _sceneNavigator.GoToScene(PvPSceneNames.DESTRUCTION_SCENE, true);

            }
            else
            {
                _sceneNavigator.GoToScene(PvPSceneNames.SCREENS_SCENE, true);
            }
        }
    }
}