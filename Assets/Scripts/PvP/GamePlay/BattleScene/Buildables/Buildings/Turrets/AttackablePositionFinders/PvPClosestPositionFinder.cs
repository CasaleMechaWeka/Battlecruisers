using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AttackablePositionFinders
{
    /// <summary>
    /// Designed for large targets (cruisers), where the attack boat may be
    /// in range, but out of range of the cruiser center (default attack
    /// position).
    /// 
    /// Hence want to find a position on the cruiser the attack boat can attack
    /// (ie, front of cruiser).
    /// </summary>
    public class PvPClosestPositionFinder : IPvPAttackablePositionFinder
    {
        private readonly float _targetCutoffWidthInM;

        // Do not want the very front or back of the target.  Want to
        // be slightly inside the target.  Eg, if the raptor is positioned
        // from 0 to 6, would want the front attack position to be ~1
        // and the rear attack position ~5.
        private readonly float _targetBufferInM;

        // Anti-air turret:     0.4 x 0.3
        // Raptor cruiser:      9 x 6
        // Longbow cruiser:     19 x 6
        private const float DEFAULT_TARGET_CUTOFF_WIDTH_IN_M = 3;
        private const float DEFAULT_TARGET_BUFFER_IN_M = 1;
        private const float MAX_BUFFER_TO_CUTOFF_RATIO = 0.4f;  // (1 / 3) > 0.4

        public PvPClosestPositionFinder(
            float targetCutoffWidthInM = DEFAULT_TARGET_CUTOFF_WIDTH_IN_M,
            float targetBufferInM = DEFAULT_TARGET_BUFFER_IN_M)
        {
            Assert.IsTrue((targetBufferInM / targetCutoffWidthInM) <= MAX_BUFFER_TO_CUTOFF_RATIO);

            _targetCutoffWidthInM = targetCutoffWidthInM;
            _targetBufferInM = targetBufferInM;
        }

        public Vector2 FindClosestAttackablePosition(Vector2 sourcePosition, IPvPTarget target)
        {
            if (target.Size.x <= _targetCutoffWidthInM)
            {
                // Target is small, so whether attacking the front or center or back
                // should not make any difference.
                return target.Position;
            }

            float attackPositionX = target.Position.x + (FindXChangeInM(target.Size.x) * FindDirectionMultiplier(sourcePosition, target.Position));
            Vector2 closestAttackablePosition = new Vector2(attackPositionX, target.Position.y);

            // Logging.Verbose(Tags.CLOSEST_POSITION_FINDER, $"Target position: {target.Position}  Attackable position: {closestAttackablePosition}  Target size: {target.Size}");
            return closestAttackablePosition;
        }

        private float FindXChangeInM(float targetWidthInM)
        {
            return targetWidthInM / 2 - _targetBufferInM;
        }

        private int FindDirectionMultiplier(Vector2 sourcePosition, Vector2 targetPosition)
        {
            if (sourcePosition.x < targetPosition.x)
            {
                // Target is to the right, want to reduce target position
                return -1;
            }
            else
            {
                // Target is to the left, want to increase target position
                return 1;
            }
        }
    }
}