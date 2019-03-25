using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Factories.Spawning
{
    // FELIX  Test :)
    public class PopulationLimitSpawnDecider : IUnitSpawnDecider
    {
        private readonly ICruiserUnitMonitor _unitMonitor;
        private readonly int _populationLimit;

        public PopulationLimitSpawnDecider(ICruiserUnitMonitor unitMonitor, int populationLimit)
        {
            Assert.IsNotNull(unitMonitor);
            Assert.IsTrue(populationLimit > 0);

            _unitMonitor = unitMonitor;
            _populationLimit = populationLimit;
        }

        public bool CanSpawnUnit(IUnit unitToSpawn)
        {
            return _unitMonitor.AliveUnits.Count < _populationLimit;
        }
    }
}