using UnityEngine;
using Unity.Netcode;
using Unity.Multiplayer.Samples.Utilities;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene
{
    [RequireComponent(typeof(NetcodeHooks))]
    public class DestroyOnPlatform : MonoBehaviour
    {
        [SerializeField]
        NetcodeHooks m_NetcodeHooks;
        public enum TargetPlatform
        {
            Server,
            Client
        }

        public TargetPlatform targetPlatform;


        private void Awake()
        {
            m_NetcodeHooks.OnNetworkSpawnHook += OnNetworkSpawn;
            m_NetcodeHooks.OnNetworkDespawnHook += OnNetworkDespawn;
        }
        public void OnNetworkSpawn()
        {

            if (NetworkManager.Singleton.IsServer && targetPlatform != TargetPlatform.Server)
            {
                Destroy(gameObject);
            }

            if (NetworkManager.Singleton.IsClient && targetPlatform != TargetPlatform.Client)
            {
                Destroy(gameObject);
            }
        }

        public void OnNetworkDespawn()
        {


        }


    }

}
