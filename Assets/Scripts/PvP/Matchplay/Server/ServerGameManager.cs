#if UNITY_SERVER || UNITY_EDITOR
using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Matchmaker.Models;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using Random = UnityEngine.Random;


namespace BattleCruisers.Network.Multiplay.Matchplay.Server
{
    public class ServerGameManager : IDisposable
    {

        public bool StartedServices => m_StartedServices;
        bool m_StartedServices;

        public MatchplayNetworkServer NetworkServer => m_NetworkServer;
        public SynchedServerData ServerData => m_SynchedServerData;


        MatchplayBackfiller m_Backfiller;
        string connectionString => $"{m_ServerIP}:{m_ServerPort}";
        string m_ServerIP = "0.0.0.0";
        int m_ServerPort = 7777;
        int m_QueryPort = 7787;
        const int k_MultiplyServiceTimeout = 20000;
        MultiplayAllocationService m_MultiplayAllocationService;
        MultiplayServerQueryService m_MultiplayServerQueryService;


        MatchplayNetworkServer m_NetworkServer;
        SynchedServerData m_SynchedServerData;
        string m_ServerName = "BC Matchplay Server";


        public ServerGameManager(string serverIP, int serverPort, int serverQPort, NetworkManager manager)
        {
            m_ServerIP = serverIP;
            m_ServerPort = serverPort;
            m_QueryPort = serverQPort;
            m_NetworkServer = new MatchplayNetworkServer(manager);
            m_MultiplayAllocationService = new MultiplayAllocationService();
            m_MultiplayServerQueryService = new MultiplayServerQueryService();
            m_ServerName = "RandomeServerName" + Random.Range(1, 9999).ToString();
        }

        public async Task StartGameServerAsync(GameInfo startingGameInfo)
        {
            Debug.Log($"starting server with: {startingGameInfo}");
            await m_MultiplayServerQueryService.BeginServerQueryHandler();
            try
            {
                var matchmakerPayload = await GetMatchmakerPayload(k_MultiplyServiceTimeout);
                if (matchmakerPayload != null)
                {
                    Debug.Log($"Got payload: {matchmakerPayload}");
                    startingGameInfo = PickGameInfo(matchmakerPayload);

                    MatchStartedServerQuery(startingGameInfo);
                    await StartBackfill(matchmakerPayload, startingGameInfo);
                    m_NetworkServer.OnPlayerJoined += UserJoinedServer;
                    m_NetworkServer.OnPlayerLeft += UserLeft;
                    m_StartedServices = true;
                }
                else
                {
                    Debug.LogWarning("Getting the Matchmaker Payload timed out, starting with defaults");
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Something went wrong trying to set up the Services: \n{ex}");
            }

            if (!m_NetworkServer.OpenConnection(m_ServerIP, m_ServerPort, startingGameInfo))
            {
                Debug.LogError("NetworkServer did not start as expected.");
                return;
            }


            m_SynchedServerData = await m_NetworkServer.ConfigureServer(startingGameInfo);
            if (m_SynchedServerData == null)
            {
                Debug.LogError("Could not find the synchedServerData.");
                return;
            }
            m_SynchedServerData.serverID.Value = m_ServerName;

            m_SynchedServerData.map.OnValueChanged += OnServerChangedMap;
            m_SynchedServerData.gameMode.OnValueChanged += OnServerChangedMode;



        }


        async Task<MatchmakingResults> GetMatchmakerPayload(int timeout)
        {
            if (m_MultiplayAllocationService == null)
            {
                return null;
            }
            var matchmakerPayloadTask = m_MultiplayAllocationService.SubscribeAndWaitMatchmakerAllocation();
            if (await Task.WhenAny(matchmakerPayloadTask, Task.Delay(timeout)) == matchmakerPayloadTask)
            {
                return matchmakerPayloadTask.Result;
            }
            return null;
        }

        private void MatchStartedServerQuery(GameInfo startingGameInfo)
        {
            m_MultiplayServerQueryService.SetServerName(m_ServerName);
            m_MultiplayServerQueryService.SetMaxPlayers(10);
            m_MultiplayServerQueryService.SetBuildID("0");
            m_MultiplayServerQueryService.SetMap(startingGameInfo.map.ToString());
            m_MultiplayServerQueryService.SetMode(startingGameInfo.gameMode.ToString());
        }


        async Task StartBackfill(MatchmakingResults payload, GameInfo startingGameinfo)
        {
            m_Backfiller = new MatchplayBackfiller(connectionString, payload.QueueName, payload.MatchProperties, startingGameinfo.MaxUsers);
            if (m_Backfiller.NeedsPlayers())
            {
                await m_Backfiller.BeginBackfillng();
            }

        }

        #region ServerSynching

        void OnServerChangedMap(Map oldMap, Map newMap)
        {
            m_MultiplayServerQueryService.SetMap(newMap.ToString());
        }

        void OnServerChangedMode(GameMode oldMode, GameMode newMode)
        {
            m_MultiplayServerQueryService.SetMode(newMode.ToString());
        }

        void UserJoinedServer(UserData joinedUser)
        {
            Debug.Log($"{joinedUser} joined the game");
            m_Backfiller.AddPlayerToMatch(joinedUser);
            m_MultiplayServerQueryService.AddPlayer();
            if (!m_Backfiller.NeedsPlayers() && m_Backfiller.Backfilling)
            {
#pragma warning disable 4014
                m_Backfiller.StopBackfill();
#pragma warning restore 4014

            }
        }


        void UserLeft(UserData leftUser)
        {
            var playerCount = m_Backfiller.RemovePlayerFormMatch(leftUser.userAuthId);
            m_MultiplayServerQueryService.RemovePlayer();

            Debug.Log($"player '{leftUser?.userName}' left the game, {playerCount} players left in game.");
            if (playerCount <= 0)
            {
#pragma warning disable 4014
                CloseServer();
#pragma warning restore 4014
                return;
            }

            if (m_Backfiller.NeedsPlayers() && !m_Backfiller.Backfilling)
            {
#pragma warning disable 4014
                m_Backfiller.BeginBackfillng();
#pragma warning restore 4014
            }
        }

        #endregion


        public static GameInfo PickGameInfo(MatchmakingResults mmAllocation)
        {
            var chosenMap = Map.PracticeWreckyards;
            var chosenMode = GameMode.Starting;

            foreach (var player in mmAllocation.MatchProperties.Players)
            {
                var playerGameInfo = player.CustomData.GetAs<GameInfo>();
                chosenMap = playerGameInfo.map;
                chosenMode = playerGameInfo.gameMode;
            }

            var queue = GameInfo.ToGameQueue(mmAllocation.QueueName);
            return new GameInfo { map = chosenMap, gameMode = chosenMode, gameQueue = queue };
        }

        async Task CloseServer()
        {
            Debug.Log($"Closing Server");
            await m_Backfiller.StopBackfill();
            Dispose();
            Application.Quit();
        }

        public void Dispose()
        {
            if (!m_StartedServices)
            {
                if (m_NetworkServer.OnPlayerJoined != null) m_NetworkServer.OnPlayerJoined -= UserJoinedServer;
                if (m_NetworkServer.OnPlayerLeft != null) m_NetworkServer.OnPlayerLeft -= UserLeft;
            }

            if (m_SynchedServerData != null)
            {
                if (m_SynchedServerData.map.OnValueChanged != null)
                    m_SynchedServerData.map.OnValueChanged -= OnServerChangedMap;
                if (m_SynchedServerData.gameMode.OnValueChanged != null)
                    m_SynchedServerData.gameMode.OnValueChanged -= OnServerChangedMode;
            }

            m_Backfiller?.Dispose();
            m_MultiplayAllocationService?.Dispose();
            NetworkServer?.Dispose();
        }
    }
}
#endif
