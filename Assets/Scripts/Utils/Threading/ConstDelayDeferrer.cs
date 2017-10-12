using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Threading
{
	public class ConstDelayDeferrer : MonoBehaviour, IDeferrer
	{
        private float _delayInS;

        public void StaticInitialise(float delayInMs)
        {
			Assert.IsTrue(delayInMs > 0);
            _delayInS = delayInMs / Constants.MS_PER_S;
        }

        public void Defer(Action action)
        {
            StartCoroutine(CreateEnumerator(action));
        }

        private IEnumerator CreateEnumerator(Action action)
        {
            Assert.IsNotNull(action);

            yield return new WaitForSeconds(_delayInS);

			action.Invoke();
		}
	}
}
