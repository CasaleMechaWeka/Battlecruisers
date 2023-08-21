using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.HeckleMessage;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    public class PvPHeckleMessageManager : NetworkBehaviour
    {
        public PvPHeckleMessage leftMessage, rightMessage;
        public static PvPHeckleMessageManager Instance;



        public void Initialise(IDataProvider dataProvider, IPvPSingleSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(dataProvider, soundPlayer, leftMessage, rightMessage);

            leftMessage.Initialise(dataProvider, soundPlayer);
            rightMessage.Initialise(dataProvider, soundPlayer);

            leftMessage.Hide();
            rightMessage.Hide();
        }
        public void SendHeckle(int heckleIndex)
        {
            if(IsClient)
            {
                SendHeckleServerRpc(heckleIndex);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void SendHeckleServerRpc(int heckleIndex, ServerRpcParams serverRpcParams = default)
        {
            var senderID = serverRpcParams.Receive.SenderClientId;

            DisplayHeckleMessageClientRpc(heckleIndex, senderID);
        }


        [ClientRpc]
        private void DisplayHeckleMessageClientRpc(int heckleIndex, ulong sender)
        {
            if(sender == NetworkManager.Singleton.LocalClientId)
            {
                if (SynchedServerData.Instance.GetTeam() == Team.LEFT)
                    leftMessage.Show(heckleIndex);
                else
                    rightMessage.Show(heckleIndex);
            }
            else
            {
                if (SynchedServerData.Instance.GetTeam() == Team.LEFT)
                    rightMessage.Show(heckleIndex);
                else
                    leftMessage.Show(heckleIndex);
            }
        }


        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if(Instance == null)
                Instance = this;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
        }
    }
}
