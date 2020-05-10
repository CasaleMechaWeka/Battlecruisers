using UnityEngine;
using System.Collections;

namespace BattleCruisers.Utils.Threading
{
    public class CoroutineStarter : MonoBehaviour, ICoroutineStarter
    {
        public void StartRoutine(IEnumerator routine)
        {
            StartCoroutine(routine);
        }
    }
}