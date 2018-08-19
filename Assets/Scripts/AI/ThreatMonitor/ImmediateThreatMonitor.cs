using BattleCruisers.Cruisers;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.ThreatMonitors
{
    /// <summary>
    /// Immediately emits threat level changes, in contrast to DelayedThreatMonitor.
    /// </summary>
    public abstract class ImmediateThreatMonitor : BaseThreatMonitor
	{
		protected readonly ICruiserController _enemyCruiser;
		protected readonly IThreatEvaluator _threatEvaluator;
        
        public ImmediateThreatMonitor(ICruiserController enemyCruiser, IThreatEvaluator threatEvaluator)
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
