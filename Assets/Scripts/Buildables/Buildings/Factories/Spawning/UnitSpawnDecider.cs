using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Factories.Spawning
{
    public class UnitSpawnDecider : IUnitSpawnDecider
    {
        private readonly IFactory _factory;
        private readonly IUnitSpawnPositionFinder _unitSpawnPositionFinder;

        public const float SPAWN_RADIUS_MULTIPLIER = 1.2f;

        public UnitSpawnDecider(IFactory factory, IUnitSpawnPositionFinder unitSpawnPositionFinder)
        {
            Helper.AssertIsNotNull(factory, unitSpawnPositionFinder);

            _factory = factory;
            _unitSpawnPositionFinder = unitSpawnPositionFinder;
        }

        public bool CanSpawnUnit(IUnit unitToSpawn)
        {
            if (_factory.LastUnitProduced != null && !_factory.LastUnitProduced.IsDestroyed)
            {
                Vector3 spawnPositionV3 = _unitSpawnPositionFinder.FindSpawnPosition(unitToSpawn);
                Vector2 spawnPositionV2 = new Vector2(spawnPositionV3.x, spawnPositionV3.y);
                float spawnRadius = SPAWN_RADIUS_MULTIPLIER * unitToSpawn.Size.x;
                Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPositionV2, spawnRadius, _factory.UnitLayerMask);

                foreach (Collider2D collider in colliders)
                {
                    if (collider.gameObject == _factory.LastUnitProduced.GameObject)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}