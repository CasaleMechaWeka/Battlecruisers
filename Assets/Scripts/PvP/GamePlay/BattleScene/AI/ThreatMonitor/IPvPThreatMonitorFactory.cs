using BattleCruisers.AI.ThreatMonitors;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors
{
    public interface IPvPThreatMonitorFactory
    {
        IThreatMonitor CreateAirThreatMonitor();
        IThreatMonitor CreateNavalThreatMonitor();

        IThreatMonitor CreateRocketThreatMonitor();
        IThreatMonitor CreateStealthThreatMonitor();

        IThreatMonitor CreateDelayedThreatMonitor(IThreatMonitor coreMonitor);
    }
}
