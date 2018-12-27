using System;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    /// <summary>
    /// Assumes:
    /// 1. Shells ARE affected by gravity
    /// 2. Target is in facing direction of source
    /// </summary>
    public abstract class GravityAffectedAngleCalculator : AngleCalculator
	{
        private readonly float _adjustedGravity;

        protected abstract bool UseLargerAngle { get; }

        public GravityAffectedAngleCalculator(IAngleHelper angleHelper, IProjectileFlightStats projectileFlightStats) 
            : base(angleHelper, projectileFlightStats)
        {
            _adjustedGravity = Constants.GRAVITY * projectileFlightStats.GravityScale;
        }

		public override float FindDesiredAngle(Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored)
		{
            if (!Helper.IsFacingTarget(targetPosition, sourcePosition, isSourceMirrored))
            {
                throw new ArgumentException("Source does not face target :(  source: " + sourcePosition + "  target: " + targetPosition + "  isSourceMirrored: " + isSourceMirrored);
            }

            float distanceInM = Math.Abs(sourcePosition.x - targetPosition.x);
			float targetAltitude = targetPosition.y - sourcePosition.y;

            float velocitySquared = _projectileFlightStats.MaxVelocityInMPerS * _projectileFlightStats.MaxVelocityInMPerS;
			float squareRootArg = (velocitySquared * velocitySquared) - _adjustedGravity * ((_adjustedGravity * distanceInM * distanceInM) + (2 * targetAltitude * velocitySquared));

			if (squareRootArg < 0)
			{
				throw new ArgumentException("Out of range :/  source: " + sourcePosition + "  target: " + targetPosition);
			}

			float denominator = _adjustedGravity * distanceInM;
			float firstAngleInRadians = Mathf.Atan((velocitySquared + Mathf.Sqrt(squareRootArg)) / denominator);
			float secondAngleInRadians = Mathf.Atan((velocitySquared - Mathf.Sqrt(squareRootArg)) / denominator);

            float angleInRadians = UseLargerAngle ? Mathf.Max(firstAngleInRadians, secondAngleInRadians) : Mathf.Min(firstAngleInRadians, secondAngleInRadians);
			float angleInDegrees = angleInRadians * Mathf.Rad2Deg;

            Logging.Log(Tags.ANGLE_CALCULATORS, "GravityAffectedAngleCalculator.FindDesiredAngle() " + angleInDegrees + "*");

			return angleInDegrees;
		}
	}
}
