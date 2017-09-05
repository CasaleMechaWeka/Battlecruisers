using BattleCruisers.Cruisers;

namespace BattleCruisers.AI.ThreatMonitors
{
    public interface IThreatMonitorFactory
    {
		IThreatMonitor CreateAirThreatMonitor(ICruiserController playerCruiser);
		IThreatMonitor CreateNavalThreatMonitor(ICruiserController playerCruiser);
        IThreatMonitor CreateRocketThreatMonitor(ICruiserController playerCruiser);
        IThreatMonitor CreateStealthThreatMonitor(ICruiserController playerCruiser);
	}
}
