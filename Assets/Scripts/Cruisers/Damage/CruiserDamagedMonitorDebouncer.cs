using BattleCruisers.Utils;
using System;
using UnityCommon.PlatformAbstractions;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Damage
{
    public class CruiserDamagedMonitorDebouncer : ICruiserDamageMonitor, IManagedDisposable
    {
        private readonly ICruiserDamageMonitor _monitor;
        private readonly ITime _time;
        private readonly float _debounceTimeInS;
        private float _lastChangeTimestamp;

        private const float DEFAULT_DEBOUNCE_TIME_IN_S = 20;

        public event EventHandler CruiserOrBuildingDamaged;

        public CruiserDamagedMonitorDebouncer(ICruiserDamageMonitor monitor, ITime time, float debounceTimeInS = DEFAULT_DEBOUNCE_TIME_IN_S)
        {
            Helper.AssertIsNotNull(monitor, time);
            Assert.IsTrue(debounceTimeInS > 0);

            _monitor = monitor;
            _time = time;
            _debounceTimeInS = debounceTimeInS;
            _lastChangeTimestamp = float.MinValue;

            _monitor.CruiserOrBuildingDamaged += _monitor_CruiserOrBuildingDamaged;
        }

        private void _monitor_CruiserOrBuildingDamaged(object sender, EventArgs e)
        {
            if ((_time.TimeSinceGameStartInS - _lastChangeTimestamp) >= _debounceTimeInS)
            {
                CruiserOrBuildingDamaged?.Invoke(this, EventArgs.Empty);

                _lastChangeTimestamp = _time.TimeSinceGameStartInS;
            }
        }
        
        public void DisposeManagedState()
        {
            _monitor.CruiserOrBuildingDamaged -= _monitor_CruiserOrBuildingDamaged;
        }
    }
}