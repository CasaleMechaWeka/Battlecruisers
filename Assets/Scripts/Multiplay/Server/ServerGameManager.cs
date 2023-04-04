using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;


namespace BattleCruisers.Network.Multiplay.Matchplay.Server
{
    public class ServerGameManager : IDisposable
    {

        public bool StartedServices => m_StartedServices;
        bool m_StartedServices;


        public ServerGameManager(string serverIP, int serverPort, int serverQPort, NetworkManager manager)
        {

        }
        public void Dispose()
        {

        }

        public async Task StartGameServerAsync(GameInfo startingGameInfo)
        {
            Debug.Log($"starting server with: {startingGameInfo}");

        }
    }
}
