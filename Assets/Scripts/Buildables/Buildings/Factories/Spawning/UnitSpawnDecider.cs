using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Factories.Spawning
{
    public class UnitSpawnDecider : IUnitSpawnDecider
    {
        private readonly IFactory _factory;
        private readonly IUnitSpawnPositionFinder _unitSpawnPositionFinder;
        private readonly IUnitSpawnTimer _unitSpawnTimer;

        public const float MIN_BUILD_BREAK_IN_S = 0.5f;
        public const float SPAWN_RADIUS_MULTIPLIER = 1.2f;

        public UnitSpawnDecider(IFactory factory, IUnitSpawnPositionFinder unitSpawnPositionFinder, IUnitSpawnTimer unitSpawnTimer)
        {
            Helper.AssertIsNotNull(factory, unitSpawnPositionFinder, unitSpawnTimer);

            _factory = factory;
            _unitSpawnPositionFinder = unitSpawnPositionFinder;
            _unitSpawnTimer = unitSpawnTimer;
        }

        public bool CanSpawnUnit(IUnit unitToSpawn)
        {
            Helper.AssertIsNotNull(unitToSpawn);

            // If the unit under construction is destroyed, do not want to immediately
            // start buliding the next unit.  This avoids the factory being "protected"
            // by instantly respawning in progress units.  Ignore if the unit under 
            // construction was recently changed.
            Logging.Verbose(Tags.FACTORY, "UnitSpawnDecider.CanSpawnUnit(): " + unitToSpawn
                + "  time since chosen: " + _unitSpawnTimer.TimeSinceUnitWasChosenInS
                + "  time since clear:  " + _unitSpawnTimer.TimeSinceFactoryWasClearInS);

            if (_unitSpawnTimer.TimeSinceUnitWasChosenInS >= MIN_BUILD_BREAK_IN_S
                && _unitSpawnTimer.TimeSinceFactoryWasClearInS <= MIN_BUILD_BREAK_IN_S)
            {
                Logging.Verbose(Tags.FACTORY, "UnitSpawnDecider.CanSpawnUnit():  times mean false :)");

                return false;
            }

            Vector3 spawnPositionV3 = _unitSpawnPositionFinder.FindSpawnPosition(unitToSpawn);
            Vector2 spawnPositionV2 = new Vector2(spawnPositionV3.x, spawnPositionV3.y);
            float spawnRadius = SPAWN_RADIUS_MULTIPLIER * unitToSpawn.Size.x;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPositionV2, spawnRadius, _factory.UnitLayerMask);

            foreach (Collider2D collider in colliders)
            {
                IUnit blockingUnit = collider.GetComponent<IUnit>();

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