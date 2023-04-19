#if UNITY_SERVER || UNITY_EDITOR
using UnityEngine;
using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.Matchplay.Server;
using Unity.Netcode;


namespace BattleCruisers.Network.Multiplay.Matchplay.Shared
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField]
        ServerSingleton m_ServerPrefab;
        ApplicationData m_AppData;
        [SerializeField]
        NetworkManager m_NetworkManagerPrefab;
        void Start()
        {
            LaunchInMode();
        }


        void LaunchInMode()
        {
#pragma warning disable 4014
            LaunchServer();
#pragma warning restore 4014

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
#endif