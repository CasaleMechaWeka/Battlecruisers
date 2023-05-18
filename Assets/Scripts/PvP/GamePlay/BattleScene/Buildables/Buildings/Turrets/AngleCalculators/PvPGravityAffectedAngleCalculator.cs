using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators
{
    /// <summary>
    /// Assumes:
    /// 1. Shells ARE affected by gravity
    /// 2. Target is in facing direction of source
    /// </summary>
    public abstract class PvPGravityAffectedAngleCalculator : PvPAngleCalculator
    {
        private readonly IPvPAngleConverter _angleConverter;
        private readonly IPvPProjectileFlightStats _projectileFlightStats;
        private readonly float _adjustedGravity;
        private float previousAngle;

        protected abstract bool UseLargerAngle { get; }

        public PvPGravityAffectedAngleCalculator(IPvPAngleHelper angleHelper, IPvPAngleConverter angleConverter, IPvPProjectileFlightStats projectileFlightStats)
            : base(angleHelper)
        {
            PvPHelper.AssertIsNotNull(angleConverter, projectileFlightStats);
            Assert.IsTrue(projectileFlightStats.GravityScale > 0);

            _angleConverter = angleConverter;
            _projectileFlightStats = projectileFlightStats;
            _adjustedGravity = Constants.GRAVITY * projectileFlightStats.GravityScale;
        }

        public override float FindDesiredAngle(Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored)
        {
            if (!PvPHelper.IsFacingTarget(targetPosition, sourcePosition, isSourceMirrored))
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

            float angleInRadians = UseLargerAngle ? Mathf.Max(firstAngleInRadians, secondAngleInRadians) : Mathf.Min(firstAngleInRadians, secondAngleInRadians);
            float angleInDegrees = angleInRadians * Mathf.Rad2Deg;

            angleInDegrees = _angleConverter.ConvertToUnsigned(angleInDegrees);
            previousAngle = angleInDegrees;
            // Logging.Verbose(
            //     Tags.ANGLE_CALCULATORS,
            //     $"source: {sourcePosition}  target: {targetPosition}  isSourceMirrored: {isSourceMirrored}  UseLargerAngle: {UseLargerAngle}  " +
            //     $"firstAngleInRadians: {firstAngleInRadians}  secondAngleInRadians: {secondAngleInRadians}  angleInDegrees: {angleInDegrees}*");

            return angleInDegrees;
        }
    }
}
