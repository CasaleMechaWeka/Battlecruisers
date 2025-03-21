using System;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    /// <summary>
    /// NOTE:  All angles are in degrees.
    /// </summary>
    public class AccuracyAdjuster
    {
        private readonly IAngleCalculator _angleCalculator;
        private readonly IRandomGenerator _random;
        private readonly ITurretStats _turretStats;
        private readonly (float x, float y) _targetMargins;

        public AccuracyAdjuster(
            (float x, float y) targetMargins,
            IAngleCalculator angleCalculator = null,
            IRandomGenerator random = null,
            ITurretStats turretStats = null)
        {
            if (targetMargins != (0, 0))
                Helper.AssertIsNotNull(angleCalculator, random, turretStats);

            //_boundsFinder = boundsFinder;
            _targetMargins = targetMargins;
            _angleCalculator = angleCalculator;
            _random = random;
            _turretStats = turretStats;
        }

        public float FindAngleInDegrees(float idealFireAngle, Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored)
        {
            if (_targetMargins == (0, 0))
                return idealFireAngle;

            Assert.IsTrue(sourcePosition.x != targetPosition.x);

            // Calculate the direction sign (-1 for left, +1 for right)
            int direction = Math.Sign(targetPosition.x - sourcePosition.x);

            // Compute min and max target positions using direction sign
            Vector2 minPosition = new Vector2(targetPosition.x - direction * _targetMargins.x, targetPosition.y - _targetMargins.y);
            Vector2 maxPosition = new Vector2(targetPosition.x + direction * _targetMargins.x, targetPosition.y + _targetMargins.y);

            // Calculate firing angles for min/max positions
            float minAngle = _angleCalculator.FindDesiredAngle(sourcePosition, minPosition, isSourceMirrored);
            float maxAngle = _angleCalculator.FindDesiredAngle(sourcePosition, maxPosition, isSourceMirrored);

            Logging.Log(Tags.ACCURACY_ADJUSTERS, $"MinAngle: {minAngle}  MaxAngle: {maxAngle}");

            // Compute fire angle range with accuracy adjustment
            float errorMarginEachSide = ((maxAngle - minAngle) / _turretStats.Accuracy - (maxAngle - minAngle)) * 0.5f;

            float fireMin = minAngle - errorMarginEachSide;
            float fireMax = maxAngle + errorMarginEachSide;

            Logging.Log(Tags.ACCURACY_ADJUSTERS, $"FireAngleRange: {fireMin} - {fireMax}");

            // Return a randomized angle within the computed range
            return _random.Range(fireMin, fireMax);
        }
    }
}
