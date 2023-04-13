using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleCruisers.Network.Multiplay.Matchplay.Server;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine.SceneManagement;


namespace BattleCruisers.Network.Multiplay.Matchplay.Client
{
    public class ClientGameManager : IDisposable
    {
        public event Action<Matchplayer> MatchPlayerSpawned;
        public event Action<Matchplayer> MatchPlayerDespawned;
        public MatchplayUser User { get; private set; }
        public MatchplayNetworkClient NetworkClient { get; private set; }
        public MatchplayMatchmaker Matchmaker { get; private set; }
        public bool Initialized { get; private set; } = false;
        public string ProfileName { get; private set; }

        public ClientGameManager(string profileName = "default")
        {
            User = new MatchplayUser();
            Debug.Log($"Beginning with new Profile:{profileName}");
            ProfileName = profileName;


#pragma warning disable 4014
            InitAsync();
#pragma warning restore 4014
        }

        async Task InitAsync()
        {
            var unityAuthenticationInitOptions = new InitializationOptions();
            unityAuthenticationInitOptions.SetProfile($"{ProfileName} {LocalProfileTool.LocalProfileSuffix}");
            await Unity.Services.Core.UnityServices.InitializeAsync(unityAuthenticationInitOptions);

            NetworkClient = new MatchplayNetworkClient();
            Matchmaker = new MatchplayMatchmaker();
            var authenticationResult = await AuthenticationWrapper.DoAuth();

            if (authenticationResult == AuthState.Authenticated)
            {
                User.AuthId = AuthenticationWrapper.PlayerID();
            }
            else
            {
                User.AuthId = Guid.NewGuid().ToString();
            }
            Debug.Log($"did Auth? {authenticationResult} {User.AuthId}");
            Initialized = true;
        }

        public void BeginConnection(string ip, int port)
        {
            Debug.Log($"Starting networkClient @ {ip}:{port}\nWith : {User}");
            NetworkClient.StartClient(ip, port);
        }

        public void Disconnect()
        {
            NetworkClient.DisconnectClient();
        }

        public async Task MatchmakeAsync(Action<MatchmakerPollingResult> onMatchmakerResponse = null)
        {
            if (Matchmaker.IsMatchmaking)
            {
                Debug.LogWarning("Already matchmaking, please wait or cancel.");
                return;
            }
            var matchResult = await GetMatchAsync();
            onMatchmakerResponse?.Invoke(matchResult);
        }


        public async Task CancelMatchmaking()
        {
            await Matchmaker.CancelMatchmaking();
        }

        public void ToMainMenu()
        {
            SceneManager.LoadScene("MultiplayScreensScene", LoadSceneMode.Single);
        }

        public void AddMatchPlayer(Matchplayer player)
        {
            MatchPlayerSpawned?.Invoke(player);
        }

        public void RemoveMatchPlayer(Matchplayer player)
        {
            MatchPlayerDespawned?.Invoke(player);
        }

        public void SetGameMode(GameMode gameMode)
        {
            User.GameModePreferences = gameMode;
        }
        public void SetGameMap(Map map)
        {
            User.MapPreferences = map;
        }

        public void SetGameQueue(GameQueue queue)
        {
            User.QueuePreference = queue;
        }


        async Task<MatchmakerPollingResult> GetMatchAsync()
        {
            Debug.Log($"Beginning Matchmaking with {User}");
            var matchmakingResult = await Matchmaker.Matchmake(User.Data);
            if (matchmakingResult.result == MatchmakerPollingResult.Success)
                BeginConnection(matchmakingResult.ip, matchmakingResult.port);
            else
                Debug.LogWarning($"{matchmakingResult.result} : {matchmakingResult.resultMessage}");
            return matchmakingResult.result;
        }

        public async Task<MatchmakingResult> GetMatchAsyncInLobby()
        {
            Debug.Log($"Beginning Matchmaking with {User}");
            var matchmakingResult = await Matchmaker.Matchmake(User.Data);
            return matchmakingResult;
        }



        public void Dispose()
        {
            NetworkClient?.Dispose();
            Matchmaker?.Dispose();
        }

        public void ExitGame()
        {
            Dispose();
            Application.Quit();
        }
    }
}

