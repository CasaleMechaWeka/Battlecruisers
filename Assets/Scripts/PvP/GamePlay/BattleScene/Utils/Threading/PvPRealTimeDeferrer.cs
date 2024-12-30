using System;
using System.Collections;
using BattleCruisers.Utils.Threading;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading
{
    public class PvPRealTimeDeferrer : MonoBehaviour, IDeferrer
    {
        public void Defer(Action action, float delayInS)
        {
            StartCoroutine(Wait(action, delayInS));
        }

        private IEnumerator Wait(Action action, float delayInS)
        {
            yield return new WaitForSecondsRealtime(delayInS);
            action.Invoke();
        }
    }
}