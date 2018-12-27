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
		protected override bool MustFaceTarget { get { return true; } }

        public GravityAffectedAngleCalculator(IAngleHelper angleHelper, IProjectileFlightStats projectileFlightStats) 
            : base(angleHelper, projectileFlightStats)
        {
            _adjustedGravity = Constants.GRAVITY * projectileFlightStats.GravityScale;
        }

		protected override float CalculateDesiredAngle(Vector2 source, Vector2 targetPosition, bool isSourceMirroed)
		{
			float distanceInM = Math.Abs(source.x - targetPosition.x);
			float targetAltitude = targetPosition.y - source.y;

            float velocitySquared = _projectileFlightStats.MaxVelocityInMPerS * _projectileFlightStats.MaxVelocityInMPerS;
			float squareRootArg = (velocitySquared * velocitySquared) - _adjustedGravity * ((_adjustedGravity * distanceInM * distanceInM) + (2 * targetAltitude * velocitySquared));

			if (squareRootArg < 0)
			{
				throw new ArgumentException("Out of range :/  source: " + source + "  target: " + targetPosition);
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
