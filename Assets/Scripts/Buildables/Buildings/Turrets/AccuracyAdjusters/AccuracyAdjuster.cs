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
		private readonly IRandomGenerator _random;
        private readonly float _projectileVelocityInMPerS;
        private readonly float _accuracy;

        private const float MIN_ACCURACY = 0;
        private const float MAX_ACCURACY = 1;

        public AccuracyAdjuster(
            ITargetBoundsFinder boundsFinder,
            IAngleCalculator angleCalculator,
            IRandomGenerator random,
            float projectileVelocityInMPerS,
            float accuracy)
        {
            Helper.AssertIsNotNull(boundsFinder, angleCalculator, random);
            Assert.IsTrue(projectileVelocityInMPerS > 0);
            Assert.IsTrue(accuracy > MIN_ACCURACY && accuracy < MAX_ACCURACY);

            _boundsFinder = boundsFinder;
            _angleCalculator = angleCalculator;
            _random = random;
            _projectileVelocityInMPerS = projectileVelocityInMPerS;
            _accuracy = accuracy;
        }

        public float FindAngleInDegrees(float idealFireAngle, Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored)
        {
            IRange<Vector2> onTargetBounds = _boundsFinder.FindTargetBounds(sourcePosition, targetPosition);

            float minOnTargetAngle = _angleCalculator.FindDesiredAngle(sourcePosition, onTargetBounds.Min, isSourceMirrored, _projectileVelocityInMPerS);
			float maxOnTargetAngle = _angleCalculator.FindDesiredAngle(sourcePosition, onTargetBounds.Max, isSourceMirrored, _projectileVelocityInMPerS);

            Logging.Log(Tags.ACCURACY_ADJUSTERS, "minOnTargetAngle: " + minOnTargetAngle + "  maxOnTargetAngle: " + maxOnTargetAngle);

            IRange<float> fireAngleRange = FindFireAngleRange(new Range<float>(minOnTargetAngle, maxOnTargetAngle));

            Logging.Log(Tags.ACCURACY_ADJUSTERS, "fireAngleRange: " + fireAngleRange.Min + " - " + fireAngleRange.Max);

            return _random.Range(fireAngleRange.Min, fireAngleRange.Max);
        }

        /// <returns>
        /// The possible range of fire angles.  Includes the given on target range,
        /// with an error margin either side.
        /// </returns>
        private IRange<float> FindFireAngleRange(IRange<float> onTargetRange)
        {
            Assert.IsTrue(onTargetRange.Max > onTargetRange.Min);
			
            float onTargetRangeSize = onTargetRange.Max - onTargetRange.Min;

            float fireAngleRangeSize = onTargetRangeSize / _accuracy;
            Assert.IsTrue(fireAngleRangeSize > onTargetRangeSize);

            float totalErrorMargin = fireAngleRangeSize - onTargetRangeSize;
            float errorMarginEachSide = totalErrorMargin / 2;

            float minFireAngle = onTargetRange.Min - errorMarginEachSide;
            float maxFireAngle  = onTargetRange.Max + errorMarginEachSide;

            return new Range<float>(minFireAngle, maxFireAngle);
        }
    }
}
