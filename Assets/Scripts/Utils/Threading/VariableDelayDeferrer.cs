using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Threading
{
    public class VariableDelayDeferrer : MonoBehaviour
    {
        public void Defer(Action action, float delayInS)
        {
            StartCoroutine(CreateEnumerator(action, delayInS));
        }

        private IEnumerator CreateEnumerator(Action action, float delayInS)
        {
            Assert.IsNotNull(action);

            yield return new WaitForSeconds(delayInS);

            action.Invoke();
        }
    }
}
