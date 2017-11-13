using System;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    /// <summary>
    /// Assumes:
    /// 1. Shells ARE affected by gravity
    /// 2. Targets do not move
    /// 3. No y axis difference in source and target
    /// 4. Target is in facing direction of source
    /// </summary>
    public class ArtilleryAngleCalculator : AngleCalculator
	{
        public ArtilleryAngleCalculator(IAngleHelper angleHelper) 
            : base(angleHelper)
        {
        }

        protected override bool MustFaceTarget { get { return true; } }

		protected override float CalculateDesiredAngle(Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored, float projectileVelocityInMPerS)
		{
			float distanceInM = Math.Abs(sourcePosition.x - targetPosition.x);
			if (distanceInM > FindMaxRange(projectileVelocityInMPerS))
			{
				throw new ArgumentException("Out of range");
			}

			float angleInRadians = 0.5f * Mathf.Asin(Constants.GRAVITY * distanceInM / (projectileVelocityInMPerS * projectileVelocityInMPerS));
			float angleInDegrees = angleInRadians * Mathf.Rad2Deg;

			Logging.Log(Tags.ANGLE_CALCULATORS, "ArtilleryAngleCalculator.FindDesiredAngle() " + angleInDegrees + "*");

			return angleInDegrees;
		}

		/// <summary>
		/// Assumes no y axis difference in source and target
		/// </summary>
		private float FindMaxRange(float velocityInMPerS)
		{
			return (velocityInMPerS * velocityInMPerS) / Constants.GRAVITY;
		}
	}
}
