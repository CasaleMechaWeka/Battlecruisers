using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TestUtils = BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing.Spawners
{
    public abstract class BuildableSpawner : IBuildableSpawner
    {
        protected readonly TestUtils.Helper _helper;

        public const float DEFAULT_SPACING_MULTIPLIER = 1.2f;

        protected BuildableSpawner(TestUtils.Helper helper)
        {
            Assert.IsNotNull(helper);
            _helper = helper;
        }

        public IList<IBuildable> SpawnBuildables(
            IPrefabKey buildableKey, 
            int numOfBuildables, 
            TestUtils.BuildableInitialisationArgs args,
            Vector2 spawnPosition,
            float spacingMultiplier = DEFAULT_SPACING_MULTIPLIER)
        {
            Assert.IsTrue(args.ParentCruiserFacingDirection == Direction.Left 
                || args.ParentCruiserFacingDirection == Direction.Right);

            IList<IBuildable> buildables = new List<IBuildable>(numOfBuildables);

            for (int i = 0; i < numOfBuildables; ++i)
            {
                IBuildable buildable = SpawnBuildable(buildableKey, args);
				buildable.StartConstruction();

                buildable.Position = spawnPosition;
                spawnPosition = IncrementSpawnPosition(spawnPosition, buildable, args.ParentCruiserFacingDirection, spacingMultiplier);

                // Mirror building
                if (args.ParentCruiserFacingDirection == Direction.Left)
                {
                    buildable.Rotation = Helper.MirrorAccrossYAxis(buildable.Rotation);
                }

                buildables.Add(buildable);
            }

            return buildables;
        }

        protected abstract IBuildable SpawnBuildable(IPrefabKey buildableKey, TestUtils.BuildableInitialisationArgs initialisationArgs);

        private Vector2 IncrementSpawnPosition(Vector2 currentPosition, IBuildable buildable, Direction facingDirection, float spacingMultiplier)
        {
            float incrementInM = buildable.Size.x * spacingMultiplier;
            float directionMultiplier = facingDirection == Direction.Right ? -1 : 1;
            return new Vector2(currentPosition.x + directionMultiplier * incrementInM, currentPosition.y);
        }
    }
}
