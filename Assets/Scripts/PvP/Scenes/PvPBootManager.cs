using BattleCruisers.Data;
using BattleCruisers.Scenes;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using VContainer;
using BattleCruisers.Network.Multiplay.UnityServices.Auth;
using Unity.Services.Lobbies.Models;
using BattleCruisers.Network.Multiplay.Utils;
using Unity.Services.Authentication;
using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;
using BattleCruisers.Network.Multiplay.Gameplay.GameState;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Network.Multiplay.Gameplay.Configuration;
using BattleCruisers.Network.Multiplay.ConnectionManagement;
using BattleCruisers.Network.Multiplay.Infrastructure;
using System.Diagnostics;
using System.Net;
using Unity.Services.Qos;
using System.Threading;

namespace BattleCruisers.Network.Multiplay.Scenes
{
    public class PvPBootManager : GameStateBehaviour
    {

        public override GameState ActiveState { get { return GameState.MultiplayScreenScene; } }

        // networking settings

        [SerializeField]
        string m_IP;
        [SerializeField]
        string m_Port;
        [SerializeField]
        NameGenerationData m_NameGenerationData;


        //// ------------------------------------  Local Test  -------------------------------------  //////
        public bool m_LocalLaunchMode = true;
        private string k_DefaultLobbyName;
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        public int defaultLevel;
        public static PvPBootManager Instance;
        public CancellationTokenSource m_CancellationToken = new CancellationTokenSource();
        const int k_DefaultPort = 7777;
        const string k_DefaultIP = "127.0.0.1";


        public TrashTalkDataList trashDataList;
        [SerializeField]
        private AudioSource _uiAudioSource;

        [Inject] AuthenticationServiceFacade m_AuthServiceFacade;
        [Inject] LocalLobbyUser m_LocalUser;
        [Inject] LocalLobby m_LocalLobby;
        [Inject] LobbyServiceFacade m_LobbyServiceFacade;
        [Inject] LobbyAPIInterface m_LobbyAPIInterface;
        [Inject] ProfileManager m_ProfileManager;

        [Inject] ConnectionManager m_ConnectionManager;
        [Inject] AuthenticationServiceFacade m_AuthenticationServiceFacade;
        ISubscriber<ConnectStatus> m_ConnectStatusSubscriber;

        [Inject]
        void InjectDependencies(ISubscriber<ConnectStatus> connectStatusSubscriber)
        {
            m_ConnectStatusSubscriber = connectStatusSubscriber;
            m_ConnectStatusSubscriber.Subscribe(OnConnectStatusMessage);
        }

        void OnConnectStatusMessage(ConnectStatus connectStatus)
        {

        }

        private void JoinWithIP(string ip, string port)
        {
            int.TryParse(port, out var portNum);
            if (portNum <= 0)
            {
                portNum = k_DefaultPort;
            }
            ip = string.IsNullOrEmpty(ip) ? k_DefaultIP : ip;

#if UNITY_EDITOR
            if (ParrelSync.ClonesManager.IsClone())
            {
                m_ConnectionManager.StartClientIp(ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.PlayerName, ip, portNum);
            }
#endif
            m_ConnectionManager.StartHostIp(ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.PlayerName, ip, portNum);
        }

        private void JoinWithLobby()
        {

#pragma warning disable 4014
            JoinWithLobbyRequest();
#pragma warning restore 4014
        }

        private void onMatchmakingFailed()
        {
            DestroyAllNetworkObjects();
        }

        private void onMatchmakingStarted()
        {

        }

        public async void DestroyAllNetworkObjects()
        {
            await Task.Delay(10);
            MatchmakingScreenController.Instance.FailedMatchmaking();
        }

        private async Task JoinWithLobbyRequest()
        {
            bool playerIsAuthorized = await m_AuthenticationServiceFacade.EnsurePlayerIsAuthorized();

            if (!playerIsAuthorized)
            {
                return;
            }

            string wantMap = ConvertToScene((Map)ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.GameMap);

            m_LocalUser.ID = AuthenticationService.Instance.PlayerId;
            m_LocalUser.DisplayName = ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.PlayerName;
            m_LocalLobby.AddUser(m_LocalUser);

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
                value: ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.BattleWinScore.ToString()),*/
            };

            List<QueryOrder> mOrders = new List<QueryOrder>
        {
                // Order by newest lobbies first
            new QueryOrder(false, QueryOrder.FieldOptions.Created),
            new QueryOrder(false, QueryOrder.FieldOptions.N1),
            new QueryOrder(false, QueryOrder.FieldOptions.N2),
        };
            string joinedCode = PlayerPrefs.GetString("JOINCODE", " ");
            while (true && !m_CancellationToken.IsCancellationRequested)
            {
                UnityEngine.Debug.Log("===> Started Finding Lobbies");
                QueryResponse response = await m_LobbyServiceFacade.QueryLobbyListAsync(mFilters, mOrders);
                List<Lobby> foundLobbies = response.Results;
                MatchmakingScreenController.Instance.SetMMStatus(MatchmakingScreenController.MMStatus.FINDING_LOBBY);
                bool isFound = false;
                if (foundLobbies.Any())
                {
                    UnityEngine.Debug.Log("Found Lobbies :\n" + JsonConvert.SerializeObject(foundLobbies));
                    bool joined = false;
                    foreach (Lobby lobby in foundLobbies)
                    {
                        string RelayJoinCode = lobby.Data.ContainsKey("RelayJoinCode") ? lobby.Data["RelayJoinCode"].Value : null;
                        string Region = lobby.Data.ContainsKey("Region") ? lobby.Data["Region"].Value : null;
                        string HostLatency = lobby.Data.ContainsKey("Latency") ? lobby.Data["Latency"].Value : null;
                        if (RelayJoinCode == joinedCode || string.IsNullOrEmpty(RelayJoinCode) || string.IsNullOrEmpty(Region) || string.IsNullOrEmpty(HostLatency))
                            continue;
                        else if (!string.IsNullOrEmpty(RelayJoinCode) && !string.IsNullOrEmpty(Region) && !string.IsNullOrEmpty(HostLatency))
                        {
                            if (lobby.Data["GameMap"].Value == wantMap)
                            {
                                var regions = new List<string>();
                                regions.Add(Region);
                                var qosResultsForRegion = await QosService.Instance.GetSortedQosResultsAsync("relay", regions);
                                int ClientLatency = qosResultsForRegion[0].AverageLatencyMs;
                                CheckLatency(ClientLatency);
                                UnityEngine.Debug.Log("===>client latency ---> " + ClientLatency);
                                int iHostLatency = 0;
                                int.TryParse(HostLatency, out iHostLatency);
                                if ((iHostLatency + ClientLatency) > ConnectionManager.LatencyLimit)
                                {
                                    continue;
                                }
                                UnityEngine.Debug.Log("===>joined latency ---> " + (iHostLatency + ClientLatency));
                                MatchmakingScreenController.Instance.SetMMStatus(MatchmakingScreenController.MMStatus.JOIN_LOBBY);
                                var lobbyJoinAttemp = await m_LobbyServiceFacade.TryJoinLobbyAsync(lobbyId: lobby.Id, null);

                                if (lobbyJoinAttemp.Success)
                                {
                                    m_LobbyServiceFacade.SetRemoteLobby(lobbyJoinAttemp.Lobby);
                                    if (m_LobbyServiceFacade.CurrentUnityLobby != null)
                                    {
                                        UnityEngine.Debug.Log($"Joined Lobby {lobbyJoinAttemp.Lobby.Name} ({lobbyJoinAttemp.Lobby.Id})");
                                        PlayerPrefs.SetString("JOINCODE", RelayJoinCode);
                                        m_ConnectionManager.StartClientLobby(ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.PlayerName);
                                        joined = true;
                                        isFound = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    // cross-arena 
                    if (!joined)
                    {
                        foreach (Lobby lobby in foundLobbies)
                        {
                            string RelayJoinCode = lobby.Data.ContainsKey("RelayJoinCode") ? lobby.Data["RelayJoinCode"].Value : null;
                            string Region = lobby.Data.ContainsKey("Region") ? lobby.Data["Region"].Value : null;
                            string HostLatency = lobby.Data.ContainsKey("Latency") ? lobby.Data["Latency"].Value : null;
                            if (RelayJoinCode == joinedCode || string.IsNullOrEmpty(RelayJoinCode) || string.IsNullOrEmpty(Region) || string.IsNullOrEmpty(HostLatency))
                                continue;
                            else
                            {
                                int _iMap = ConvertToMap(lobby.Data["GameMap"].Value);
                                if (ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.Coins >= ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.Arenas[_iMap + 1].costcoins && ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.Credits >= ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.Arenas[_iMap + 1].costcredits)
                                {
                                    var regions = new List<string>();
                                    regions.Add(Region);
                                    var qosResultsForRegion = await QosService.Instance.GetSortedQosResultsAsync("relay", regions);
                                    int ClientLatency = qosResultsForRegion[0].AverageLatencyMs;
                                    CheckLatency(ClientLatency);
                                    UnityEngine.Debug.Log("===>client latency ---> " + ClientLatency);
                                    int iHostLatency = 0;
                                    int.TryParse(HostLatency, out iHostLatency);
                                    if ((iHostLatency + ClientLatency) > ConnectionManager.LatencyLimit)
                                    {
                                        continue;
                                    }
                                    UnityEngine.Debug.Log("===>joined latency ---> " + (iHostLatency + ClientLatency));
                                    MatchmakingScreenController.Instance.SetMMStatus(MatchmakingScreenController.MMStatus.JOIN_LOBBY);
                                    var lobbyJoinAttemp = await m_LobbyServiceFacade.TryJoinLobbyAsync(lobbyId: lobby.Id, null);
                                    if (lobbyJoinAttemp.Success)
                                    {
                                        m_LobbyServiceFacade.SetRemoteLobby(lobbyJoinAttemp.Lobby);
                                        if (m_LobbyServiceFacade.CurrentUnityLobby != null)
                                        {
                                            UnityEngine.Debug.Log($"Joined Lobby {lobbyJoinAttemp.Lobby.Name} ({lobbyJoinAttemp.Lobby.Id})");
                                            ApplicationModelProvider.ApplicationModel.DataProvider.SaveGame();
                                            PlayerPrefs.SetString("JOINCODE", RelayJoinCode);
                                            m_ConnectionManager.StartClientLobby(ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.PlayerName);
                                            joined = true;
                                            isFound = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (!joined)
                    {
                        var qosResultsForRegion = await QosService.Instance.GetSortedQosResultsAsync("relay", null);
                        int averageLatency = qosResultsForRegion[0].AverageLatencyMs;
                        CheckLatency(averageLatency);
                        if (averageLatency > ConnectionManager.LatencyLimit / 2)
                            continue;
                        MatchmakingScreenController.Instance.SetMMStatus(MatchmakingScreenController.MMStatus.CREATING_LOBBY);
                        var lobbyData = new Dictionary<string, DataObject>()
                        {
                            ["GameMap"] = new DataObject(DataObject.VisibilityOptions.Public, wantMap, DataObject.IndexOptions.S1),
                            ["Score"] = new DataObject(DataObject.VisibilityOptions.Public, Mathf.FloorToInt(ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.BattleWinScore).ToString(), DataObject.IndexOptions.N1),
                        };
                        var lobbyCreationAttemp = await m_LobbyServiceFacade.TryCreateLobbyAsync(m_NameGenerationData.GenerateName(), m_ConnectionManager.MaxConnectedPlayers, isPrivate: false, m_LocalUser.GetDataForUnityServices(), lobbyData);
                        while (true)
                        {
                            if (lobbyCreationAttemp.Success)
                            {
                                m_LocalUser.IsHost = true;
                                m_LobbyServiceFacade.SetRemoteLobby(lobbyCreationAttemp.Lobby);
                                if (m_LobbyServiceFacade.CurrentUnityLobby != null)
                                {
                                    UnityEngine.Debug.Log($"Created new Lobby {lobbyCreationAttemp.Lobby.Name} ({lobbyCreationAttemp.Lobby.Id})");
                                    m_ConnectionManager.StartHostLobby(ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.PlayerName);
                                    isFound = true;
                                    break;
                                }
                                else
                                {
                                    lobbyCreationAttemp = await m_LobbyServiceFacade.TryCreateLobbyAsync(m_NameGenerationData.GenerateName(), m_ConnectionManager.MaxConnectedPlayers, isPrivate: false, m_LocalUser.GetDataForUnityServices(), lobbyData);
                                }
                            }
                            else
                            {
                                lobbyCreationAttemp = await m_LobbyServiceFacade.TryCreateLobbyAsync(m_NameGenerationData.GenerateName(), m_ConnectionManager.MaxConnectedPlayers, isPrivate: false, m_LocalUser.GetDataForUnityServices(), lobbyData);
                            }
                            await Task.Delay(1000);
                        }
                    }
                }
                else
                {
                    var qosResultsForRegion = await QosService.Instance.GetSortedQosResultsAsync("relay", null);
                    int averageLatency = qosResultsForRegion[0].AverageLatencyMs;
                    CheckLatency(averageLatency);
                    if (averageLatency > ConnectionManager.LatencyLimit / 2)
                        continue;
                    
                    var lobbyData = new Dictionary<string, DataObject>()
                    {
                        ["GameMap"] = new DataObject(DataObject.VisibilityOptions.Public, wantMap, DataObject.IndexOptions.S1),
                        ["Score"] = new DataObject(DataObject.VisibilityOptions.Public,Mathf.FloorToInt(ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.BattleWinScore).ToString(), DataObject.IndexOptions.N1),
                    };
                    MatchmakingScreenController.Instance.SetMMStatus(MatchmakingScreenController.MMStatus.CREATING_LOBBY);
                    var lobbyCreationAttemp = await m_LobbyServiceFacade.TryCreateLobbyAsync(m_NameGenerationData.GenerateName(), m_ConnectionManager.MaxConnectedPlayers, isPrivate: false, m_LocalUser.GetDataForUnityServices(), lobbyData);
                    while (true)
                    {
                        if (lobbyCreationAttemp.Success)
                        {
                            m_LocalUser.IsHost = true;
                            m_LobbyServiceFacade.SetRemoteLobby(lobbyCreationAttemp.Lobby);
                            if (m_LobbyServiceFacade.CurrentUnityLobby != null)
                            {
                                UnityEngine.Debug.Log($"Created new Lobby {lobbyCreationAttemp.Lobby.Name} ({lobbyCreationAttemp.Lobby.Id})");
                                m_ConnectionManager.StartHostLobby(ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.PlayerName);
                                isFound = true;
                                break;
                            }
                            else
                            {
                                lobbyCreationAttemp = await m_LobbyServiceFacade.TryCreateLobbyAsync(m_NameGenerationData.GenerateName(), m_ConnectionManager.MaxConnectedPlayers, isPrivate: false, m_LocalUser.GetDataForUnityServices(), lobbyData);
                            }
                        }
                        else
                        {
                            lobbyCreationAttemp = await m_LobbyServiceFacade.TryCreateLobbyAsync(m_NameGenerationData.GenerateName(), m_ConnectionManager.MaxConnectedPlayers, isPrivate: false, m_LocalUser.GetDataForUnityServices(), lobbyData);
                        }
                        await Task.Delay(1000);
                    }
                }
                if (isFound)
                    break;
                await Task.Delay(1000);
            }
        }

        void CheckLatency(int latency)
        {
            if (latency < 50)
            {
                MatchmakingScreenController.Instance.Connection_Quality = MatchmakingScreenController.ConnectionQuality.HIGH;
                return;
            }
                
            if(latency < 100)
            {
                MatchmakingScreenController.Instance.Connection_Quality = MatchmakingScreenController.ConnectionQuality.MID;
                return;
            }

            if (latency < 150)
            {
                MatchmakingScreenController.Instance.Connection_Quality = MatchmakingScreenController.ConnectionQuality.LOW;
                return;
            }
            MatchmakingScreenController.Instance.Connection_Quality = MatchmakingScreenController.ConnectionQuality.DEAD;
        }

        string ConvertToScene(Map map)
        {
            switch (map)
            {
                case Map.PracticeWreckyards:
                    return "PracticeWreckyards";
                case Map.OzPenitentiary:
                    return "OzPenitentiary";
                case Map.SanFranciscoFightClub:
                    return "SanFranciscoFightClub";
                case Map.UACBattleNight:
                    return "UACBattleNight";
                case Map.NuclearDome:
                    return "NuclearDome";
                case Map.UACArena:
                    return "UACArena";
                case Map.RioBattlesport:
                    return "RioBattlesport";
                case Map.UACUltimate:
                    return "UACUltimate";
                case Map.MercenaryOne:
                    return "MercenaryOne";
                default:
                    return "PvPBattleScene";
            }
        }
        int ConvertToMap(string map)
        {
            switch (map)
            {
                case "PracticeWreckyards":
                    return 0;
                case "OzPenitentiary":
                    return 1;
                case "SanFranciscoFightClub":
                    return 2;
                case "UACBattleNight":
                    return 3;
                case "NuclearDome":
                    return 4;
                case "UACArena":
                    return 5;
                case "RioBattlesport":
                    return 6;
                case "UACUltimate":
                    return 7;
                case "MercenaryOne":
                    return 8;
                default:
                    return 0;
            }
        }
        IEnumerator iStartPvP()
        {
            yield return new WaitForEndOfFrame();

            if (m_LocalLaunchMode)
            {
                JoinWithIP("127.0.0.1", "7777");
            }
            else
            {
                JoinWithLobby();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            //   TrySignIn();
        }



        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
        }

        void OnDestroy()
        {
            m_ProfileManager.onProfileChanged -= OnProfileChanged;
            m_LobbyServiceFacade.OnMatchMakingFailed -= onMatchmakingFailed;
            m_LobbyServiceFacade.OnMatchMakingStarted -= onMatchmakingStarted;
        }

        async void OnProfileChanged()
        {
            await m_AuthServiceFacade.SwitchProfileAndReSignInAsync(m_ProfileManager.Profile);

            UnityEngine.Debug.Log($"Signed in. Unity Player ID {AuthenticationService.Instance.PlayerId}");

            // Updating LocalUser and LocalLobby
            m_LocalLobby.RemoveUser(m_LocalUser);
            m_LocalUser.ID = AuthenticationService.Instance.PlayerId;
            m_LocalLobby.AddUser(m_LocalUser);
        }

        private async void Start()
        {
            /*            if (NetworkManager.Singleton.IsConnectedClient)  // I am not sure, PvPBootScene.unity is being loaded twice
                            return;*/
            if (Instance == null)
                Instance = this;
            Helper.AssertIsNotNull(_uiAudioSource, trashDataList);
            Logging.Log(Tags.Multiplay_SCREENS_SCENE_GOD, "START");

            ILocTable commonStrings = await LocTableFactory.Instance.LoadCommonTableAsync();
            ILocTable storyStrings = await LocTableFactory.Instance.LoadStoryTableAsync();

            trashDataList.Initialise(storyStrings);
            ITrashTalkData trashTalkData = await trashDataList.GetTrashTalkAsync(/*_gameModel.SelectedLevel*/1);
            MatchmakingScreenController.Instance.SetTraskTalkData(trashTalkData, commonStrings, storyStrings);

            // cheat code for local test
            k_DefaultLobbyName = m_NameGenerationData.GenerateName();

            m_LobbyServiceFacade.OnMatchMakingFailed += onMatchmakingFailed;
            m_LobbyServiceFacade.OnMatchMakingStarted += onMatchmakingStarted;

            StartCoroutine(iStartPvP());
        }
    }
}

