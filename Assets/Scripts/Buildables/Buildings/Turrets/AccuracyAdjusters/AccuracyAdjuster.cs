using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    /// <summary>
    /// NOTE:  All angles are in degrees.
    /// </summary>
    public class AccuracyAdjuster : IAccuracyAdjuster
    {
        private readonly IAngleCalculator _angleCalculator;
        private readonly IRandomGenerator _random;
        private readonly ITurretStats _turretStats;
        private (float x, float y) _targetMargins;

        public AccuracyAdjuster(
            //TargetBoundsFinder boundsFinder,
            (float x, float y) targetMargins,
            IAngleCalculator angleCalculator,
            IRandomGenerator random,
            ITurretStats turretStats)
        {
            Helper.AssertIsNotNull(angleCalculator, random, turretStats);

            //_boundsFinder = boundsFinder;
            _targetMargins = targetMargins;
            _angleCalculator = angleCalculator;
            _random = random;
            _turretStats = turretStats;
        }

        public float FindAngleInDegrees(float idealFireAngle, Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored)
        {
            Assert.IsTrue(sourcePosition.x != targetPosition.x);
            Vector2 minPosition, maxPosition;

            if (sourcePosition.x < targetPosition.x)
            {
                // Firing left to right
                minPosition = new Vector2(targetPosition.x - _targetMargins.x, targetPosition.y - _targetMargins.y);
                maxPosition = new Vector2(targetPosition.x + _targetMargins.x, targetPosition.y + _targetMargins.y);
            }
            else
            {
                // Firing right to left
                minPosition = new Vector2(targetPosition.x + _targetMargins.x, targetPosition.y - _targetMargins.y);
                maxPosition = new Vector2(targetPosition.x - _targetMargins.x, targetPosition.y + _targetMargins.y);
            }

            IRange<Vector2> onTargetBounds = new Range<Vector2>(minPosition, maxPosition);

            float angleForCloserTarget = _angleCalculator.FindDesiredAngle(sourcePosition, onTargetBounds.Min, isSourceMirrored);
            float angleForFurtherTarget = _angleCalculator.FindDesiredAngle(sourcePosition, onTargetBounds.Max, isSourceMirrored);

            Logging.Log(Tags.ACCURACY_ADJUSTERS, $"angleForCloserTarget: {angleForCloserTarget}  angleForFurtherTarget: {angleForFurtherTarget}");

            IRange<float> onTargetAngleRange = new OrderedRange(angleForCloserTarget, angleForFurtherTarget);

            float onTargetRangeSize = onTargetAngleRange.Max - onTargetAngleRange.Min;
            float errorMarginEachSide = (onTargetRangeSize / _turretStats.Accuracy - onTargetRangeSize) * 0.5f;

            IRange<float> fireAngleRange = new Range<float>(onTargetAngleRange.Min - errorMarginEachSide,
                                                            onTargetAngleRange.Max + errorMarginEachSide);

            Logging.Log(Tags.ACCURACY_ADJUSTERS, $"fireAngleRange: {fireAngleRange.Min} - {fireAngleRange.Max}");

            return _random.Range(fireAngleRange.Min, fireAngleRange.Max);
        }
    }
}
