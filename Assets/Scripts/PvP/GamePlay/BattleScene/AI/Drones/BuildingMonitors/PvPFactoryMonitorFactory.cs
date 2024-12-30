using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones.BuildingMonitors
{
    public class PvPFactoryMonitorFactory : IPvPFactoryMonitorFactory
    {
        private readonly IRandomGenerator _randomGenerator;

        public PvPFactoryMonitorFactory(IRandomGenerator randomGenerator)
        {
            Assert.IsNotNull(randomGenerator);
            _randomGenerator = randomGenerator;
        }

        public IPvPFactoryMonitor CreateMonitor(IPvPFactory factory)
        {
            int numOfUnitsToBuild = _randomGenerator.Range(minInclusive: 2, maxInclusive: 4);
            return new PvPFactoryMonitor(factory, numOfUnitsToBuild);
        }
    }
}
