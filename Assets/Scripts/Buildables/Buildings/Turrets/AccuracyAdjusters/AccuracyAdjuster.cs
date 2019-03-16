using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters.BoundsFinders;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    /// <summary>
    /// NOTE:  All angles are in degrees.
    /// </summary>
    public class AccuracyAdjuster : IAccuracyAdjuster
    {
        private readonly ITargetBoundsFinder _boundsFinder;
        private readonly IAngleCalculator _angleCalculator;
        private readonly IAngleRangeFinder _angleRangeFinder;
        private readonly IRandomGenerator _random;
        private readonly ITurretStats _turretStats;

        public AccuracyAdjuster(
            ITargetBoundsFinder boundsFinder,
            IAngleCalculator angleCalculator,
            IAngleRangeFinder angleRangeFinder,
            IRandomGenerator random,
            ITurretStats turretStats)
        {
            Helper.AssertIsNotNull(boundsFinder, angleCalculator, angleRangeFinder, random, turretStats);

            _boundsFinder = boundsFinder;
            _angleCalculator = angleCalculator;
            _angleRangeFinder = angleRangeFinder;
            _random = random;
            _turretStats = turretStats;
        }

        public float FindAngleInDegrees(float idealFireAngle, Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored)
        {
            IRange<Vector2> onTargetBounds = _boundsFinder.FindTargetBounds(sourcePosition, targetPosition);

            float angleForCloserTarget = _angleCalculator.FindDesiredAngle(sourcePosition, onTargetBounds.Min, isSourceMirrored);
            float angleForFurtherTarget = _angleCalculator.FindDesiredAngle(sourcePosition, onTargetBounds.Max, isSourceMirrored);

            Logging.Log(Tags.ACCURACY_ADJUSTERS, $"angleForCloserTarget: {angleForCloserTarget}  angleForFurtherTarget: {angleForFurtherTarget}");

            IRange<float> onTargetAngleRange = new OrderedRange(angleForCloserTarget, angleForFurtherTarget);
            IRange<float> fireAngleRange = _angleRangeFinder.FindFireAngleRange(onTargetAngleRange, _turretStats.Accuracy);

            Logging.Log(Tags.ACCURACY_ADJUSTERS, $"fireAngleRange: {fireAngleRange.Min} - {fireAngleRange.Max}");

            return _random.Range(fireAngleRange.Min, fireAngleRange.Max);
        }
    }
}
   