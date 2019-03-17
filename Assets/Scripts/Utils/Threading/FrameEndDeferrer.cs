using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Threading
{
    // FELIX  Remove :)
    public class FrameEndDeferrer : MonoBehaviour, IDeferrer
	{
        public void Defer(Action action)
		{
            StartCoroutine(CreateEnumeratorForFrameEnd(action));
		}

        private IEnumerator CreateEnumeratorForFrameEnd(Action action)
		{
			Assert.IsNotNull(action);

			yield return new WaitForEndOfFrame();

			action.Invoke();
		}
	}
}
