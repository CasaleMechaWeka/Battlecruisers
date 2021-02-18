using BattleCruisers.Utils.Threading;
using System;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Timers
{
    /// <summary>
    /// Performs the debounce action after the specified time after the last debounce call.
    /// </summary>
    public class DeferredDebouncer : IDebouncer
    {
        private readonly ITimeSinceGameStartProvider _time;
        private readonly IDeferrer _deferrer;
        private readonly float _debounceTimeInS;
        private float _lastDebounceTimestamp;
        private Action _action;

        public DeferredDebouncer(ITimeSinceGameStartProvider time, IDeferrer deferrer, float debounceTimeInS)
        {
            Helper.AssertIsNotNull(time, deferrer);
            Assert.IsTrue(debounceTimeInS > 0);

            _time = time;
            _deferrer = deferrer;
            _debounceTimeInS = debounceTimeInS;
        }

        public void Debounce(Action action)
        {
            _lastDebounceTimestamp = _time.TimeSinceGameStartInS;
            _action = action;
            _deferrer.Defer(ExecuteActionIfDebounced, _debounceTimeInS);
        }

        private void ExecuteActionIfDebounced()
        {
            if ((_time.TimeSinceGameStartInS - _lastDebounceTimestamp) >= _debounceTimeInS)
            {
                _action.Invoke();
            }
        }
    }
}