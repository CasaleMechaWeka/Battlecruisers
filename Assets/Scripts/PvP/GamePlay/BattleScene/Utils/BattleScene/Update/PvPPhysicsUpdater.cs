using System;
using Unity.Netcode;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update
{
    public class PvPPhysicsUpdater : MonoBehaviour, IPvPUpdater
    {
        public float DeltaTime => Time.deltaTime;

        public event EventHandler Updated;

        void FixedUpdate()
        {
            if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer && NetworkManager.Singleton.ConnectedClientsIds.Count == 2)
                Updated?.Invoke(this, EventArgs.Empty);
        }
    }
}