using BattleCruisers.Buildables.Buildings.Factories;

namespace BattleCruisers.AI.Drones
{
    public interface IFactoryMonitorFactory
    {
        IFactoryMonitor CreateMonitor(IFactory factory);
    }
}
