using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones.BuildingMonitors
{
    public class PvPFactoryMonitorFactory : IPvPFactoryMonitorFactory
    {
        private readonly IPvPRandomGenerator _randomGenerator;

        public PvPFactoryMonitorFactory(IPvPRandomGenerator randomGenerator)
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
