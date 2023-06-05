using UnityEngine;
using Unity.Netcode;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene
{

    public class DestroyOnPlatform : MonoBehaviour
    {

        public enum TargetPlatform
        {
            Server,
            Client
        }

        public TargetPlatform targetPlatform;


        private void Awake()
        {

        }



        void Start()
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


    }

}
