using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters.BoundsFinders;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
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
        private readonly ITargetBoundsFinder _boundsFinder;
        private readonly IAngleCalculator _angleCalculator;
        private readonly IAngleRangeFinder _angleRangeFinder;
        private readonly IRandomGenerator _random;
        private readonly float _projectileVelocityInMPerS;
        private readonly float _accuracy;

        public const float MIN_ACCURACY = 0;
        public const float MAX_ACCURACY = 1;

        public AccuracyAdjuster(
            ITargetBoundsFinder boundsFinder,
            IAngleCalculator angleCalculator,
            IAngleRangeFinder angleRangeFinder,
            IRandomGenerator random,
            float projectileVelocityInMPerS,
            float accuracy)
        {
            Helper.AssertIsNotNull(boundsFinder, angleCalculator, angleRangeFinder, random);
            Assert.IsTrue(projectileVelocityInMPerS > 0);
            Assert.IsTrue(accuracy > MIN_ACCURACY && accuracy < MAX_ACCURACY);

            _boundsFinder = boundsFinder;
            _angleCalculator = angleCalculator;
            _angleRangeFinder = angleRangeFinder;
            _random = random;
            _projectileVelocityInMPerS = projectileVelocityInMPerS;
            _accuracy = accuracy;
        }

        public float FindAngleInDegrees(float idealFireAngle, Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored)
        {
            IRange<Vector2> onTargetBounds = _boundsFinder.FindTargetBounds(sourcePosition, targetPosition);

            float angleForCloserTarget = _angleCalculator.FindDesiredAngle(sourcePosition, onTargetBounds.Min, isSourceMirrored, _projectileVelocityInMPerS);
            float angleForFurtherTarget = _angleCalculator.FindDesiredAngle(sourcePosition, onTargetBounds.Max, isSourceMirrored, _projectileVelocityInMPerS);

            Logging.Log(Tags.ACCURACY_ADJUSTERS, "angleForCloserTarget: " + angleForCloserTarget + "  angleForFurtherTarget: " + angleForFurtherTarget);

            IRange<float> onTargetAngleRange = new OrderedRange(angleForCloserTarget, angleForFurtherTarget);
            IRange<float> fireAngleRange = _angleRangeFinder.FindFireAngleRange(onTargetAngleRange, _accuracy);

            Logging.Log(Tags.ACCURACY_ADJUSTERS, "fireAngleRange: " + fireAngleRange.Min + " - " + fireAngleRange.Max);

            return _random.Range(fireAngleRange.Min, fireAngleRange.Max);
        }
    }
}
   