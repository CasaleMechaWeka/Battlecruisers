using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace BattleCruisers.Network.Multiplay.Matchplay.Client
{
    public class Matchplayer : NetworkBehaviour
    {
        [HideInInspector]
        public NetworkVariable<NetworkString> PlayerName = new NetworkVariable<NetworkString>();
        public override void OnNetworkSpawn()
        {

        }

        public override void OnNetworkDespawn()
        {

        }
    }
}


