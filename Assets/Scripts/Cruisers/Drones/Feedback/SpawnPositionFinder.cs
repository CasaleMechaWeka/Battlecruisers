using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public class SpawnPositionFinder : ISpawnPositionFinder
    {
        private readonly IRandomGenerator _random;
        private readonly float _waterLine;

        public SpawnPositionFinder(IRandomGenerator random, float waterLine)
        {
            Assert.IsNotNull(random);

            _random = random;
            _waterLine = waterLine;
        }

        public Vector2 FindSpawnPosition(IDroneConsumerInfo droneConsumerInfo)
        {
            Assert.IsNotNull(droneConsumerInfo);

            float xDeltaInM = droneConsumerInfo.Size.x / 2;
            IRange<float> xPositionRange 
                = new Range<float>(
                    droneConsumerInfo.Position.x - xDeltaInM, 
                    droneConsumerInfo.Position.x + xDeltaInM);

            float yDeltaInM = droneConsumerInfo.Size.y / 2;
            IRange<float> yPositionRange 
                = new Range<float>(
                    // Drones must be above the water line
                    Mathf.Max(droneConsumerInfo.Position.y - yDeltaInM, _waterLine),
                    Mathf.Max(droneConsumerInfo.Position.y + yDeltaInM, _waterLine));

            return 
                new Vector2(
                    _random.Range(xPositionRange),
                    _random.Range(yPositionRange));
        }
    }
}