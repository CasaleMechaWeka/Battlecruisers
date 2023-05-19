using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public class PvPSpawnPositionFinder : IPvPSpawnPositionFinder
    {
        private readonly IPvPRandomGenerator _random;
        private readonly float _waterLine;

        public PvPSpawnPositionFinder(IPvPRandomGenerator random, float waterLine)
        {
            Assert.IsNotNull(random);

            _random = random;
            _waterLine = waterLine;
        }

        public Vector2 FindSpawnPosition(IPvPDroneConsumerInfo droneConsumerInfo)
        {
            Assert.IsNotNull(droneConsumerInfo);

            float xDeltaInM = droneConsumerInfo.Size.x / 2;
            IPvPRange<float> xPositionRange
                = new PvPRange<float>(
                    droneConsumerInfo.Position.x - xDeltaInM,
                    droneConsumerInfo.Position.x + xDeltaInM);

            float yDeltaInM = droneConsumerInfo.Size.y / 2;
            IPvPRange<float> yPositionRange
                = new PvPRange<float>(
                    // Drones must be above the water line
                    Mathf.Max(droneConsumerInfo.Position.y - yDeltaInM, _waterLine),
                    Mathf.Max(droneConsumerInfo.Position.y + yDeltaInM, _waterLine));

            float xPosition = _random.Range(xPositionRange);
            float yPosition = _random.Range(yPositionRange);

            return new Vector2(xPosition, yPosition);
        }
    }
}