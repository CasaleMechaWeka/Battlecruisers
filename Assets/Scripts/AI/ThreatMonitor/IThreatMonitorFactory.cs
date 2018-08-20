namespace BattleCruisers.AI.ThreatMonitors
{
    public interface IThreatMonitorFactory
    {
		IThreatMonitor CreateAirThreatMonitor();
		IThreatMonitor CreateNavalThreatMonitor();

        IThreatMonitor CreateRocketThreatMonitor();
        IThreatMonitor CreateStealthThreatMonitor();

        IThreatMonitor CreateDelayedThreatMonitor(IThreatMonitor coreMonitor);
	}
}
