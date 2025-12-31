using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.Infrastructure;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.UnityServices.Lobbies
{
    /// <summary>
    /// An abstraction layer between the direct calls into the Lobby API and the outcomes you actually want.
    /// </summary>
    public class LobbyServiceFacade
    {
        UpdateRunner m_UpdateRunner;

        LocalLobby m_LocalLobby;
        LocalLobbyUser m_LocalUser;
        IPublisher<UnityServiceErrorMessage> m_UnityServiceErrorMessagePub;
        IPublisher<LobbyListFetchedMessage> m_LobbyListFetchedPub;


        const float k_HeartbeatPeriod = 8; // The heartbeat must be rate-limited to 5 calls per 30 seconds. We'll aim for longer in case periods don't align.
        float m_HeartbeatTime = 0;

        LobbyAPIInterface m_LobbyApiInterface;
        JoinedLobbyContentHeartbeat m_JoinedLobbyContentHeartbeat;

        RateLimitCooldown m_RateLimitQuery;
        RateLimitCooldown m_RateLimitJoin;
        RateLimitCooldown m_RateLimitQuickJoin;
        RateLimitCooldown m_RateLimitHost;
        RateLimitCooldown m_RateLimitLobbyQuery;

        public Lobby CurrentUnityLobby { get; private set; }

        bool m_IsTracking = false;

        public Action OnMatchMakingFailed;

        public LobbyServiceFacade(
            UpdateRunner updateRunner,
            LocalLobby localLobby,
            LocalLobbyUser localUser,
            IPublisher<UnityServiceErrorMessage> unityServiceErrorMessagePub,
            IPublisher<LobbyListFetchedMessage> lobbyListFetchedPub)
        {
            m_UpdateRunner = updateRunner;
            m_LocalLobby = localLobby;
            m_LocalUser = localUser;
            m_UnityServiceErrorMessagePub = unityServiceErrorMessagePub;
            m_LobbyListFetchedPub = lobbyListFetchedPub;

            m_JoinedLobbyContentHeartbeat
             = new JoinedLobbyContentHeartbeat(
                m_LocalLobby,
                m_LocalUser,
                m_UpdateRunner,
                this);

            m_LobbyApiInterface = new LobbyAPIInterface();

            //See https://docs.unity.com/lobby/rate-limits.html
            m_RateLimitQuery = new RateLimitCooldown(1f);
            m_RateLimitJoin = new RateLimitCooldown(3f);
            m_RateLimitQuickJoin = new RateLimitCooldown(10f);
            m_RateLimitHost = new RateLimitCooldown(3f);
            m_RateLimitLobbyQuery = new RateLimitCooldown(30f);
        }

        public void SetRemoteLobby(Lobby lobby)
        {
            CurrentUnityLobby = lobby;
            m_LocalLobby.ApplyRemoteData(lobby);
        }

        public void BeginTracking()
        {
            if (!m_IsTracking)
            {
                m_IsTracking = true;
                m_RateLimitLobbyQuery.PutOnCooldown();
                m_UpdateRunner.Subscribe(UpdateLobby, 3f);
                m_JoinedLobbyContentHeartbeat.BeginTracking();
            }
        }

        public void PauseTracking()
        {
            if (m_IsTracking)
            {
                m_UpdateRunner.Unsubscribe(UpdateLobby);
                m_JoinedLobbyContentHeartbeat.EndTracking();
                m_IsTracking = false;
            }
        }
        public async void LockLobby()
        {
            if (CurrentUnityLobby != null)
            {
                var dataCurr = CurrentUnityLobby.Data ?? new Dictionary<string, DataObject>();
                var result = await m_LobbyApiInterface.UpdateLobbyWithPrivate(CurrentUnityLobby.Id, dataCurr, isPrivate: true);
                if (result != null)
                {
                    CurrentUnityLobby = result;
                    m_LocalLobby.ApplyRemoteData(result);
                }
            }
        }
        public Task EndTracking()
        {
            var task = Task.CompletedTask;
            if (this == null)
                return task;
            if (CurrentUnityLobby != null)
            {
                CurrentUnityLobby = null;

                var lobbyId = m_LocalLobby?.LobbyID;

                if (!string.IsNullOrEmpty(lobbyId))
                {
                    if (m_LocalUser.IsHost)
                    {
                        task = DeleteLobbyAsync(lobbyId);
                    }
                    else
                    {
                        task = LeaveLobbyAsync(lobbyId);
                    }
                }
                m_LocalUser.ResetState();
                m_LocalLobby?.Reset(m_LocalUser);
            }

            if (m_IsTracking)
            {
                m_UpdateRunner.Unsubscribe(UpdateLobby);
                m_IsTracking = false;
                m_HeartbeatTime = 0;
                m_JoinedLobbyContentHeartbeat.EndTracking();
            }

            return task;
        }


        async void UpdateLobby(float unused)
        {
            if (!m_RateLimitQuery.CanCall)
            {
                return;
            }

            try
            {
                var lobby = await m_LobbyApiInterface.GetLobby(m_LocalLobby.LobbyID);
                if (lobby == null)
                    return;
                CurrentUnityLobby = lobby;
                m_LocalLobby.ApplyRemoteData(lobby);
                // as client, check if host is still in lobby
                if (m_LocalUser.IsHost)
                {
                    if (m_LocalLobby.PlayerCount == 2)
                    {
                        /*                        var dataCurr = CurrentUnityLobby.Data ?? new Dictionary<string, DataObject>();
                                                var result = await m_LobbyApiInterface.UpdateLobbyWithPrivate(CurrentUnityLobby.Id, dataCurr, isPrivate: true);
                                                if (result != null)
                                                {
                                                    CurrentUnityLobby = result;
                                                }*/
                    }
                }
                else
                {
                    foreach (var lobbyUser in m_LocalLobby.LobbyUsers)
                    {
                        if (lobbyUser.Value.IsHost)
                        {
                            return;
                        }
                    }
                    await EndTracking();
                }
            }
            catch /*(LobbyServiceException e)*/
            {
                /*                if (e.Reason == LobbyExceptionReason.RateLimited)
                                {
                                    m_RateLimitQuery.PutOnCooldown();
                                }
                                else if (e.Reason != LobbyExceptionReason.LobbyNotFound && !m_LocalUser.IsHost) // If Lobby is not found and if we are not the host, it has already been deleted. No need to publish the error here.
                                {
                                    PublishError(e);
                                }*/
            }
        }

        /// <summary>
        /// Attempt to create a new lobby and then join it.
        /// </summary>
        public async Task<(bool Success, Lobby Lobby)> TryCreateLobbyAsync(string lobbyName, int maxPlayers, bool isPrivate, Dictionary<string, PlayerDataObject> hostUserData, Dictionary<string, DataObject> lobbyData)
        {
            if (!m_RateLimitHost.CanCall)
            {
                Debug.LogWarning("Create Lobby hit the rate limit.");
                return (false, null);
            }

            try
            {
                var lobby = await m_LobbyApiInterface.CreateLobby(AuthenticationService.Instance.PlayerId, lobbyName, maxPlayers, isPrivate, hostUserData, lobbyData);
                if (lobby != null)
                {
                    return (true, lobby);
                }
                else
                {
                    return (false, lobby);
                }
            }
            catch (Exception e) /*(LobbyServiceException e)*/
            {
                /*                if (e.Reason == LobbyExceptionReason.RateLimited)
                                {
                                    m_RateLimitHost.PutOnCooldown();
                                }
                                else
                                {
                                    PublishError(e);
                                }*/
                Debug.Log(e.Message);
            }
            return (false, null);
        }
        public async Task<(bool Success, Lobby Lobby)> TryJoinLobbyAsync(string lobbyId, string lobbyCode)
        {
            if (!m_RateLimitJoin.CanCall ||
            (lobbyId == null && lobbyCode == null))
            {
                Debug.LogWarning("Join Lobby hit the rate limit.");
                return (false, null);
            }

            Debug.Log("PVP: LobbyServiceFacade.TryJoinLobbyAsync - JOIN code: " + lobbyCode);

            try
            {
                if (!string.IsNullOrEmpty(lobbyCode))
                {
                    var lobby = await m_LobbyApiInterface.JoinLobbyByCode(AuthenticationService.Instance.PlayerId, lobbyCode, m_LocalUser.GetDataForUnityServices());
                    Debug.Log("PVP: LobbyServiceFacade.TryJoinLobbyAsync - JoinLobbyByCode returned " + (lobby != null));
                    if (lobby != null)
                        return (true, lobby);
                    else
                        return (false, null);
                }
                else
                {
                    var lobby = await m_LobbyApiInterface.JoinLobbyById(AuthenticationService.Instance.PlayerId, lobbyId, m_LocalUser.GetDataForUnityServices());
                    return (true, lobby);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"PVP: Failed to join lobby (lobby may have closed or be full): {e.Message}");
            }

            return (false, null);
        }
        /// <summary>
        /// Attempt to join the first lobby among the available lobbies that match the filtered onlineMode.
        /// </summary>
        public async Task<(bool Success, Lobby Lobby)> TryQuickJoinLobbyAsync(List<QueryFilter> m_Filters)
        {
            if (!m_RateLimitQuickJoin.CanCall)
            {
                Debug.LogWarning("Quick Join Lobby hit the rate limit.");
                return (false, null);
            }
            try
            {
                var lobby = await m_LobbyApiInterface.QuickJoinLobby(AuthenticationService.Instance.PlayerId, m_LocalUser.GetDataForUnityServices(), m_Filters);

                return (true, lobby);
            }
            catch /*(LobbyServiceException e)*/
            {
                // Debug.Log(e);
                /*                if (e.Reason == LobbyExceptionReason.RateLimited)
                                {
                                    m_RateLimitQuickJoin.PutOnCooldown();
                                }*/
                // else
                // {
                //     PublishError(e);
                // }
            }

            return (false, null);
        }


        public async Task<QueryResponse> QueryLobbyListAsync(List<QueryFilter> mFilters, List<QueryOrder> mOrders)
        {
            if (!m_RateLimitQuery.CanCall)
            {
                Debug.LogWarning("Retrieve Lobby list hit the rate limit. Will try again soon...");
                return null;
            }

            try
            {
                return await m_LobbyApiInterface.QueryAllLobbies(mFilters, mOrders);
            }
            catch /*(LobbyServiceException e)*/
            {
                /*                if (e.Reason == LobbyExceptionReason.RateLimited)
                                {
                                    m_RateLimitQuery.PutOnCooldown();
                                }
                                else
                                {
                                    PublishError(e);
                                }*/
            }
            return null;
        }

        /// <summary>
        /// Used for getting the list of all active lobbies, without needing full info for each.
        /// </summary>
        public async Task RetrieveAndPublishLobbyListAsync()
        {
            if (!m_RateLimitQuery.CanCall)
            {
                Debug.LogWarning("Retrieve Lobby list hit the rate limit. Will try again soon...");
                return;
            }

            try
            {
                //cheat code, should be modified!!!
                var response = await m_LobbyApiInterface.QueryAllLobbies(null, null);
                m_LobbyListFetchedPub.Publish(new LobbyListFetchedMessage(LocalLobby.CreateLocalLobbies(response)));
            }
            catch /*(LobbyServiceException e)*/
            {
                /*                if (e.Reason == LobbyExceptionReason.RateLimited)
                                {
                                    m_RateLimitQuery.PutOnCooldown();
                                }
                                else
                                {
                                    PublishError(e);
                                }*/
            }
        }

        public async Task<Lobby> ReconnectToLobbyAsync(string lobbyId)
        {
            try
            {
                return await m_LobbyApiInterface.ReconnectToLobby(lobbyId);
            }
            catch /*(LobbyServiceException e)*/
            {
                // If Lobby is not found and if we are not the host, it has already been deleted. No need to publish the error here.
                /*               if (e.Reason != LobbyExceptionReason.LobbyNotFound && !m_LocalUser.IsHost)
                               {
                                   PublishError(e);
                               }*/
            }

            return null;
        }

        /// <summary>
        /// Attempt to leave a lobby
        /// </summary>
        public async Task LeaveLobbyAsync(string lobbyId)
        {
            string uasId = AuthenticationService.Instance.PlayerId;
            try
            {
                if (this == null)
                    return;
                await m_LobbyApiInterface.RemovePlayerFromLobby(uasId, lobbyId);
            }
            catch /*(LobbyServiceException e)*/
            {
                // If Lobby is not found and if we are not the host, it has already been deleted. No need to publish the error here.
                /*            if (e.Reason != LobbyExceptionReason.LobbyNotFound && !m_LocalUser.IsHost)
                            {
                                PublishError(e);
                            }*/
            }

        }

        public async void RemovePlayerFromLobbyAsync(string uasId, string lobbyId)
        {
            if (m_LocalUser.IsHost)
            {
                try
                {
                    await m_LobbyApiInterface.RemovePlayerFromLobby(uasId, lobbyId);
                }
                catch /*(LobbyServiceException e)*/
                {
                    // PublishError(e);
                }
            }
            else
            {
                Debug.Log("Only the host can remove other players from the lobby.");
            }
        }

        public async Task DeleteLobbyAsync(string lobbyId)
        {
            if (m_LocalUser.IsHost)
            {
                try
                {
                    if (this == null)
                        return;
                    await m_LobbyApiInterface.DeleteLobby(lobbyId);
                }
                catch /*(LobbyServiceException e)*/
                {
                    //  PublishError(e);
                }
            }
            else
            {
                Debug.Log("PVP: LobbyServiceFacade.DeleteLobbyAsync - Only the host can delete a lobby.");
            }
        }
        /// <summary>
        /// Attempt to push a set of key-value pairs associated with the local player which will overwrite any existing data for these keys.
        /// </summary>
        public async Task UpdatePlayerDataAsync(Dictionary<string, PlayerDataObject> data)
        {
            if (!m_RateLimitQuery.CanCall)
            {
                return;
            }
            try
            {
                var result = await m_LobbyApiInterface.UpdatePlayer(CurrentUnityLobby.Id, AuthenticationService.Instance.PlayerId, data, null, null);

                if (result != null)
                {
                    CurrentUnityLobby = result;
                    m_LocalLobby.ApplyRemoteData(result);
                }
            }
            catch
            {

            }
        }
        /// <summary>
        /// Lobby can be provided info about Relay (or any other remote allocation) so it can add automatic disconnect handling.
        /// </summary>
        public async Task UpdatePlayerRelayInfoAsync(string allocationId, string connectionInfo)
        {
            if (!m_RateLimitQuery.CanCall)
                return;

            try
            {
                await m_LobbyApiInterface.UpdatePlayer(CurrentUnityLobby.Id, AuthenticationService.Instance.PlayerId, new Dictionary<string, PlayerDataObject>(), allocationId, connectionInfo);
            }
            catch
            {
                /*                if (e.Reason == LobbyExceptionReason.RateLimited)
                                {
                                    m_RateLimitQuery.PutOnCooldown();
                                }
                                else
                                {
                                    PublishError(e);
                                }*/

                //todo - retry logic? SDK is supposed to handle this eventually
            }
        }
        public async Task UpdateLobbyDataAsync(Dictionary<string, DataObject> data)
        {
            if (!m_RateLimitQuery.CanCall)
                return;

            if (CurrentUnityLobby == null)
                return;
            var dataCurr = CurrentUnityLobby.Data ?? new Dictionary<string, DataObject>();

            string lobbyDataSummary = string.Join(", ", dataCurr.Select(kvp => $"{kvp.Key}={kvp.Value?.Value?.ToString() ?? "NULL"}"));
            Debug.Log($"PVP: UpdateLobbyData ({lobbyDataSummary})");

            foreach (var dataNew in data)
                if (dataCurr.ContainsKey(dataNew.Key))
                    dataCurr[dataNew.Key] = dataNew.Value;
                else
                    dataCurr.Add(dataNew.Key, dataNew.Value);

            try
            {
                var result = await m_LobbyApiInterface.UpdateLobby(CurrentUnityLobby.Id, dataCurr, shouldLock: false);

                if (result != null)
                {
                    CurrentUnityLobby = result;
                    m_LocalLobby.ApplyRemoteData(result);
                }
            }
            catch
            {

            }
        }/// <summary>
         /// Lobby requires a periodic ping to detect rooms that are still active, in order to mitigate "zombie" lobbies.
         /// </summary>
        public void DoLobbyHeartbeat(float dt)
        {
            m_HeartbeatTime += dt;
            if (m_HeartbeatTime > k_HeartbeatPeriod)
            {
                m_HeartbeatTime -= k_HeartbeatPeriod;
                try
                {
                    m_LobbyApiInterface.SendHeartbeatPing(CurrentUnityLobby.Id);
                }
                catch (LobbyServiceException e)
                {
                    // If Lobby is not found and if we are not the host, it has already been deleted. No need to publish the error here.
                    if (e.Reason != LobbyExceptionReason.LobbyNotFound && !m_LocalUser.IsHost)
                        PublishError(e);
                }
            }
        }

        void PublishError(LobbyServiceException e)
        {
            var reason = $"{e.Message} ({e.InnerException?.Message})"; // Lobby error type, then HTTP error type.
            m_UnityServiceErrorMessagePub.Publish(new UnityServiceErrorMessage("Lobby Error", reason, UnityServiceErrorMessage.Service.Lobby, e));
        }
    }
}
