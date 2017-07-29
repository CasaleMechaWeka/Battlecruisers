using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Threading
{
    public class Deferrer : MonoBehaviour, IDeferrer
	{
		public void DeferToFrameEnd(Action action)
		{
            StartCoroutine(CreateEnumerator(action));
		}

        private IEnumerator CreateEnumerator(Action action)
		{
			Assert.IsNotNull(action);

			yield return new WaitForEndOfFrame();

			action.Invoke();
		}
	}
}
