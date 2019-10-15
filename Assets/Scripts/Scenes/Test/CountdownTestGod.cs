using BattleCruisers.Utils.Timers;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class CountdownTestGod : MonoBehaviour
    {
        void Start()
        {
            CountdownController countdown = FindObjectOfType<CountdownController>();
            countdown.StaticInitialise();
            countdown.Begin(() => { });
        }
    }
}
