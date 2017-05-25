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
	/// FELIX  Handle moving targets!
	/// 2. Targets do not move
	/// </summary>
	public class MortarAngleCalculator : AngleCalculator
	{
		public override float FindDesiredAngle(Vector2 source, Vector2 target, bool isSourceMirrored, float projectileVelocityInMPerS, Vector2 targetVelocity)
		{
			// FELIX  Extract these two checks, common with ArtilleryAngleCalculator
			if (isSourceMirrored && target.x >= source.x)
			{
				throw new ArgumentException("Source faces left, but target is to the right");
			}

			if (!isSourceMirrored && target.x <= source.x)
			{
				throw new ArgumentException("Source faces right, but target is to the left");
			}

			// FELIX  Lead target

			float distanceInM = Math.Abs(source.x - target.x);
			float sourceAltitude = source.y - target.y;

			float velocitySquared = projectileVelocityInMPerS * projectileVelocityInMPerS;
			float squareRootArg = (velocitySquared * velocitySquared) - Constants.GRAVITY * ((Constants.GRAVITY * distanceInM * distanceInM) + (2 * sourceAltitude * velocitySquared));

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

			Logging.Log(Tags.ANGLE_CALCULATORS, "MortarAngleCalculator.FindDesiredAngle() " + angleInDegrees + "*");

			return angleInDegrees;
		}
	}
}
