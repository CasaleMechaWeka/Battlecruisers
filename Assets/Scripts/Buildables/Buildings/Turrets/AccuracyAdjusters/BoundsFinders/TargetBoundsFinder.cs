using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters.BoundsFinders
{
    public abstract class TargetBoundsFinder : ITargetBoundsFinder
    {
        private readonly float _targetXMarginInM;
        private readonly float _targetYMarginInM;

        public TargetBoundsFinder(float targetXMarginInM, float targetYMarginInM)
        {
            Assert.IsTrue(targetXMarginInM > 0 || targetYMarginInM > 0);

            _targetXMarginInM = targetXMarginInM;
            _targetYMarginInM = targetYMarginInM;
        }

        /// <summary>
        /// x margin: 0.5
        /// y margin: 1
        /// 
        /// Eg:
        /// Input:
        ///     source:  -10, 1
        ///     target:  10, 0
        /// 
        /// Output:
        ///     ITargetBounds
        ///         min:  9.5, -1
        ///         max:  10.5, 1
        /// </summary>
        public IRange<Vector2> FindTargetBounds(Vector2 sourcePosition, Vector2 targetPosition)
        {
            Assert.IsTrue(sourcePosition.x != targetPosition.x);

            Vector2 minPosition, maxPosition;

            if (sourcePosition.x < targetPosition.x)
            {
                // Firing left to right
                minPosition = new Vector2(targetPosition.x - _targetXMarginInM, targetPosition.y - _targetYMarginInM);
                maxPosition = new Vector2(targetPosition.x + _targetXMarginInM, targetPosition.y + _targetYMarginInM);
            }
            else
            {
                // Firing right to left
                minPosition = new Vector2(targetPosition.x + _targetXMarginInM, targetPosition.y - _targetYMarginInM);
                maxPosition = new Vector2(targetPosition.x - _targetXMarginInM, targetPosition.y + _targetYMarginInM);
            }

            return new Range<Vector2>(minPosition, maxPosition);
        }
    }
}
