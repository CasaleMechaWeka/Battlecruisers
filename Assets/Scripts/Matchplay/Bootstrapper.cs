using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
#if UNITY_SERVER
using BattleCruisers.Network.Multiplay.Matchplay.Server;
#endif
using Unity.Netcode;


namespace BattleCruisers.Network.Multiplay.Matchplay.Shared
{
    public class Bootstrapper : MonoBehaviour
    {

#if UNITY_SERVER
        [SerializeField]
        ServerSingleton m_ServerPrefab;
        ApplicationData m_AppData;
#endif

        [SerializeField]
        NetworkManager m_NetworkManagerPrefab;
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
#if UNITY_SERVER
#pragma warning disable 4014
                    LaunchServer();
#pragma warning restore 4014
#endif
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

#if UNITY_SERVER
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
#endif
    }

}
