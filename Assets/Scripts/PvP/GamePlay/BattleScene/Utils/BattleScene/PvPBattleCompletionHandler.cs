using BattleCruisers.Data;
using BattleCruisers.Scenes;
using System;
using UnityEngine;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.ApplicationLifecycle;
using BattleCruisers.Network.Multiplay.ConnectionManagement;
using BattleCruisers.Network.Multiplay.Gameplay.UI;
using BattleCruisers.Network.Multiplay.Infrastructure;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;
using BattleCruisers.Utils;
using Unity.Services.Leaderboards;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene
{
    public class PvPBattleCompletionHandler : IPvPBattleCompletionHandler
    {
        private readonly IApplicationModel _applicationModel;
        private readonly ISceneNavigator _sceneNavigator;
        public static bool _isCompleted = false;

        public event EventHandler BattleCompleted;
        private Team team;
        private float playerARating;
        private float playerBRating;
        public float registeredTime { get; set; }
        private const int POST_GAME_WAIT_TIME_IN_S = 10 * 1000;
        public PvPBattleCompletionHandler(
            IApplicationModel applicationModel,
            ISceneNavigator sceneNavigator)
        {
            PvPHelper.AssertIsNotNull(applicationModel, sceneNavigator);
            _applicationModel = applicationModel;
            _sceneNavigator = sceneNavigator;

            _isCompleted = false;
            team = SynchedServerData.Instance.GetTeam();
            playerARating = SynchedServerData.Instance.playerARating.Value;
            playerBRating = SynchedServerData.Instance.playerBRating.Value;
            registeredTime = -1;
        }

        // For disconnection
        public async void CompleteBattle(bool wasVictory, bool retryLevel)
        {
            if (_isCompleted)
            {
                return;
            }
            _isCompleted = true;
            if (MatchmakingScreenController.Instance != null)
            {
                MatchmakingScreenController.Instance.Destroy();
            }

#if !DISABLE_MATCHMAKING
            var newRatings = CalculateNewRatings(playerARating, playerBRating, wasVictory, team);
            if (team == Cruisers.Team.LEFT)
            {
                _applicationModel.DataProvider.GameModel.BattleWinScore = newRatings.Item1;
            }
            else
            {
                _applicationModel.DataProvider.GameModel.BattleWinScore = newRatings.Item2;
            }
            _applicationModel.DataProvider.SaveGame();

            double score = (double)_applicationModel.DataProvider.GameModel.BattleWinScore;
            const string LeaderboardID = "BC-PvP1v1Leaderboard";
            bool isSetPlayerName = PlayerPrefs.GetInt("SETNAME", 0) != 0;
            if (isSetPlayerName)
            {
                try
                {
                    await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardID, score);
                }
                catch
                {
                    Debug.LogWarning("Could not add player to leaderboard");
                }
            }
#endif

            BattleCompleted?.Invoke(this, EventArgs.Empty);
            PvPBattleSceneGodClient.Instance.OnTunnelBattleCompleted_ValueChanged();
            _applicationModel.DataProvider.SaveGame();

            //--->CODE CHANGED BY ANUJ
            //_applicationModel.ShowPostBattleScreen = true;
            //<---
            TimeBC.Instance.TimeScale = 1;
            if (wasVictory)
            {
                PvPBattleSceneGodClient.Instance.messageBox.ShowMessage(LocTableFactory.CommonTable.GetString("EnemyLeft"), () => PvPBattleSceneGodClient.Instance.messageBox.HideMessage());
                await Task.Delay(POST_GAME_WAIT_TIME_IN_S);
            }
            if (NetworkManager.Singleton.IsConnectedClient)
            {
                NetworkManager.Singleton.Shutdown(true);
                DestroyAllNetworkObjects();
            }

            if (wasVictory) //wasVictory means LEFT player, !wasVictory means RIGHT player.
            {
                if (PvPBattleSceneGodClient.Instance.wasOpponentDisconnected)
                {
                    GoToScene(SceneNames.PvP_DESTRUCTION_SCENE);
                }
                else
                    GoToScene(SceneNames.SCREENS_SCENE);
            }
            else
                GoToScene(SceneNames.SCREENS_SCENE);
        }

        bool isTriggtered = false;
        private void GoToScene(string sceneName)
        {
            if (isTriggtered)
                return;
            isTriggtered = false;
            _sceneNavigator.GoToScene(sceneName, true);
        }

        private float Probability(float rating1, float rating2)
        {
            return 1.0f / (1 + (float)(Math.Pow(10, (rating1 - rating2) / 400)));
        }
        private (float hostRating, float clientRating) CalculateNewRatings(float hostRating, float clientRating, bool wasVictory, Team playerTeam)
        {
            const int K = 30;
            float Pb = Probability(hostRating, clientRating);
            float Pa = Probability(clientRating, hostRating);

            //If wasVictory == true if host wins; false if client wins
            if (wasVictory)
            {
                if (playerTeam == Cruisers.Team.LEFT)
                {
                    if (registeredTime > 0 && Time.time - registeredTime > 60f)
                        hostRating += K * (1 - Pa);
                    clientRating -= K * Pb;
                }
                else
                {
                    hostRating -= K * Pa;
                    if (registeredTime > 0 && Time.time - registeredTime > 60f)
                        clientRating += K * (1 - Pb);
                }
            }

            else
            {
                if (playerTeam == Cruisers.Team.LEFT)
                {
                    hostRating -= K * Pa;
                    if (registeredTime > 0 && Time.time - registeredTime > 60f)
                        clientRating += K * (1 - Pb);
                }
                else
                {
                    if (registeredTime > 0 && Time.time - registeredTime > 60f)
                        hostRating += K * (1 - Pa);
                    clientRating -= K * Pb;
                }
            }
            return (hostRating, clientRating);
        }

        // For match normally completed
        public async void CompleteBattle(bool wasVictory, bool retryLevel, long destructionScore)
        {
            if (_isCompleted)
            {
                return;
            }
            _isCompleted = true;
            if (MatchmakingScreenController.Instance != null)
            {
                MatchmakingScreenController.Instance.Destroy();
            }

            if (registeredTime > 0 && Time.time - registeredTime > 60f)
            {
#if !DISABLE_MATCHMAKING
                var newRatings = CalculateNewRatings(playerARating, playerBRating, wasVictory, team);
                if (team == Cruisers.Team.LEFT)
                {
                    _applicationModel.DataProvider.GameModel.BattleWinScore = newRatings.hostRating;
                }
                else
                {
                    _applicationModel.DataProvider.GameModel.BattleWinScore = newRatings.clientRating;
                }
                _applicationModel.DataProvider.SaveGame();

                double score = (double)_applicationModel.DataProvider.GameModel.BattleWinScore;
                const string LeaderboardID = "BC-PvP1v1Leaderboard";
                bool isSetPlayerName = PlayerPrefs.GetInt("SETNAME", 0) != 0;
                if (isSetPlayerName)
                {
                    try
                    {
                        await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardID, score);
                    }
                    catch
                    {
                        Debug.Log("Failed to update player leaderboard score");
                    }
                }
#endif
            }

            BattleCompleted?.Invoke(this, EventArgs.Empty);
            PvPBattleSceneGodClient.Instance.OnTunnelBattleCompleted_ValueChanged();

            //--->CODE CHANGED BY ANUJ
            //_applicationModel.ShowPostBattleScreen = true;
            //<---
            TimeBC.Instance.TimeScale = 1;
            await Task.Delay(POST_GAME_WAIT_TIME_IN_S);

            // Whatever the outcome, everyone goes to Destruction Scene:
            _applicationModel.DataProvider.GameModel.LifetimeDestructionScore += destructionScore;
            if (_applicationModel.DataProvider.GameModel.BestDestructionScore < destructionScore)
            {
                _applicationModel.DataProvider.GameModel.BestDestructionScore = destructionScore;
            }
            _applicationModel.DataProvider.SaveGame();
            if (NetworkManager.Singleton != null)
                NetworkManager.Singleton.Shutdown(true);
            DestroyAllNetworkObjects();
            _sceneNavigator.GoToScene(SceneNames.PvP_DESTRUCTION_SCENE, true);
        }

        public async void DestroyAllNetworkObjects()
        {
            await Task.Delay(10);
            GameObject.Find("ApplicationController")?.GetComponent<ApplicationController>().DestroyNetworkObject();
            GameObject.Find("ConnectionManager")?.GetComponent<ConnectionManager>().DestroyNetworkObject();
            GameObject.Find("PopupPanelManager")?.GetComponent<PopupManager>().DestroyNetworkObject();
            GameObject.Find("UIMessageManager")?.GetComponent<ConnectionStatusMessageUIManager>().DestroyNetworkObject();
            GameObject.Find("UpdateRunner")?.GetComponent<UpdateRunner>().DestroyNetworkObject();
            GameObject.Find("NetworkManager")?.GetComponent<BCNetworkManager>().DestroyNetworkObject();
        }
    }
}