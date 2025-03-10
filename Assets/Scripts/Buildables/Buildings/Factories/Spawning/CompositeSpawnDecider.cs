using BattleCruisers.Buildables.Units;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Factories.Spawning
{
    public class CompositeSpawnDecider : IUnitSpawnDecider
    {
        private readonly IUnitSpawnDecider[] _spawnDeciders;

        public CompositeSpawnDecider(params IUnitSpawnDecider[] spawnDeciders)
        {
            Assert.IsNotNull(spawnDeciders);
            Assert.IsTrue(spawnDeciders.Length > 0);

            _spawnDeciders = spawnDeciders;
        }

        public bool CanSpawnUnit(IUnit unitToSpawn)
        {
            foreach (IUnitSpawnDecider decider in _spawnDeciders)
            {
                if (!decider.CanSpawnUnit(unitToSpawn))
                {
                    return false;
                }
            }

            return true;
        }
    }
}