using BattleCruisers.Buildables.Buildings.Factories;

namespace BattleCruisers.AI.Drones.BuildingMonitors
{
    public interface IFactoryMonitorFactory
    {
        IFactoryMonitor CreateMonitor(IFactory factory);
    }
}
