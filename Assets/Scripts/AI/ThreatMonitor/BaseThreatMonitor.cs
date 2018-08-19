using System;

namespace BattleCruisers.AI.ThreatMonitors
{
    // FELIX  Test :)
    public abstract class BaseThreatMonitor : IThreatMonitor
	{
		private ThreatLevel _currentThreatLevel;
		public ThreatLevel CurrentThreatLevel
		{
			get { return _currentThreatLevel; }
			protected set
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

        public BaseThreatMonitor()
        {
            CurrentThreatLevel = ThreatLevel.None;
        }
    }
}
