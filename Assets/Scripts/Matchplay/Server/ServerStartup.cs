using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using Unity.Netcode;


namespace BattleCruisers.Network.Multiplay.Matchplay.Server
{
    public class ServerStartup : MonoBehaviour
    {
        [SerializeField]
        ServerSingleton m_ServerPrefab;

        ApplicationData m_AppData;
        async void Start()
        {
            await LaunchServer();
        }

        async Task LaunchServer()
        {
            m_AppData = new ApplicationData();
            var serverSingletone = Instantiate(m_ServerPrefab);
            await serverSingletone.CreateServer(NetworkManager.Singleton);
            var defaultGameInfo = new GameInfo
            {
                gameMode = GameMode.Starting,
                map = Map.PracticeWreckyards,
                gameQueue = GameQueue.Casual
            };
            // NetworkManager.Singleton.StartServer();
            await serverSingletone.Manager.StartGameServerAsync(defaultGameInfo);

        }

    }
}

