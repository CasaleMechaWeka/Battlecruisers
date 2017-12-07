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
    // FELIX  Avoid duplicate code with UnitSpawner
    // FELIX  Use in DefenceBuildingBalancingTest :)
    public class BuildingSpawner : IBuildableSpawner
    {
        private readonly IPrefabFactory _prefabFactory;
        private readonly TestUtils.Helper _helper;

        private const float SPACING_MULTIPLIER = 1.2f;

        public BuildingSpawner(IPrefabFactory prefabFactory, TestUtils.Helper helper)
        {
            Helper.AssertIsNotNull(prefabFactory, helper);

            _prefabFactory = prefabFactory;
            _helper = helper;
        }

        // FELIX  Take on building completed/started callback :)
        public void SpawnBuildables(IPrefabKey buildableKey, int numOfBuildables, Direction facingDirection, Vector2 spawnPosition)
        {
            Assert.IsTrue(facingDirection == Direction.Left || facingDirection == Direction.Right);

            for (int i = 0; i < numOfBuildables; ++i)
            {
                IBuildableWrapper<IBuilding> buildingWrapper = _prefabFactory.GetBuildingWrapperPrefab(buildableKey);
                IBuilding building = _prefabFactory.CreateBuilding(buildingWrapper);
                _helper.InitialiseBuilding(building, Faction.Reds, parentCruiserDirection: Direction.Left);

                building.Position = spawnPosition;
                spawnPosition = IncrementSpawnPosition(spawnPosition, building, facingDirection);

                // Mirror building
                if (facingDirection == Direction.Left)
                {
                    building.Rotation = Helper.MirrorAccrossYAxis(building.Rotation);
				}

                building.StartConstruction();
            }
        }

        private Vector2 IncrementSpawnPosition(Vector2 currentPosition, IBuildable buildable, Direction facingDirection)
        {
            float incrementInM = buildable.Size.x * SPACING_MULTIPLIER;
            float directionMultiplier = facingDirection == Direction.Right ? -1 : 1;
            return new Vector2(currentPosition.x + directionMultiplier * incrementInM, currentPosition.y);
        }
    }
}
