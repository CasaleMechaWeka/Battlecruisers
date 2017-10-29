using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public class GravityAffectedTargetBoundsFinder : ITargetBoundsFinder
    {
        private const float TARGET_MARGIN_X_IN_M = 0.5f;

		/// <summary>
        /// NOTE:  Ignores position y values.
        /// 
        /// Eg:
        /// Input:
        ///     source:  -10, 1
        ///     target:  10, 0
        /// 
        /// Output:
        ///     ITargetBounds
        ///         min:  9.5, 0
        ///         max:  10.5, 0
		/// </summary>
        public ITargetBounds FindTargetBounds(Vector2 sourcePosition, Vector2 targetPosition)
        {
            Assert.IsTrue(sourcePosition.x != targetPosition.x);

            Vector2 minPosition, maxPosition;

            if (sourcePosition.x < targetPosition.x)
            {
                // Firing left to right
                minPosition = new Vector2(targetPosition.x - TARGET_MARGIN_X_IN_M, targetPosition.y);
                maxPosition = new Vector2(targetPosition.x + TARGET_MARGIN_X_IN_M, targetPosition.y);
            }
            else
            {
                // Firing right to left
                minPosition = new Vector2(targetPosition.x + TARGET_MARGIN_X_IN_M, targetPosition.y);
                maxPosition = new Vector2(targetPosition.x - TARGET_MARGIN_X_IN_M, targetPosition.y);
            }

            return new TargetBounds(minPosition, maxPosition);
        }
    }
}
