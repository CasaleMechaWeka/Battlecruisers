using BattleCruisers.Buildables.Units;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Factories.Spawning
{
    public class AirFactorySpawnPositionFinder : IUnitSpawnPositionFinder
    {
        private readonly IFactory _factory;

        public AirFactorySpawnPositionFinder(IFactory factory)
        {
            Assert.IsNotNull(factory);
            _factory = factory;
        }

        public Vector3 FindSpawnPosition(IUnit unitToSpawn)
        {
            float verticalChange = (_factory.Size.y * 0.7f) + (unitToSpawn.Size.y * 0.5f);
            return _factory.Transform.Position + (_factory.Transform.Up * verticalChange);
        }
    }
}