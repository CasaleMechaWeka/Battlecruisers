using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using TestUtils = BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public abstract class BuildableSpawner : IBuildableSpawner
    {
        protected readonly IPrefabFactory _prefabFactory;
        protected readonly TestUtils.Helper _helper;

        private const float SPACING_MULTIPLIER = 1.2f;

        public BuildableSpawner(IPrefabFactory prefabFactory, TestUtils.Helper helper)
        {
            Helper.AssertIsNotNull(prefabFactory, helper);

            _prefabFactory = prefabFactory;
            _helper = helper;
        }

        // FELIX  Take on building completed/started callback :)
        public void SpawnBuildables(
            IPrefabKey buildableKey, 
            int numOfBuildables, 
            Faction faction, 
            Direction facingDirection, 
            Vector2 spawnPosition)
        {
            Assert.IsTrue(facingDirection == Direction.Left || facingDirection == Direction.Right);

            for (int i = 0; i < numOfBuildables; ++i)
            {
                IBuildable buildable = SpawnBuildable(buildableKey, faction, facingDirection);

                buildable.Position = spawnPosition;
                spawnPosition = IncrementSpawnPosition(spawnPosition, buildable, facingDirection);

                // Mirror building
                if (facingDirection == Direction.Left)
                {
                    buildable.Rotation = Helper.MirrorAccrossYAxis(buildable.Rotation);
                }

                buildable.StartConstruction();
            }
        }

        protected abstract IBuildable SpawnBuildable(IPrefabKey buildableKey, Faction faction, Direction facingDirection);

        private Vector2 IncrementSpawnPosition(Vector2 currentPosition, IBuildable buildable, Direction facingDirection)
        {
            float incrementInM = buildable.Size.x * SPACING_MULTIPLIER;
            float directionMultiplier = facingDirection == Direction.Right ? -1 : 1;
            return new Vector2(currentPosition.x + directionMultiplier * incrementInM, currentPosition.y);
        }
    }
}
