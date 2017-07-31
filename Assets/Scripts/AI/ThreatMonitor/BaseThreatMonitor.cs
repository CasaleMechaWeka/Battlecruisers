using System;
using BattleCruisers.Cruisers;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.ThreatMonitors
{
    public abstract class BaseThreatMonitor : IThreatMonitor
	{
		protected readonly ICruiserController _enemyCruiser;
		protected readonly IThreatEvaluator _threatEvaluator;

		private ThreatLevel _currentThreatLevel;
		public ThreatLevel CurrentThreatLevel
		{
			get { return _currentThreatLevel; }
			private set
			{
				if (_currentThreatLevel != value)
				{
					_currentThreatLevel = value;

					if (ThreatLevelChanged != null)
					{
						ThreatLevelChanged.Invoke(this, EventArgs.Empty);
					}
				}
			}
		}
		
		public event EventHandler ThreatLevelChanged;

        public BaseThreatMonitor(ICruiserController enemyCruiser, IThreatEvaluator threatEvaluator)
        {
			Assert.IsNotNull(enemyCruiser);
			Assert.IsNotNull(threatEvaluator);

			_enemyCruiser = enemyCruiser;
			_threatEvaluator = threatEvaluator;

            CurrentThreatLevel = ThreatLevel.None;
        }

		protected void EvaluateThreatLevel()
		{
            CurrentThreatLevel = _threatEvaluator.FindThreatLevel(FindThreatEvaluationParameter());
		}

        protected abstract float FindThreatEvaluationParameter();
	}
}
