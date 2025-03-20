using System;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    /// <summary>
    /// Assumes:
    /// 1. Shells ARE affected by gravity
    /// 2. Target is in facing direction of source
    /// </summary>
    public class GravityAffectedAngleCalculator : AngleCalculator
    {
        private readonly IProjectileFlightStats _projectileFlightStats;
        private readonly float _adjustedGravity;
        private float previousAngle;

        private bool _useLargerAngle { get; }

        public GravityAffectedAngleCalculator(IProjectileFlightStats projectileFlightStats, bool useLargerAngle)
            : base()
        {
            Helper.AssertIsNotNull(projectileFlightStats);
            Assert.IsTrue(projectileFlightStats.GravityScale > 0);

            _projectileFlightStats = projectileFlightStats;
            _adjustedGravity = Constants.GRAVITY * projectileFlightStats.GravityScale;
            _useLargerAngle = useLargerAngle;
        }

        public override float FindDesiredAngle(Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored)
        {
            if (!Helper.IsFacingTarget(targetPosition, sourcePosition, isSourceMirrored))
            {
                return previousAngle;
                //throw new ArgumentException("Source does not face target :(  source: " + sourcePosition + "  target: " + targetPosition + "  isSourceMirrored: " + isSourceMirrored);
            }

            float distanceInM = Math.Abs(sourcePosition.x - targetPosition.x);
            float targetAltitude = targetPosition.y - sourcePosition.y;

            float velocitySquared = _projectileFlightStats.MaxVelocityInMPerS * _projectileFlightStats.MaxVelocityInMPerS;
            float squareRootArg = (velocitySquared * velocitySquared) - _adjustedGravity * ((_adjustedGravity * distanceInM * distanceInM) + (2 * targetAltitude * velocitySquared));

            if (squareRootArg < 0)
            {
                return previousAngle;
                //throw new ArgumentException("Out of range :/  source: " + sourcePosition + "  target: " + targetPosition);
            }

            float denominator = _adjustedGravity * distanceInM;
            float firstAngleInRadians = Mathf.Atan((velocitySquared + Mathf.Sqrt(squareRootArg)) / denominator);
            float secondAngleInRadians = Mathf.Atan((velocitySquared - Mathf.Sqrt(squareRootArg)) / denominator);

            float angleInRadians = _useLargerAngle ? Mathf.Max(firstAngleInRadians, secondAngleInRadians) : Mathf.Min(firstAngleInRadians, secondAngleInRadians);
            float angleInDegrees = angleInRadians * Mathf.Rad2Deg;

            if (angleInDegrees < 0)
                angleInDegrees += 360;

            previousAngle = angleInDegrees;
            Logging.Verbose(
                Tags.ANGLE_CALCULATORS,
                $"source: {sourcePosition}  target: {targetPosition}  isSourceMirrored: {isSourceMirrored}  UseLargerAngle: {_useLargerAngle}  " +
                $"firstAngleInRadians: {firstAngleInRadians}  secondAngleInRadians: {secondAngleInRadians}  angleInDegrees: {angleInDegrees}*");

            return angleInDegrees;
        }
    }
}
