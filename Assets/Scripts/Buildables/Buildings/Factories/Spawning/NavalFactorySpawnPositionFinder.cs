using BattleCruisers.Buildables.Units;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Factories.Spawning
{
    public class NavalFactorySpawnPositionFinder : IUnitSpawnPositionFinder
    {
        private readonly IFactory _factory;

        public NavalFactorySpawnPositionFinder(IFactory factory)
        {
            Assert.IsNotNull(factory);
            _factory = factory;
        }

        public Vector3 FindSpawnPosition(IUnit unitToSpawn)
        {
            float horizontalChange = (_factory.Size.x * 0.6f) + (unitToSpawn.Size.x * 0.5f);

            // If the factory is facing left it has been mirrored (rotated
            // around the y-axis by 180*).  So its right is an unmirrored
            // factory's left :/
            Vector3 direction = _factory.Transform.Right;

            return _factory.Transform.Position + (direction * horizontalChange);
        }
    }
}