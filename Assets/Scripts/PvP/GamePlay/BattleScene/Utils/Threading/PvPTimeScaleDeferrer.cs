using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading
{
    public class PvPTimeScaleDeferrer : MonoBehaviour, IPvPDeferrer
    {
        public void Defer(Action action, float delayInS)
        {
            StartCoroutine(Wait(action, delayInS));
        }

        private IEnumerator Wait(Action action, float delayInS)
        {
            yield return new WaitForSeconds(delayInS);
            if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer && NetworkManager.Singleton.ConnectedClientsIds.Count == 2)
                action.Invoke();
        }
    }
}