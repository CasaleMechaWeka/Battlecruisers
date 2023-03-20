using BattleCruisers.Network.Multiplay.ConnectionManagement;
using BattleCruisers.Network.Multiplay.Gameplay.Configuration;
using BattleCruisers.Network.Multiplay.Gameplay.GameState;
using BattleCruisers.Network.Multiplay.Infrastructure;
using ParrelSync;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using VContainer;


namespace BattleCruisers.Network.Multiplay.Scenes
{
    public class MatchmakingScreensSceneGod : GameStateBehaviour
    {
        public const string k_DefaultIP = "127.0.0.1";
        public const int k_DefaultPort = 9998;

        public override GameState ActiveState { get { return GameState.MatchmakingScreenScene; } }


        [SerializeField]
        string m_IP;
        [SerializeField]
        string m_Port;
        [SerializeField]
        NameGenerationData m_NameGenerationData;


        /// <summary>
        ////////////////////////////////////////////  Local Test  ///////////////////////////////////////////
        /// </summary>
        public bool isLocalTest = true;
        private string m_localTestName;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        

        [Inject] ConnectionManager m_ConnectionManager;

        ISubscriber<ConnectStatus> m_ConnectStatusSubscriber;


        [Inject]
        void InjectDependencies(ISubscriber<ConnectStatus> connectStatusSubscriber)
        {
            m_ConnectStatusSubscriber = connectStatusSubscriber;
            m_ConnectStatusSubscriber.Subscribe(OnConnectStatusMessage);
        }


        void OnConnectStatusMessage(ConnectStatus connectStatus)
        {
            
        }

        protected override void Awake()
        {
            base.Awake();
        }
        // Start is called before the first frame update
        void Start()
        {
            m_localTestName = m_NameGenerationData.GenerateName();
            StartCoroutine(iStartPvP());
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
        }

        private void HostIPRequest(string ip, string port)
        {
           
            int.TryParse(port, out var portNum);
            if (portNum <= 0)
            {
                portNum = k_DefaultPort;
            }
            ip = string.IsNullOrEmpty(ip) ? k_DefaultIP : ip;
            m_ConnectionManager.StartHostIp(m_localTestName, ip, portNum);

        }


        private void JoinWithIP(string ip, string port)
        {
            int.TryParse(port, out var portNum);
            if (portNum <= 0)
            {
                portNum = k_DefaultPort;
            }
            ip = string.IsNullOrEmpty(ip) ? k_DefaultIP : ip;
            m_ConnectionManager.StartClientIp(m_localTestName, ip, portNum);
        }

        IEnumerator iStartPvP()
        {

            yield return new WaitForEndOfFrame();

            if (isLocalTest)
            {
                // Local Test
                if (!ClonesManager.IsClone())
                {
                    HostIPRequest(m_IP, m_Port);
                }
                else
                {
                    JoinWithIP(m_IP, m_Port);
                }
            }
            else
            {
                // Lobby Service


            }
        }

    }
}

