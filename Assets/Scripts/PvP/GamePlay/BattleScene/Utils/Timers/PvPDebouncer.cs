using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Timers
{
    /// <summary>
    /// Performs the debounce action immediately the first time.  Subsequently, performs the action
    /// only if there has been no debounce call in the last specified time.
    /// </summary>
    public class PvPDebouncer : IPvPDebouncer
    {
        private readonly IPvPTimeSinceGameStartProvider _time;
        private readonly float _debounceTimeInS;
        private float _lastChangeTimestamp;

        public PvPDebouncer(IPvPTimeSinceGameStartProvider time, float debounceTimeInS)
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