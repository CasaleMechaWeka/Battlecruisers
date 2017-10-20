using BattleCruisers.Utils.Timers;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class TimerTestGod : MonoBehaviour
    {
        void Start()
        {
            CountdownController countdown = FindObjectOfType<CountdownController>();
            countdown.Initialise();
            countdown.Begin(() => { });
        }
    }
}
