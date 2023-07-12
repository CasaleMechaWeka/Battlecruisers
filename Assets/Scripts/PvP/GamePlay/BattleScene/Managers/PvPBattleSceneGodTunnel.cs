using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    public class PvPBattleSceneGodTunnel : NetworkBehaviour
    {
        public IPvPBattleCompletionHandler battleCompletionHandler;

        // events
        public NetworkVariable<Tunnel_BattleCompletedState> BattleCompleted = new NetworkVariable<Tunnel_BattleCompletedState>(writePerm: NetworkVariableWritePermission.Owner); // _battleCompletionHandler.BattleCompleted?.



        public override void OnNetworkSpawn()
        {
            BattleCompleted.Value = Tunnel_BattleCompletedState.None;
        }

        public void CompleteBattle(bool wasVictory, bool retryLevel)
        {
            if (IsServer)
            {
                CompleteBattleClientRpc(wasVictory, retryLevel);
            }
        }

        public void CompleteBattle(bool wasPlayerVictory, bool retryLevel,long destructionScore)
        {
            if(IsServer)
            {
                CompleteBattleClientRpc(wasPlayerVictory, retryLevel, destructionScore);
            }
        }

        [ClientRpc]
        private void CompleteBattleClientRpc(bool wasVictory, bool retryLevel)
        {
            battleCompletionHandler?.CompleteBattle(wasVictory, retryLevel);
        }

        [ClientRpc]
        private void CompleteBattleClientRpc(bool wasVictory, bool retryLevel, long destructionScore)
        {
            battleCompletionHandler?.CompleteBattle(wasVictory, retryLevel, destructionScore);
        }
    }

    public enum Tunnel_BattleCompletedState
    {
        None = 0,
        Completed = 1
    }
}
