using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    public class PvPPlayerManager : NetworkBehaviour
    {
        IApplicationModel applicationModel;
        NetworkVariable<NetworkString> prefabPathOfCruiser = new NetworkVariable<NetworkString>(readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Owner);

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsClient)
            {
                applicationModel = ApplicationModelProvider.ApplicationModel;
                string[] sList = applicationModel.DataProvider.GameModel.PlayerLoadout.Hull.PrefabPath.Split("/");
                string prefabPath = sList[sList.Length - 1];
                prefabPathOfCruiser.Value = (NetworkString)$"cruisername={prefabPath}";
            }

        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
        }

        void Update()
        {
            if (IsServer)
            {
                if (prefabPathOfCruiser.Value.ToString().Split("=").Length == 2)
                {
                    Debug.Log("Cruiser Name is " + prefabPathOfCruiser.Value.ToString().Split("=")[1]);
                }
            }

        }
    }
}

