using System;
using System.Collections;
using UnityEngine;

namespace BattleCruisers.Utils.Threading
{
    public class RealTimeDeferrer : MonoBehaviour, IDeferrer
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