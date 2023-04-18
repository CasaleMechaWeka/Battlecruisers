#if UNITY_SERVER
using System.Threading.Tasks;
using UnityEngine;
using Unity.Netcode;


namespace BattleCruisers.Network.Multiplay.Matchplay.Server
{
    public class ServerSingleton : MonoBehaviour
    {
        public static ServerSingleton Instance
        {
            get
            {
                if (s_ServerSingleton != null)
                {
                    s_ServerSingleton = FindObjectOfType<ServerSingleton>();
                }
                if (s_ServerSingleton == null)
                {
                    Debug.LogError("No ServerSingleton in scene, did you run this from the bootstrap scene?");
                    return null;
                }
                return s_ServerSingleton;
            }
        }



        static ServerSingleton s_ServerSingleton;


        public ServerGameManager Manager
        {
            get
            {
                if (m_GameManager != null)
                {
                    return m_GameManager;
                }
                Debug.LogError($"ServerManager is Missing, did you run OpenConnection?");
                return null;
            }
        }


        ServerGameManager m_GameManager;

        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        void OnDestroy()
        {
            m_GameManager?.Dispose();
        }

        public async Task CreateServer(NetworkManager networkManager)
        {
            await Unity.Services.Core.UnityServices.InitializeAsync();
            m_GameManager = new ServerGameManager(
                ApplicationData.IP(),
                ApplicationData.Port(),
                ApplicationData.QPort(),
                networkManager
            );
        }
    }
}
#endif
