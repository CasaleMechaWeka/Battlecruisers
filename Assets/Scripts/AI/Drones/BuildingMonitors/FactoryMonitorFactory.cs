using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.Drones.BuildingMonitors
{
    public class FactoryMonitorFactory : IFactoryMonitorFactory
    {
        public IFactoryMonitor CreateMonitor(IFactory factory)
        {
            int numOfUnitsToBuild = RandomGenerator.Range(2, 4);
            return new FactoryMonitor(factory, numOfUnitsToBuild);
        }
    }
}
