using System;

namespace BattleCruisers.AI.ThreatMonitors
{
	public enum ThreatLevel
	{
		None, Low, High
	}

	public abstract class BaseThreatMonitor
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

					ThreatLevelChanged?.Invoke(this, EventArgs.Empty);
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
