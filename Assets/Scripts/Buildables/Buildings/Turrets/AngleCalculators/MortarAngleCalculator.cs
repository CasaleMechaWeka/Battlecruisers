using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
	/// <summary>
	/// Assumes:
	/// 1. Shells ARE affected by gravity
	/// 2. Target is in facing direction of source
	/// </summary>
	public class MortarAngleCalculator : AngleCalculator
	{
		private const float MAX_ANGLE_IN_DEGREES = 85;

		protected override bool LeadsTarget { get { return true; } }
		protected override bool MustFaceTarget { get { return true; } }

		protected override float CalculateDesiredAngle(Vector2 source, Vector2 target, bool isSourceMirrored, float projectileVelocityInMPerS, Vector2 targetVelocity)
		{
			float distanceInM = Math.Abs(source.x - target.x);
			float targetAltitude = target.y - source.y;

			float velocitySquared = projectileVelocityInMPerS * projectileVelocityInMPerS;
			float squareRootArg = (velocitySquared * velocitySquared) - Constants.GRAVITY * ((Constants.GRAVITY * distanceInM * distanceInM) + (2 * targetAltitude * velocitySquared));

			if (squareRootArg < 0)
			{
				throw new ArgumentException("Out of range :/");
			}

			float denominator = Constants.GRAVITY * distanceInM;
			float firstAngleInRadians = Mathf.Atan((velocitySquared + Mathf.Sqrt(squareRootArg)) / denominator);
			float secondAngleInRadians = Mathf.Atan((velocitySquared - Mathf.Sqrt(squareRootArg)) / denominator);

			// Choose larger angle, because we want the mortar to fire high instead of flat
			float angleInRadians = Mathf.Max(firstAngleInRadians, secondAngleInRadians);
			float angleInDegrees = angleInRadians * Mathf.Rad2Deg;

			if (angleInDegrees > MAX_ANGLE_IN_DEGREES)
			{
				angleInDegrees = MAX_ANGLE_IN_DEGREES;
			}

			Logging.Log(Tags.ANGLE_CALCULATORS, "MortarAngleCalculator.FindDesiredAngle() " + angleInDegrees + "*");

			return angleInDegrees;
		}

		protected override float EstimateTimeToTarget(Vector2 source, Vector2 target, float projectileVelocityInMPerS, float currentAngleInRadians)
		{
			float sourceElevationInM = source.y - target.y;
			float vSin = projectileVelocityInMPerS * Mathf.Sin(currentAngleInRadians);
			float squareRootArg = (vSin * vSin) + 2 * Constants.GRAVITY * sourceElevationInM;
			return (vSin + Mathf.Sqrt(squareRootArg)) / Constants.GRAVITY;
		}
	}
}
