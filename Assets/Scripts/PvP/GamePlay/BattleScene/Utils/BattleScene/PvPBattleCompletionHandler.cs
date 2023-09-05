using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using System;
using UnityEngine;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.ApplicationLifecycle;
using BattleCruisers.Network.Multiplay.ConnectionManagement;
using BattleCruisers.Network.Multiplay.Gameplay.UI;
using BattleCruisers.Network.Multiplay.Infrastructure;
using Unity.Services.Leaderboards;

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

        public async void CompleteBattle(bool wasVictory, bool retryLevel)
        {
            await Task.Delay(10);
            if (_isCompleted)
            {
                return;
            }
            var Ratings = EloRating(SynchedServerData.Instance.playerARating.Value, SynchedServerData.Instance.playerBRating.Value, 30, wasVictory);
            if (SynchedServerData.Instance.GetTeam() == Cruisers.Team.LEFT)
            {
                _applicationModel.DataProvider.GameModel.BattleWinScore = Ratings.Item1;
                _applicationModel.DataProvider.SaveGame();
            }
            else
            {
                _applicationModel.DataProvider.GameModel.BattleWinScore = Ratings.Item2;
                _applicationModel.DataProvider.SaveGame();
            }

            double score = (double)_applicationModel.DataProvider.GameModel.BattleWinScore;
            const string LeaderboardID = "BC-PvP1v1Leaderboard";
            await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardID, score);

            _isCompleted = true;

            BattleCompleted?.Invoke(this, EventArgs.Empty);
            PvPBattleSceneGodClient.Instance.OnTunnelBattleCompleted_ValueChanged();
            _applicationModel.DataProvider.SaveGame();

            //--->CODE CHANGED BY ANUJ
            //_applicationModel.ShowPostBattleScreen = true;
            //<---
            PvPTimeBC.Instance.TimeScale = 1;
            if (NetworkManager.Singleton.IsConnectedClient)
                NetworkManager.Singleton.Shutdown(true);

            DestroyAllNetworkObjects();
            _sceneNavigator.GoToScene(PvPSceneNames.SCREENS_SCENE, true);
        }

        private float Probability(float rating1, float rating2)
        {
            return 1.0f * 1.0f
            / (1
               + 1.0f
                     * (float)(Math.Pow(
                         10, 1.0f * (rating1 - rating2)
                                 / 400)));
        }
        private (float, float) EloRating(float Ra, float Rb, int K, bool d)
        {
            float Pb = Probability(Ra, Rb);
            float Pa = Probability(Rb, Ra);

            if (d == true)
            {
                Ra = Ra + K * (1 - Pa);
                Rb = Rb + K * (0 - Pb);
            }

            else
            {
                Ra = Ra + K * (0 - Pa);
                Rb = Rb + K * (1 - Pb);
            }
            return (Ra, Rb);
        }

        public async void CompleteBattle(bool wasVictory, bool retryLevel, long destructionScore)
        {
            await Task.Delay(10);

            if (_isCompleted)
            {
                return;
            }

            var Ratings = EloRating(SynchedServerData.Instance.playerARating.Value, SynchedServerData.Instance.playerBRating.Value, 30, wasVictory);
            if(SynchedServerData.Instance.GetTeam() == Cruisers.Team.LEFT)
            {
                _applicationModel.DataProvider.GameModel.BattleWinScore = Ratings.Item1;
                _applicationModel.DataProvider.SaveGame();
            }
            else
            {
                _applicationModel.DataProvider.GameModel.BattleWinScore = Ratings.Item2;
                _applicationModel.DataProvider.SaveGame();
            }

            double score = (double)_applicationModel.DataProvider.GameModel.BattleWinScore;
            const string LeaderboardID = "BC-PvP1v1Leaderboard";
            await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardID, score);

            _isCompleted = true;
            BattleCompleted?.Invoke(this, EventArgs.Empty);
            PvPBattleSceneGodClient.Instance.OnTunnelBattleCompleted_ValueChanged();
            if (NetworkManager.Singleton.IsConnectedClient)
                NetworkManager.Singleton.Shutdown(true);

            //--->CODE CHANGED BY ANUJ
            //_applicationModel.ShowPostBattleScreen = true;
            //<---
            PvPTimeBC.Instance.TimeScale = 1;

            if (wasVictory)
            {
                if (SynchedServerData.Instance.GetTeam() == Cruisers.Team.LEFT)
                {
                    //Debug.Log(_applicationModel.DataProvider.GameModel.LifetimeDestructionScore + " - before");
                    _applicationModel.DataProvider.GameModel.LifetimeDestructionScore += destructionScore;
                    //Debug.Log(_applicationModel.DataProvider.GameModel.LifetimeDestructionScore + " - after");
                    if (_applicationModel.DataProvider.GameModel.BestDestructionScore < destructionScore)
                    {
                        _applicationModel.DataProvider.GameModel.BestDestructionScore = destructionScore;
                    }
                    _applicationModel.DataProvider.SaveGame();
                    DestroyAllNetworkObjects();
                    _sceneNavigator.GoToScene(PvPSceneNames.PvP_DESTRUCTION_SCENE, true);
                }
                else
                {

                    DestroyAllNetworkObjects();
                    _sceneNavigator.GoToScene(PvPSceneNames.SCREENS_SCENE, true);
                }
            }
            else
            {
                if (SynchedServerData.Instance.GetTeam() == Cruisers.Team.LEFT)
                {

                    DestroyAllNetworkObjects();
                    _sceneNavigator.GoToScene(PvPSceneNames.SCREENS_SCENE, true);
                }
                else
                {
                    //Debug.Log(_applicationModel.DataProvider.GameModel.LifetimeDestructionScore + " - before");
                    _applicationModel.DataProvider.GameModel.LifetimeDestructionScore += destructionScore;
                    //Debug.Log(_applicationModel.DataProvider.GameModel.LifetimeDestructionScore + " - after");
                    if (_applicationModel.DataProvider.GameModel.BestDestructionScore < destructionScore)
                    {
                        _applicationModel.DataProvider.GameModel.BestDestructionScore = destructionScore;
                    }
                    _applicationModel.DataProvider.SaveGame();

                    DestroyAllNetworkObjects();
                    _sceneNavigator.GoToScene(PvPSceneNames.PvP_DESTRUCTION_SCENE, true);
                }
            }
        }

        public async void DestroyAllNetworkObjects()
        {
            await Task.Delay(10);
            if (GameObject.Find("ApplicationController") != null)
                GameObject.Find("ApplicationController").GetComponent<ApplicationController>().DestroyNetworkObject();

            if (GameObject.Find("ConnectionManager") != null)
                GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>().DestroyNetworkObject();

            if (GameObject.Find("PopupPanelManager") != null)
                GameObject.Find("PopupPanelManager").GetComponent<PopupManager>().DestroyNetworkObject();

            if (GameObject.Find("UIMessageManager") != null)
                GameObject.Find("UIMessageManager").GetComponent<ConnectionStatusMessageUIManager>().DestroyNetworkObject();

            if (GameObject.Find("UpdateRunner") != null)
                GameObject.Find("UpdateRunner").GetComponent<UpdateRunner>().DestroyNetworkObject();

            if (GameObject.Find("NetworkManager") != null)
                GameObject.Find("NetworkManager").GetComponent<BCNetworkManager>().DestroyNetworkObject();
        }
    }
}