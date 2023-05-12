using UnityEngine;
using System.Collections;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading
{
    public class PvPCoroutineStarter : MonoBehaviour, IPvPCoroutineStarter
    {
        public void StartRoutine(IEnumerator routine)
        {
            StartCoroutine(routine);
        }
    }
}