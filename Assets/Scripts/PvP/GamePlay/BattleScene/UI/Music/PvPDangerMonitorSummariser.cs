using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Music
{
    public class PvPDangerMonitorSummariser : IPvPDangerMonitorSummariser
    {
        private readonly IPvPDangerMonitor _dangerMonitor;
        private int _activeDangerEventCount;

        private readonly IPvPSettableBroadcastingProperty<bool> _isInDanger;
        public IPvPBroadcastingProperty<bool> IsInDanger { get; }

        public PvPDangerMonitorSummariser(IPvPDangerMonitor dangerMonitor)
        {
            Assert.IsNotNull(dangerMonitor);

            _dangerMonitor = dangerMonitor;
            _isInDanger = new PvPSettableBroadcastingProperty<bool>(initialValue: false);
            IsInDanger = new PvPBroadcastingProperty<bool>(_isInDanger);

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