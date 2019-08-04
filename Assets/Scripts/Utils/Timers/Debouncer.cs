using System;
using UnityCommon.PlatformAbstractions;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Timers
{
    // FELIX  Use
    // FELIX  Test
    public class Debouncer : IDebouncer
    {
        private readonly ITime _time;
        private readonly float _debounceTimeInS;
        private float _lastChangeTimestamp;

        public Debouncer(ITime time, float debounceTimeInS)
        {
            Assert.IsNotNull(time);
            Assert.IsTrue(debounceTimeInS > 0);

            _time = time;
            _debounceTimeInS = debounceTimeInS;
            _lastChangeTimestamp = float.MinValue;
        }

        public void Debounce(Action action)
        {
            if ((_time.TimeSinceGameStartInS - _lastChangeTimestamp) >= _debounceTimeInS)
            {
                action.Invoke();
                _lastChangeTimestamp = _time.TimeSinceGameStartInS;
            }
        }
    }
}