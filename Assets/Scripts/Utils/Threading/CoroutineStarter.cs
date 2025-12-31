using UnityEngine;
using System.Collections;

namespace BattleCruisers.Utils.Threading
{
    public class CoroutineStarter : MonoBehaviour
    {
        public void StartRoutine(IEnumerator routine)
        {
            StartCoroutine(routine);
        }
    }
}