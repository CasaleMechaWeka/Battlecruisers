using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.Helpers
{
    public class ShipRangeHelper : ITargetRangeHelper
    {
        private readonly IShip _ship;

        // Frigate would have optimal range of 18.63 but target was at 18.6305.
        // Hence provide a tiny bit of leeway, so target is counted as in range.
        private const float IN_RANGE_LEEWAY_IN_M = 0.01f;

        public ShipRangeHelper(IShip ship)
        {
            Assert.IsNotNull(ship);
            _ship = ship;
        }

        public bool IsTargetInRange(ITarget target)
        {
            float distanceCenterToCenter = Vector2.Distance(target.Position, _ship.Position);
            float distanceCenterToEdge = distanceCenterToCenter - target.Size.x / 2;
            float adjustedDistanceToTarget = distanceCenterToEdge - IN_RANGE_LEEWAY_IN_M;

            Logging.Log(Tags.TARGET_RANGE_HELPER, $"Target: {target}  Distance: {adjustedDistanceToTarget}  Range: {_ship.OptimalArmamentRangeInM}");

            return adjustedDistanceToTarget <= _ship.OptimalArmamentRangeInM;            
        }
    }
}
