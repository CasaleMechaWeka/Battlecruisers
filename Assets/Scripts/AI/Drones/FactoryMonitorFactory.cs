using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Drones
{
    public class FactoryMonitorFactory : IFactoryMonitorFactory
    {
        private readonly IRandomGenerator _randomGenerator;

        public FactoryMonitorFactory(IRandomGenerator randomGenerator)
        {
            Assert.IsNotNull(randomGenerator);
            _randomGenerator = randomGenerator;
        }

        public IFactoryMonitor CreateMonitor(IFactory factory)
        {
            int numOfUnitsToBuild = _randomGenerator.Range(minInclusive: 2, maxInclusive: 4);
            return new FactoryMonitor(factory, numOfUnitsToBuild);
        }
    }
}
