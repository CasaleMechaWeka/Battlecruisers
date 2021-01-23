using BattleCruisers.Buildables.Proxy;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Factories.Spawning
{
    /// <summary>
    /// Checks whether there is enough room to spawn a unit, without
    /// hitting any other unit.
    /// </summary>
    public class SpaceSpawnDecider : IUnitSpawnDecider
    {
        private readonly IFactory _factory;
        private readonly IUnitSpawnPositionFinder _unitSpawnPositionFinder;

        public const float SPAWN_RADIUS_MULTIPLIER = 1.2f;

        public SpaceSpawnDecider(IFactory factory, IUnitSpawnPositionFinder unitSpawnPositionFinder)
        {
            Helper.AssertIsNotNull(factory, unitSpawnPositionFinder);

            _factory = factory;
            _unitSpawnPositionFinder = unitSpawnPositionFinder;
        }

        public bool CanSpawnUnit(IUnit unitToSpawn)
        {
            Helper.AssertIsNotNull(unitToSpawn);

            Vector3 spawnPositionV3 = _unitSpawnPositionFinder.FindSpawnPosition(unitToSpawn);
            Vector2 spawnPositionV2 = new Vector2(spawnPositionV3.x, spawnPositionV3.y);
            float spawnRadius = SPAWN_RADIUS_MULTIPLIER * unitToSpawn.Size.x;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPositionV2, spawnRadius, _factory.UnitLayerMask);

            foreach (Collider2D collider in colliders)
            {
                IUnit blockingUnit = collider.GetComponent<ITargetProxy>()?.Target as IUnit;

                if (blockingUnit != null
                    && blockingUnit.TargetType == unitToSpawn.TargetType)
                {
                    return false;
                }
            }

            return true;
        }
    }
}