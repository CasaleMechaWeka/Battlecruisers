using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public class SpawnPositionFinder : ISpawnPositionFinder
    {
        private readonly IRandomGenerator _random;

        public SpawnPositionFinder(IRandomGenerator random)
        {
            Assert.IsNotNull(random);
            _random = random;
        }

        public Vector2 FindSpawnPosition(IDroneConsumerInfo droneConsumerInfo)
        {
            Assert.IsNotNull(droneConsumerInfo);

            float x = _random.Range(-droneConsumerInfo.Size.x / 2, droneConsumerInfo.Size.x / 2);
            float y = _random.Range(-droneConsumerInfo.Size.y / 2, droneConsumerInfo.Size.y / 2);
            return new Vector2(droneConsumerInfo.Position.x + x, droneConsumerInfo.Position.y + y);
        }
    }
}