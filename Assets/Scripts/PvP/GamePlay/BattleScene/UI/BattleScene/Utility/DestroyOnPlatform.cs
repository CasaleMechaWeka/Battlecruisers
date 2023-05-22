using Unity.Netcode;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene
{
    public class DestroyOnPlatform : NetworkBehaviour
    {
        public enum TargetPlatform
        {
            Server,
            Client
        }

        public TargetPlatform platform;
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer && platform == TargetPlatform.Server)
            {
                Destroy(gameObject);
            }

            if (IsClient && platform == TargetPlatform.Client)
            {
                Destroy(gameObject);
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

        }


    }

}
