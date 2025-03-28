namespace BattleCruisers.AI.ThreatMonitors
{
    public interface IThreatMonitorFactory
    {
        BaseThreatMonitor CreateAirThreatMonitor();
        BaseThreatMonitor CreateNavalThreatMonitor();

        BaseThreatMonitor CreateRocketThreatMonitor();
        BaseThreatMonitor CreateStealthThreatMonitor();

        BaseThreatMonitor CreateDelayedThreatMonitor(BaseThreatMonitor coreMonitor);
    }
}
