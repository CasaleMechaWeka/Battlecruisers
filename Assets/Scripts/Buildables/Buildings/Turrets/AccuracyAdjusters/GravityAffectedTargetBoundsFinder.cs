using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public class GravityAffectedTargetBoundsFinder : ITargetBoundsFinder
    {
        private readonly float _targetXMarginInM;

        public GravityAffectedTargetBoundsFinder(float targetXMarginInM)
        {
            Assert.IsTrue(targetXMarginInM > 0);
            _targetXMarginInM = targetXMarginInM;
        }

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
        public IRange<Vector2> FindTargetBounds(Vector2 sourcePosition, Vector2 targetPosition)
        {
            Assert.IsTrue(sourcePosition.x != targetPosition.x);

            Vector2 minPosition, maxPosition;

            if (sourcePosition.x < targetPosition.x)
            {
                // Firing left to right
                minPosition = new Vector2(targetPosition.x - _targetXMarginInM, targetPosition.y);
                maxPosition = new Vector2(targetPosition.x + _targetXMarginInM, targetPosition.y);
            }
            else
            {
                // Firing right to left
                minPosition = new Vector2(targetPosition.x + _targetXMarginInM, targetPosition.y);
                maxPosition = new Vector2(targetPosition.x - _targetXMarginInM, targetPosition.y);
            }

            return new Range<Vector2>(minPosition, maxPosition);
        }
    }
}
