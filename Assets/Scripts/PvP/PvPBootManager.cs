using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using Unity.Services.Lobbies.Models;
using BattleCruisers.Network.Multiplay.ConnectionManagement;
using BattleCruisers.Network.Multiplay.Gameplay.Configuration;
using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;
using BattleCruisers.Network.Multiplay.UnityServices.Auth;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;
using BattleCruisers.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Qos;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;

namespace BattleCruisers.Network.Multiplay.Scenes
{
    public class PvPBootManager
    {
        [SerializeField]
        NameGenerationData m_NameGenerationData;

        public static PvPBootManager Instance;

        LocalLobbyUser LocalUser;
        LocalLobby LocalLobby;
        public readonly LobbyServiceFacade LobbyServiceFacade;
        public ConnectionManager ConnectionManager;
        AuthenticationServiceFacade AuthenticationServiceFacade;
        public string joinCode;

        [HideInInspector] public string playerAPrefabName;
        [HideInInspector] public ulong playerAClientNetworkId;
        [HideInInspector] public string playerAName;
        [HideInInspector] public long playerAScore;
        [HideInInspector] public string captainAPrefabName;
        [HideInInspector] public float playerRating;
        [HideInInspector] public int playerABodykit;
        [HideInInspector] public int playerABounty;
        public PvPBootManager(
        LocalLobbyUser localLobbyUser,
        LocalLobby localLobby,
        LobbyServiceFacade lobbyServiceFacade,
        ConnectionManager connectionManager,
        AuthenticationServiceFacade authenticationServiceFacade)
        {
            LocalUser = localLobbyUser;
            LocalUser.ID = AuthenticationService.Instance.PlayerId;
            LocalUser.DisplayName = DataProvider.GameModel.PlayerName;

            LocalLobby = localLobby;
            LobbyServiceFacade = lobbyServiceFacade;
            ConnectionManager = connectionManager;
            AuthenticationServiceFacade = authenticationServiceFacade;
            m_NameGenerationData = ScriptableObject.CreateInstance<NameGenerationData>();

            Logging.Log(Tags.Multiplay_SCREENS_SCENE_GOD, "START");

            LobbyServiceFacade.OnMatchMakingFailed += OnMatchmakingFailed;
            // ALWAYS update Instance to newest PvPBootManager (fixes UpdateRunner null on second match)
            Instance = this;
        }
        private void OnMatchmakingFailed()
        {
            DestroyAllNetworkObjects();
        }

        public async void DestroyAllNetworkObjects()
        {
            await Task.Delay(10);
            MatchmakingScreenController.Instance.FailedMatchmaking();
        }

        public async Task TryJoinLobby()
        {
            // find best matchmaking region

            bool playerIsAuthorized = await AuthenticationServiceFacade.EnsurePlayerIsAuthorized();

            if (!playerIsAuthorized)
            {
                Debug.Log("Player not authorized");
                return;
            }
            string selectedMap = DataProvider.GameModel.GameMap.ToString();
            LocalLobby.AddUser(LocalUser);

            List<QueryFilter> mFilters = new List<QueryFilter>()
            {
            // Let's search for games with open slots (AvailableSlots greater than 0)
            new QueryFilter(
                field: QueryFilter.FieldOptions.AvailableSlots,
                op: QueryFilter.OpOptions.EQ,
                value: "1"),
/*            new QueryFilter(
                field: QueryFilter.FieldOptions.S1, // S1 = "GameMap"
                op: QueryFilter.OpOptions.EQ,
                value: wantMap),*/
/*            new QueryFilter(
                field: QueryFilter.FieldOptions.N1, // N1 = "Score :  Battle Win"
                op: QueryFilter.OpOptions.GE,
                value: DataProvider.GameModel.BattleWinScore.ToString()),*/
            };

            List<QueryOrder> mOrders = new List<QueryOrder>
            {
                // Order by newest lobbies first
                new QueryOrder(false, QueryOrder.FieldOptions.Created),
                new QueryOrder(false, QueryOrder.FieldOptions.N1),
                new QueryOrder(false, QueryOrder.FieldOptions.N2),
            };

            string joinedCode = PlayerPrefs.GetString("JOINCODE", " ");

            Debug.Log("Started finding lobbies");
            QueryResponse response = await LobbyServiceFacade.QueryLobbyListAsync(mFilters, mOrders);
            List<Lobby> foundLobbies = response.Results;
            MatchmakingScreenController.Instance.SetMMStatus(MatchmakingScreenController.MMStatus.FINDING_LOBBY);

            (bool success, IList<IQosResult> qosResults) res = await ArenaSelectPanelScreenController.LatencyCheck;

            if (!res.success)
                return;

            int clientLatency = res.qosResults[0].AverageLatencyMs;

            Dictionary<string, int> clientLatenciesByRegion = res.qosResults
                .Where(result => result != null)
                .ToDictionary(
                    result => result.Region,
                    result => result.AverageLatencyMs
                );

            if (foundLobbies.Any())
            {
                Debug.Log($"Found {foundLobbies.Count} lobbies");
                List<(Lobby lobby, string region, int HostLatency)> validLobbies = new List<(Lobby lobby, string region, int latency)>();
                foreach (Lobby lobby in foundLobbies)
                {
                    string relayJoinCode = lobby.Data.ContainsKey("RelayJoinCode") ? lobby.Data["RelayJoinCode"].Value : null;
                    string region = lobby.Data.ContainsKey("Region") ? lobby.Data["Region"].Value : null;
                    if (!lobby.Data.TryGetValue("Latency", out DataObject latencyEntry) || !int.TryParse(latencyEntry.Value, out int hostLatency))
                        continue;

                    if (relayJoinCode != joinedCode
                        && !string.IsNullOrEmpty(relayJoinCode)
                        && !string.IsNullOrEmpty(region)
                        && clientLatenciesByRegion.ContainsKey(region)
                        && hostLatency > 0)
                    {
                        validLobbies.Add((lobby, region, hostLatency));
                    }
                }

                Lobby[] sortedLobbies = validLobbies
                    .Select(x => new
                    {
                        x.lobby,
                        TotalLatency = clientLatenciesByRegion[x.region] + x.HostLatency
                    })
                    .Where(x => x.TotalLatency <= ConnectionManager.LatencyLimit)
                    .OrderByDescending(x =>
                        x.lobby.Data.TryGetValue("GameMap", out DataObject mapEntry) && mapEntry.Value == selectedMap ? 1 : 0)
                    .ThenBy(x => x.TotalLatency)
                    .Select(x => x.lobby)
                    .ToArray();

                Debug.Log($"Found {sortedLobbies.Length} valid lobbies within latency limit");

                foreach (Lobby lobby in sortedLobbies)
                {
                    if (lobby.Players.Count >= lobby.MaxPlayers)
                    {
                        Debug.Log($"PVP: Skipping full lobby {lobby.LobbyCode} ({lobby.Players.Count}/{lobby.MaxPlayers} players)");
                        continue;
                    }

                    int totalLatency = int.Parse(lobby.Data["Latency"].Value) + clientLatenciesByRegion[lobby.Data["Region"].Value];
                    CheckLatency(totalLatency);
                    Debug.Log("Total Latency: " + totalLatency.ToString() + " ms");

                    MatchmakingScreenController.Instance.SetMMStatus(MatchmakingScreenController.MMStatus.JOIN_LOBBY);
                    (bool Success, Lobby Lobby) lobbyJoinAttemp = await LobbyServiceFacade.TryJoinLobbyAsync(lobbyId: lobby.Id, null);

                    if (!lobbyJoinAttemp.Success)
                        continue;

                    LobbyServiceFacade.SetRemoteLobby(lobbyJoinAttemp.Lobby);
                    if (LobbyServiceFacade.CurrentUnityLobby == null)
                        continue;

                    Debug.Log($"Joined Lobby {lobbyJoinAttemp.Lobby.Name} ({lobbyJoinAttemp.Lobby.Id})");
                    PlayerPrefs.SetString("JOINCODE", lobby.Data["RelayJoinCode"].Value);

                    if (MatchmakingScreenController.Instance != null)
                    {
                        Debug.Log("PVP: CLIENT re-enabling pre-loaded PvPBattleScene before StartClientLobby");
                        MatchmakingScreenController.Instance.ReEnableBattleSceneGameObjects();
                    }

                    ConnectionManager.StartClientLobby(DataProvider.GameModel.PlayerName);
                    return;
                }
            }

            await HostLobby(clientLatency, selectedMap, false);

            Debug.Log("No suitable lobbies found, hosting new lobby");
        }
        public async Task<Lobby> CreateLobby(string desiredMap, bool isPrivate)
        {
            if (isPrivate)
                LocalLobby.AddUser(LocalUser);

            Dictionary<string, DataObject> lobbyData = new Dictionary<string, DataObject>()
            {
                ["GameMap"] = new DataObject(DataObject.VisibilityOptions.Public, desiredMap, DataObject.IndexOptions.S1),
                ["Score"] = new DataObject(DataObject.VisibilityOptions.Public, Mathf.FloorToInt(DataProvider.GameModel.BattleWinScore).ToString(), DataObject.IndexOptions.N1),
            };

            if (LobbyServiceFacade == null)
                return null;

            (bool Success, Lobby Lobby) lobbyCreationAttemp = await LobbyServiceFacade.TryCreateLobbyAsync(
            m_NameGenerationData.GenerateName(),
            ConnectionManager.MaxConnectedPlayers,
            isPrivate: isPrivate,
            LocalUser.GetDataForUnityServices(),
            lobbyData);

            for (int i = 0; i < 10; i++)
            {
                if (lobbyCreationAttemp.Success)
                {
                    LocalUser.IsHost = true;
                    LobbyServiceFacade.SetRemoteLobby(lobbyCreationAttemp.Lobby);
                    if (LobbyServiceFacade.CurrentUnityLobby != null)
                    {
                        Debug.Log($"Created lobby '{lobbyCreationAttemp.Lobby.Name}' with code '{lobbyCreationAttemp.Lobby.LobbyCode}' (ID: {lobbyCreationAttemp.Lobby.Id})");

                        break;
                    }
                }
                lobbyCreationAttemp = await LobbyServiceFacade.TryCreateLobbyAsync(
                m_NameGenerationData.GenerateName(),
                ConnectionManager.MaxConnectedPlayers,
                isPrivate: isPrivate,
                LocalUser.GetDataForUnityServices(),
                lobbyData);
                await Task.Delay(1000);
            }

            return lobbyCreationAttemp.Lobby;
        }
        public async Task<Lobby> HostLobby(int latency, string desiredMap, bool isPrivate)
        {
            if (!StaticData.MeetsMinCPURequirements())
            {
                Debug.Log("CPU requirements are not met - can't host");
                return null;
            }
            CheckLatency(latency);
            if (latency > (int)(ConnectionManager.LatencyLimit * 0.6f))
            {
                MatchmakingScreenController.Instance.ShowBadInternetMessageBox();
                return null;
            }
            MatchmakingScreenController.Instance.SetMMStatus(MatchmakingScreenController.MMStatus.CREATING_LOBBY);
            Lobby lobby = await CreateLobby(desiredMap, isPrivate);
            if (lobby != null && MatchmakingScreenController.Instance != null)
            {
                if (!isPrivate)
                {
                    Debug.Log("PVP: PublicPVP lobby created - setting up relay allocation (lobby will become discoverable)");
                    await ConnectionManager.SetupRelayForMatchmaking(DataProvider.GameModel.PlayerName);
                    Debug.Log("PVP: PublicPVP relay allocated, lobby now discoverable - waiting for Players=2/2");
                    MatchmakingScreenController.Instance.SetMMStatus(MatchmakingScreenController.MMStatus.LOOKING_VICTIM);
                }
                Debug.Log("PVP: Lobby created - starting tracking, waiting for Players=2/2");
                LobbyServiceFacade.BeginTracking();
                MatchmakingScreenController.Instance.StartLobbyLoop();
            }
            return lobby;
        }
        void CheckLatency(int latency)
        {
            Debug.Log($"Update latency: {latency} ms");

            MatchmakingScreenController.Instance.Connection_Quality = latency switch
            {
                < 50 => ConnectionQuality.HIGH,
                < 100 => ConnectionQuality.MID,
                < 150 => ConnectionQuality.LOW,
                _ => ConnectionQuality.DEAD
            };
        }
        public async Task<Lobby> JoinLobbyByCode(string code)
        {
            LocalLobby.AddUser(LocalUser);
            (bool Success, Lobby Lobby) lobbyJoinAttemp = await LobbyServiceFacade.TryJoinLobbyAsync("", code);

            if (!lobbyJoinAttemp.Success || lobbyJoinAttemp.Lobby == null)
            {
                Debug.LogError($"Failed to join lobby with code {code} - Success={lobbyJoinAttemp.Success}, Lobby={lobbyJoinAttemp.Lobby}");
                return null;
            }

            Lobby lobby = lobbyJoinAttemp.Lobby;
            LobbyServiceFacade.SetRemoteLobby(lobby);

            Debug.Log($"Joined Lobby {lobby.Name} ({lobby.Id})");
            joinCode = code;

            if (lobby.Data.TryGetValue("RelayJoinCode", out DataObject relayCodeData))
            {
                PlayerPrefs.SetString("JOINCODE", relayCodeData.Value);
                Debug.Log($"Cached relay join code: {relayCodeData.Value}");
            }
            else
            {
                Debug.LogWarning("RelayJoinCode not yet in lobby data (host still setting up relay)");
            }

            return lobby;
        }
        void OnDestroy()
        {
            LobbyServiceFacade.OnMatchMakingFailed -= OnMatchmakingFailed;
        }
        public bool IsLocalUserHost => LocalUser?.IsHost ?? false;
    }
}
