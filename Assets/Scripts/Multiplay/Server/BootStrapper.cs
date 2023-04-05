using System.Threading.Tasks;
using UnityEngine;
using ParrelSync;
using UnityEngine.SceneManagement;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using Unity.Netcode;


namespace BattleCruisers.Network.Multiplay.Matchplay.Server
{
    public class BootStrapper : MonoBehaviour
    {

        public static bool IsServer;
        [SerializeField]
        ServerSingleton m_ServerPrefab;
        [SerializeField]
        NetworkManager m_NetworkManager;



        // Start is called before the first frame update
        async void Start()
        {
            if (Application.isEditor)
            {
#if UNITY_EDITOR
                if (ClonesManager.IsClone())
                {
                    var argument = ClonesManager.GetArgument();

                    if (argument == "server")
                    {

                        await LaunchInMode(true, "server");

                    }
                    else if (argument == "client")
                    {

                        await LaunchInMode(false, "client");

                    }
                }
                else
                {
                    await LaunchInMode(false, "client");
                }
#endif
            }
            else
            {
                await LaunchInMode(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null);
            }
        }


        async Task LaunchInMode(bool isServer, string profileName = "default")
        {
            if (isServer)
            {
                var serverSingletone = Instantiate(m_ServerPrefab);
                var networkManager = Instantiate(m_NetworkManager);
                await serverSingletone.CreateServer(networkManager);
                var defaultGameInfo = new GameInfo
                {
                    gameMode = GameMode.Starting,
                    map = Arena.PracticeWreckyards,
                    gameQueue = GameQueue.Casual
                };
                await serverSingletone.Manager.StartGameServerAsync(defaultGameInfo);
            }
            else
            {
                SceneManager.LoadScene("LandingScene", LoadSceneMode.Single);
            }
        }
        // Update is called once per frame
        void Update()
        {

        }
    }

}
