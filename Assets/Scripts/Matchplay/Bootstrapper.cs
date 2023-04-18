using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using BattleCruisers.Network.Multiplay.Matchplay.Server;
using Unity.Netcode;


namespace BattleCruisers.Network.Multiplay.Matchplay.Shared
{
    public class Bootstrapper : MonoBehaviour
    {

        [SerializeField]
        ServerSingleton m_ServerPrefab;
        [SerializeField]
        NetworkManager m_NetworkManagerPrefab;

        ApplicationData m_AppData;
        void Start()
        {
            if (Application.isEditor)
                return;

            LaunchInMode(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null);
        }


        void LaunchInMode(bool isServer)
        {
            if (isServer)
            {
                // SceneManager.LoadScene("MultiplayServerStartup");
#pragma warning disable 4014
                LaunchServer();
#pragma warning restore 4014
            }
            else
            {
                SceneManager.LoadScene("LandingScene");
            }
        }

        public void OnParellSyncStarted(bool isServer)
        {
            LaunchInMode(isServer);
        }


        async Task LaunchServer()
        {
            // Debug.Log("You called server");
            m_AppData = new ApplicationData();
            var serverSingletone = Instantiate(m_ServerPrefab);
            var networkManger = Instantiate(m_NetworkManagerPrefab);
            await serverSingletone.CreateServer(networkManger);
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
