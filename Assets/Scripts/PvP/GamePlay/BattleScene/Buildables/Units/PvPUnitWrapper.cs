using Unity.Netcode;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units
{
    public class PvPUnitWrapper : PvPBuildableWrapper<IPvPUnit>
    {

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsClient)
                PvPBattleSceneGodClient.Instance.AddNetworkObject(GetComponent<NetworkObject>());
        }

        public override void OnNetworkDespawn()
        {
            if(IsClient)
                PvPBattleSceneGodClient.Instance.RemoveNetworkObject(GetComponent<NetworkObject>());
            base.OnNetworkDespawn();
        }
    }
}
