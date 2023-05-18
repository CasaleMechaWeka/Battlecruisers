using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories.Spawning
{
    public class PvPNavalFactorySpawnPositionFinder : IPvPUnitSpawnPositionFinder
    {
        private readonly IPvPFactory _factory;

        public PvPNavalFactorySpawnPositionFinder(IPvPFactory factory)
        {
            Assert.IsNotNull(factory);
            _factory = factory;
        }

        public Vector3 FindSpawnPosition(IPvPUnit unitToSpawn)
        {
            float horizontalChange = (_factory.Size.x * 0.6f) + (unitToSpawn.Size.x * 0.5f);

            // If the factory is facing left it has been mirrored (rotated
            // around the y-axis by 180*). So its right is an unmirrored
            // factory's left :/
            Vector3 direction = _factory.Transform.Right;

            Vector3 spawnPosition = _factory.Transform.Position + (direction * horizontalChange);

            float yOffset = -0.35f; // Adjust this value based on your requirements
            spawnPosition.y += yOffset; // Apply the vertical offset

            return spawnPosition;
        }
    }
}
