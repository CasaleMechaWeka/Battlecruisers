using System;
using UnityCommon.PlatformAbstractions.Time;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Timers
{
    /// <summary>
    /// Performs the debounce action immediately the first time.  Subsequently, performs the action
    /// only if there has been no debounce call in the last specified time.
    /// </summary>
    public class Debouncer : IDebouncer
    {
        private readonly ITimeSinceGameStartProvider _time;
        private readonly float _debounceTimeInS;
        private float _lastChangeTimestamp;

        public Debouncer(ITimeSinceGameStartProvider time, float debounceTimeInS)
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