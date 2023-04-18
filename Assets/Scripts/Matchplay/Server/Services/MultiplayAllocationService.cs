using System;
using Unity.Services.Matchmaker.Models;
using Unity.Services.Multiplay;
using Debug = UnityEngine.Debug;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.Server
{

    public class MultiplayAllocationService : IDisposable
    {

        IMultiplayService m_MultiplayService;
        MultiplayEventCallbacks m_Servercallbacks;
        IServerEvents m_ServerEvents;
        string m_AllocationId;

        public MultiplayAllocationService()
        {
            try
            {
                m_MultiplayService = MultiplayService.Instance;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Error creating Multiplay allocation service.\n{ex}");
            }
        }


        public async Task<MatchmakingResults> SubscribeAndWaitMatchmakerAllocation()
        {
            if (m_MultiplayService == null)
            {
                return null;
            }

            m_AllocationId = null;
            m_Servercallbacks = new MultiplayEventCallbacks();
            m_Servercallbacks.Allocate += OnMultiplayAllocation;
            m_ServerEvents = await m_MultiplayService.SubscribeToServerEventsAsync(m_Servercallbacks);
            var allocationID = await AwaitAllocationID();
            var mmPayload = await GetMatchmakerAllocationPayloadAsync();
            return mmPayload;
        }


        async Task<string> AwaitAllocationID()
        {
            var config = m_MultiplayService.ServerConfig;
            Debug.Log($"Awaiting Allocation. Server Config is:\n" +
            $"-ServerID: {config.ServerId}\n" +
            $"-AllocationID: {config.AllocationId}\n" +
            $"-Port: {config.Port}\n" +
            $"-QPort: {config.QueryPort}\n" +
            $"-logs: {config.ServerLogDirectory}");
            while (string.IsNullOrEmpty(m_AllocationId))
            {
                var configID = config.AllocationId;
                if (!string.IsNullOrEmpty(configID) && string.IsNullOrEmpty(m_AllocationId))
                {
                    Debug.Log($"Config had AllocaionID: {configID}");
                    m_AllocationId = configID;
                }
                await Task.Delay(100);
            }
            return m_AllocationId;
        }

        async Task<MatchmakingResults> GetMatchmakerAllocationPayloadAsync()
        {
            var payloadAllocation = await MultiplayService.Instance.GetPayloadAllocationFromJsonAs<MatchmakingResults>();
            var modelAsJson = JsonConvert.SerializeObject(payloadAllocation, Formatting.Indented);
            Debug.Log(nameof(GetMatchmakerAllocationPayloadAsync) + ":" + Environment.NewLine + modelAsJson);
            return payloadAllocation;
        }


        void OnMultiplayAllocation(MultiplayAllocation allocation)
        {
            Debug.Log($"OnAllocation: {allocation.AllocationId}");
            if (string.IsNullOrEmpty(allocation.AllocationId))
                return;
            m_AllocationId = allocation.AllocationId;
        }

        void OnMultiplayDeAllocation(MultiplayDeallocation deallocation)
        {
            Debug.Log($"Multiplay Deallocated : ID : {deallocation.AllocationId} \n Event: {deallocation.EventId}\nServer{deallocation.ServerId}");
        }
        void OnMultiplayError(MultiplayError error)
        {
            Debug.Log($"MultiplayError : {error.Reason} \n {error.Detail}");
        }

        public void Dispose()
        {
            if (m_Servercallbacks != null)
            {
                m_Servercallbacks.Allocate -= OnMultiplayAllocation;
                m_Servercallbacks.Deallocate -= OnMultiplayDeAllocation;
                m_Servercallbacks.Error -= OnMultiplayError;
            }
            m_ServerEvents?.UnsubscribeAsync();
        }
    }
}

