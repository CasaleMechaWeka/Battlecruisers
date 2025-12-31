using Unity.Netcode;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings
{
    public class PvPBuildingWrapper : PvPBuildableWrapper<IPvPBuilding>
    {
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsClient)
                PvPBattleSceneGodClient.Instance.AddNetworkObject(GetComponent<NetworkObject>());
        }

        public override void OnNetworkDespawn()
        {
            if (IsClient)
                PvPBattleSceneGodClient.Instance.RemoveNetworkObject(GetComponent<NetworkObject>());
            base.OnNetworkDespawn();
        }
    }
}

