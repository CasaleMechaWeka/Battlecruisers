using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones.BuildingMonitors
{
    public interface IPvPFactoryMonitorFactory
    {
        IPvPFactoryMonitor CreateMonitor(IPvPFactory factory);
    }
}
