using BattleCruisers.Utils;
using BattleCruisers.Utils.Events;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Damage
{
    // FELIX  Test
    // FELIX  Use
    public class CruiserDamagedMonitorDebouncable : ICruiserDamageMonitor, IDebouncable<EventArgs>, IManagedDisposable
    {
        private readonly ICruiserDamageMonitor _monitor;

        public event EventHandler CruiserOrBuildingDamaged;
        public event EventHandler<EventArgs> UndebouncedEvent;

        public CruiserDamagedMonitorDebouncable(ICruiserDamageMonitor monitor)
        {
            Assert.IsNotNull(monitor);

            _monitor = monitor;
            _monitor.CruiserOrBuildingDamaged += _monitor_CruiserOrBuildingDamaged;
        }

        private void _monitor_CruiserOrBuildingDamaged(object sender, EventArgs e)
        {
            UndebouncedEvent?.Invoke(this, e);
        }

        public void EmitDebouncedEvent(EventArgs eventArgs)
        {
            CruiserOrBuildingDamaged?.Invoke(this, eventArgs);
        }

        public void DisposeManagedState()
        {
            _monitor.CruiserOrBuildingDamaged -= _monitor_CruiserOrBuildingDamaged;
        }
    }
}