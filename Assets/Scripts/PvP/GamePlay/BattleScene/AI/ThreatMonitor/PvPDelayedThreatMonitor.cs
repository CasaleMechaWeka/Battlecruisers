using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors
{
    /// <summary>
    /// Wraps a threat monitor, but only emits a threat level change if
    /// that threat level has not changed for a specified amount of time
    /// (say 5 seconds).
    /// 
    /// This avoids users fooling the AI by committing lots of drones 
    /// to a factory (high threat level), but immediately reassignging
    /// thos drones (so AI wastes resources buidling defences it may
    /// never need).
    /// </summary>
    public class PvPDelayedThreatMonitor : PvPBaseThreatMonitor, IPvPManagedDisposable
    {
        private readonly IPvPThreatMonitor _coreThreatMonitor;
        private readonly IPvPTime _time;
        private readonly IPvPDeferrer _deferrer;
        private readonly float _delayInS;

        private PvPThreatChangeSnapshot _lastThreatChange;

        private const float DEFAULT_DELAY_IN_S = 5;
        private const float MIN_DELAY_IN_S = 0;

        public PvPDelayedThreatMonitor(IPvPThreatMonitor coreThreatMonitor, IPvPTime time, IPvPDeferrer deferrer, float delayInS = DEFAULT_DELAY_IN_S)
        {
            PvPHelper.AssertIsNotNull(coreThreatMonitor, time, deferrer);
            Assert.IsTrue(delayInS >= MIN_DELAY_IN_S);

            _coreThreatMonitor = coreThreatMonitor;
            _time = time;
            _deferrer = deferrer;
            _delayInS = delayInS;
            _lastThreatChange = null;

            _coreThreatMonitor.ThreatLevelChanged += _coreThreatMonitor_ThreatLevelChanged;
        }

        private void _coreThreatMonitor_ThreatLevelChanged(object sender, EventArgs e)
        {
            PvPThreatChangeSnapshot cachedSnapshot = new PvPThreatChangeSnapshot(_coreThreatMonitor.CurrentThreatLevel, _time.TimeSinceGameStartInS);
            _lastThreatChange = cachedSnapshot;

            _deferrer.Defer(() => DelayedThreatEvaluation(cachedSnapshot), _delayInS);
        }

        private void DelayedThreatEvaluation(PvPThreatChangeSnapshot originalThreatSnapshot)
        {
            if (originalThreatSnapshot.Equals(_lastThreatChange))
            {
                // Threat level has not changed since _delayInS :)  Hence
                // safe to emit threat level change.
                CurrentThreatLevel = _lastThreatChange.ThreatLevel;
            }
        }

        public void DisposeManagedState()
        {
            _coreThreatMonitor.ThreatLevelChanged -= _coreThreatMonitor_ThreatLevelChanged;
        }
    }
}