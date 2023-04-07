using System;
using UnityEngine;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.Networking;

namespace BattleCruisers.Network.Multiplay.Matchplay.Shared
{
    public class SynchedServerData : NetworkBehaviour
    {
        [HideInInspector]
        public NetworkVariable<NetworkString> serverID = new NetworkVariable<NetworkString>();
        public NetworkVariable<Map> map = new NetworkVariable<Map>();
        public NetworkVariable<GameMode> gameMode = new NetworkVariable<GameMode>();
        public NetworkVariable<GameQueue> gameQueue = new NetworkVariable<GameQueue>();

        public Action OnNetworkSpawned;

        public override void OnNetworkSpawn()
        {
            OnNetworkSpawned?.Invoke();
        }
    }

}
