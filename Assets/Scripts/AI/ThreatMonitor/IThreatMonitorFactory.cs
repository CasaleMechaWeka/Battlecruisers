using BattleCruisers.Cruisers;

namespace BattleCruisers.AI.ThreatMonitors
{
    public interface IThreatMonitorFactory
    {
		IThreatMonitor CreateAirThreatMonitor();
		IThreatMonitor CreateNavalThreatMonitor();
        IThreatMonitor CreateRocketThreatMonitor();
        IThreatMonitor CreateStealthThreatMonitor();
	}
}
