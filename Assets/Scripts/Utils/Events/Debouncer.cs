using System;
using UnityCommon.PlatformAbstractions;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Events
{
    // FELIX  Delete :D
    public class Debouncer<TEventArgs> : IManagedDisposable where TEventArgs : EventArgs
    {
        private readonly IDebouncable<TEventArgs> _debouncable;
        private readonly ITime _time;
        private readonly float _debounceTimeInS;
        private float _lastChangeTimestamp;

        private const float DEFAULT_DEBOUNCE_TIME_IN_S = 20;

        public Debouncer(IDebouncable<TEventArgs> debouncable, ITime time, float debounceTimeInS = DEFAULT_DEBOUNCE_TIME_IN_S)
        {
            Helper.AssertIsNotNull(debouncable, time);
            Assert.IsTrue(debounceTimeInS > 0);

            _debouncable = debouncable;
            _time = time;
            _debounceTimeInS = debounceTimeInS;
            _lastChangeTimestamp = float.MinValue;

            _debouncable.UndebouncedEvent += _debouncable_UndebouncedEvent;
        }

        private void _debouncable_UndebouncedEvent(object sender, TEventArgs e)
        {
            if ((_time.TimeSinceGameStartInS - _lastChangeTimestamp) >= _debounceTimeInS)
            {
                _debouncable.EmitDebouncedEvent(e);

                _lastChangeTimestamp = _time.TimeSinceGameStartInS;
            }
        }

        public void DisposeManagedState()
        {
            _debouncable.UndebouncedEvent -= _debouncable_UndebouncedEvent;
        }
    }
}