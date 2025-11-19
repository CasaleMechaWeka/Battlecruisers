using System;
using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.Utils;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;
using BattleCruisers.Network.Multiplay.UnityServices.Lobbies;
using BattleCruisers.Data;
using Unity.Services.Qos;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.ConnectionManagement
{
    /// <summary>
    /// ConnectionMethod contains all setup needed to setup NGO to be ready to start a connection, either host or client side.
    /// Please override this abstract class to add a new transport or way of connecting.
    /// </summary>
    public abstract class ConnectionMethodBase
    {
        protected ConnectionManager m_ConnectionManager;
        readonly ProfileManager m_ProfileManager;
        protected readonly string m_PlayerName;

        public abstract Task SetupHostConnectionAsync();

        public abstract Task SetupClientConnectionAsync();


        public ConnectionMethodBase(ConnectionManager connectionManager, ProfileManager profileManager, string playerName)
        {
            m_ConnectionManager = connectionManager;
            m_ProfileManager = profileManager;
            m_PlayerName = playerName;
        }



        protected void SetConnectionPayload(string playerId, string playerName)
        {
            string payload = JsonUtility.ToJson(new ConnectionPayload()
            {
                playerId = playerId,
                playerName = playerName,
                playerHullPrefabName = DataProvider.GameModel.PlayerLoadout.Hull.PrefabName,
                playerScore = DataProvider.GameModel.LifetimeDestructionScore,
                playerNetworkId = 0,
                playerCaptainPrefabName = DataProvider.GameModel.PlayerLoadout.CurrentCaptain.PrefabName,
                playerGameMap = DataProvider.GameModel.GameMap,
                playerRating = DataProvider.GameModel.BattleWinScore,
                playerBodykit = DataProvider.GameModel.PlayerLoadout.SelectedBodykit,
                playerBounty = DataProvider.GameModel.Bounty,
            });

            byte[] payloadBytes = System.Text.Encoding.UTF8.GetBytes(payload);

            m_ConnectionManager.NetworkManager.NetworkConfig.ConnectionData = payloadBytes;
        }

        protected string GetPlayerId()
        {
            if (Unity.Services.Core.UnityServices.State != ServicesInitializationState.Initialized)
            {
                return ClientPrefs.GetGuid() + m_ProfileManager.Profile;
            }

            return AuthenticationService.Instance.IsSignedIn ? AuthenticationService.Instance.PlayerId : ClientPrefs.GetGuid() + m_ProfileManager.Profile;
        }
    }

    /// <summary>
    /// Simple IP connection setup with UTP
    /// </summary>
    class ConnectionMethodIP : ConnectionMethodBase
    {
        string m_Ipaddress;
        ushort m_Port;

        public ConnectionMethodIP(string ip, ushort port, ConnectionManager connectionManager, ProfileManager profileManager, string playerName)
        : base(connectionManager, profileManager, playerName)
        {
            m_Ipaddress = ip;
            m_Port = port;
            m_ConnectionManager = connectionManager;
        }

        public override async Task SetupClientConnectionAsync()
        {
            SetConnectionPayload(GetPlayerId(), m_PlayerName);
            var utp = (UnityTransport)m_ConnectionManager.NetworkManager.NetworkConfig.NetworkTransport;
            utp.SetConnectionData(m_Ipaddress, m_Port);
        }

        public override async Task SetupHostConnectionAsync()
        {
            SetConnectionPayload(GetPlayerId(), m_PlayerName); // Need to set connection payload for host as well, as host is a client too
            var utp = (UnityTransport)m_ConnectionManager.NetworkManager.NetworkConfig.NetworkTransport;
            utp.SetConnectionData(m_Ipaddress, m_Port);
        }
    }

    /// <summary>
    /// UTP's Relay connection setup
    /// </summary>
    class ConnectionMethodLobby : ConnectionMethodBase
    {
        LobbyServiceFacade m_LobbyServiceFacade;
        LocalLobby m_LocalLobby;

        public ConnectionMethodLobby(LobbyServiceFacade lobbyServiceFacade, LocalLobby localLobby, ConnectionManager connectionManager, ProfileManager profileManager, string playerName)
        : base(connectionManager, profileManager, playerName)
        {
            m_LobbyServiceFacade = lobbyServiceFacade;
            m_LocalLobby = localLobby;
            m_ConnectionManager = connectionManager;
        }


        public override async Task SetupClientConnectionAsync()
        {
            // 1-second delay to allow HOST lobby data (relay join code) to propagate through Unity services
            await Task.Delay(1000);

            Debug.Log("Setting up Unity Relay client");
            SetConnectionPayload(GetPlayerId(), m_PlayerName);
            if (m_LobbyServiceFacade.CurrentUnityLobby == null)
            {
                throw new Exception("Trying to start relay while Lobby isn't set");
            }

            Debug.Log($"Setting Unity Relay client with join code {m_LocalLobby.RelayJoinCode}");

            // Create client joining allocation from join code
            var joinedAllocation = await RelayService.Instance.JoinAllocationAsync(m_LocalLobby.RelayJoinCode);
            Debug.Log($"client: {joinedAllocation.ConnectionData[0]} {joinedAllocation.ConnectionData[1]}, " +
            $"host: {joinedAllocation.HostConnectionData[0]} {joinedAllocation.HostConnectionData[1]}, " +
            $"client: {joinedAllocation.AllocationId}");

            await m_LobbyServiceFacade.UpdatePlayerRelayInfoAsync(joinedAllocation.AllocationId.ToString(), m_LocalLobby.RelayJoinCode);

            // Configure UTP with allocation
            var utp = (UnityTransport)m_ConnectionManager.NetworkManager.NetworkConfig.NetworkTransport;
            utp.SetRelayServerData(new RelayServerData(joinedAllocation, OnlineState.k_DtlsConnType));
        }
        public override async Task SetupHostConnectionAsync()
        {
            Debug.Log("Setting up Unity Relay host");
            SetConnectionPayload(GetPlayerId(), m_PlayerName);

            Allocation hostAllocation;
            string joinCode;

            if (m_LocalLobby.CachedRelayAllocation != null)
            {
                Debug.Log($"PVP: Reusing cached relay allocation (AllocationId={m_LocalLobby.CachedRelayAllocation.AllocationId})");
                hostAllocation = m_LocalLobby.CachedRelayAllocation;
                joinCode = m_LocalLobby.RelayJoinCode;
            }
            else
            {
                System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

                hostAllocation = await RelayService.Instance.CreateAllocationAsync(m_ConnectionManager.MaxConnectedPlayers, region: null);
                joinCode = await RelayService.Instance.GetJoinCodeAsync(hostAllocation.AllocationId);
                Debug.Log($"server: connection data: {hostAllocation.ConnectionData[0]} {hostAllocation.ConnectionData[1]}, allocation ID:{hostAllocation.AllocationId}, region:{hostAllocation.Region}");

                List<string> regions = new List<string> { hostAllocation.Region };
                IList<IQosResult> qosResultsForRegion = await QosService.Instance.GetSortedQosResultsAsync("relay", regions);
                int averageLatency = qosResultsForRegion[0].AverageLatencyMs;
                Debug.Log("===>host latency ---> " + averageLatency);

                if (averageLatency > ConnectionManager.LatencyLimit / 2)
                    throw new Exception();

                sw.Stop();
                Debug.Log($"PVP: BASELINE relay allocation took {sw.ElapsedMilliseconds}ms");

                m_LocalLobby.RelayJoinCode = joinCode;
                m_LocalLobby.Region = hostAllocation.Region;
                m_LocalLobby.Latency = averageLatency.ToString();
                m_LocalLobby.CachedRelayAllocation = hostAllocation;
            }

            await m_LobbyServiceFacade.UpdatePlayerRelayInfoAsync(hostAllocation.AllocationId.ToString(), joinCode);

            UnityTransport utp = (UnityTransport)m_ConnectionManager.NetworkManager.NetworkConfig.NetworkTransport;
            utp.SetRelayServerData(new RelayServerData(hostAllocation, OnlineState.k_DtlsConnType));

            await m_LobbyServiceFacade.UpdateLobbyDataAsync(m_LocalLobby.GetDataForUnityServices());
        }
    }
}
