using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Factories.Spawning
{
    // FELIX  Test :)
    public class PopulationLimitSpawnDecider : IUnitSpawnDecider
    {
        private readonly ICruiserBuildingMonitor _buildingMonitor;
        private readonly int _populationLimit;

        public PopulationLimitSpawnDecider(ICruiserBuildingMonitor buildingMonitor, int populationLimit)
        {
            Assert.IsNotNull(buildingMonitor);
            Assert.IsTrue(populationLimit > 0);

            _buildingMonitor = buildingMonitor;
            _populationLimit = populationLimit;
        }

        public bool CanSpawnUnit(IUnit unitToSpawn)
        {
            return _buildingMonitor.AliveBuildings.Count < _populationLimit;
        }
    }
}