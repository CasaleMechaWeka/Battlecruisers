using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders
{
    public static class ClosestPositionFinder
    {
        private const float TARGET_CUTOFF_WIDTH_IN_M = 3;
        private const float TARGET_BUFFER_IN_M = 1;

        public static Vector2 FindClosestAttackablePosition(Vector2 sourcePosition, ITarget target)
        {
            if (target.Size.x <= TARGET_CUTOFF_WIDTH_IN_M)
                return target.Position; // Small target, no adjustment needed.

            float xChange = target.Size.x * 0.5f - TARGET_BUFFER_IN_M;
            float direction = Mathf.Sign(target.Position.x - sourcePosition.x);

            Vector2 closestAttackablePosition = new Vector2(target.Position.x - direction * xChange, target.Position.y);
            Logging.Verbose(Tags.CLOSEST_POSITION_FINDER, $"Target position: {target.Position}, Attackable position: {closestAttackablePosition}, Target size: {target.Size}");

            return closestAttackablePosition;
        }
    }
}
