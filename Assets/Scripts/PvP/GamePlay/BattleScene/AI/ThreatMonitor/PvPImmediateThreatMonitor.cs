using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors
{
    /// <summary>
    /// Immediately emits threat level changes, in contrast to DelayedThreatMonitor.
    /// </summary>
    public abstract class PvPImmediateThreatMonitor : PvPBaseThreatMonitor
    {
        protected readonly IPvPCruiserController _enemyCruiser;
        protected readonly IPvPThreatEvaluator _threatEvaluator;

        public PvPImmediateThreatMonitor(IPvPCruiserController enemyCruiser, IPvPThreatEvaluator threatEvaluator)
        {
            Assert.IsNotNull(enemyCruiser);
            Assert.IsNotNull(threatEvaluator);

            _enemyCruiser = enemyCruiser;
            _threatEvaluator = threatEvaluator;
        }

        protected void EvaluateThreatLevel()
        {
            CurrentThreatLevel = _threatEvaluator.FindThreatLevel(FindThreatEvaluationParameter());
        }

        protected abstract float FindThreatEvaluationParameter();
    }
}
