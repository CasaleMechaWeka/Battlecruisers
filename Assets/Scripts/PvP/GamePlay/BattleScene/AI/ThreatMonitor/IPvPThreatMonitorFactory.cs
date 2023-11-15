namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors
{
    public interface IPvPThreatMonitorFactory
    {
        IPvPThreatMonitor CreateAirThreatMonitor();
        IPvPThreatMonitor CreateNavalThreatMonitor();

        IPvPThreatMonitor CreateRocketThreatMonitor();
        IPvPThreatMonitor CreateStealthThreatMonitor();

        IPvPThreatMonitor CreateDelayedThreatMonitor(IPvPThreatMonitor coreMonitor);
    }
}
