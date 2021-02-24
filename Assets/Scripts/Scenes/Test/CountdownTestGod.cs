using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.Timers;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class CountdownTestGod : MonoBehaviour
    {
        async void Start()
        {
            ILocTable commonStrings = await LocTableFactory.Instance.LoadCommonTable();

            CountdownController countdown = FindObjectOfType<CountdownController>();
            countdown.StaticInitialise(commonStrings);
            countdown.Begin(() => { });
        }
    }
}
