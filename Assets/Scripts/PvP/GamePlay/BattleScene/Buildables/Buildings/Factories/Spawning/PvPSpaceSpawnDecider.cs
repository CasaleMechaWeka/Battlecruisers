using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories.Spawning
{
    /// <summary>
    /// Checks whether there is enough room to spawn a unit, without
    /// hitting any other unit.
    /// </summary>
    public class PvPSpaceSpawnDecider : IPvPUnitSpawnDecider
    {
        private readonly IPvPFactory _factory;
        private readonly IPvPUnitSpawnPositionFinder _unitSpawnPositionFinder;

        public const float SPAWN_RADIUS_MULTIPLIER = 1.2f;

        public PvPSpaceSpawnDecider(IPvPFactory factory, IPvPUnitSpawnPositionFinder unitSpawnPositionFinder)
        {
            PvPHelper.AssertIsNotNull(factory, unitSpawnPositionFinder);

            _factory = factory;
            _unitSpawnPositionFinder = unitSpawnPositionFinder;
        }

        public bool CanSpawnUnit(IPvPUnit unitToSpawn)
        {
            PvPHelper.AssertIsNotNull(unitToSpawn);

            Vector3 spawnPositionV3 = _unitSpawnPositionFinder.FindSpawnPosition(unitToSpawn);
            Vector2 spawnPositionV2 = new Vector2(spawnPositionV3.x, spawnPositionV3.y);
            float spawnRadius = SPAWN_RADIUS_MULTIPLIER * unitToSpawn.Size.x;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPositionV2, spawnRadius, _factory.UnitLayerMask);

            foreach (Collider2D collider in colliders)
            {
                IPvPUnit blockingUnit = collider.GetComponent<IPvPTargetProxy>()?.Target as IPvPUnit;

                if (blockingUnit != null
                    && blockingUnit.TargetType == unitToSpawn.TargetType
                    && blockingUnit.BuildableState == PvPBuildableState.Completed)
                {
                    return false;
                }
            }

            return true;
        }
    }
}