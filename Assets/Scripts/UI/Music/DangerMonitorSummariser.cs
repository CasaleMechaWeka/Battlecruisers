using System;
using BattleCruisers.Utils.Properties;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Music
{
    public class DangerMonitorSummariser : IDangerMonitorSummariser
    {
        private readonly IDangerMonitor _dangerMonitor;
        private int _activeDangerEventCount;

        private readonly ISettableBroadcastingProperty<bool> _isInDanger;
        public IBroadcastingProperty<bool> IsInDanger { get; }

        public DangerMonitorSummariser(IDangerMonitor dangerMonitor)
        {
            Assert.IsNotNull(dangerMonitor);

            _dangerMonitor = dangerMonitor;
            _isInDanger = new SettableBroadcastingProperty<bool>(initialValue: false);
            IsInDanger = new BroadcastingProperty<bool>(_isInDanger);

            _dangerMonitor.DangerStart += _dangerMonitor_DangerStart;
            _dangerMonitor.DangerEnd += _dangerMonitor_DangerEnd;

            _activeDangerEventCount = 0;
        }

        private void _dangerMonitor_DangerStart(object sender, EventArgs e)
        {
            _activeDangerEventCount++;

            if (_activeDangerEventCount == 1)
            {
                _isInDanger.Value = true;
            }
        }

        private void _dangerMonitor_DangerEnd(object sender, EventArgs e)
        {
            Assert.IsTrue(_activeDangerEventCount > 0, "Should not get a danger end before the corresponing danger start event :/");
            _activeDangerEventCount--;

            if (_activeDangerEventCount == 0)
            {
                _isInDanger.Value = false;
            }
        }
    }
}