using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	/// <summary>
	/// Assumes:
	/// 1. Shells ARE affected by gravity
	/// 2. Targets do not move
	/// </summary>
	public class ArtilleryAngleCalculator : AngleCalculator
	{
		/// <summary>
		/// Assumes no y axis difference in source and target
		/// </summary>
		public override float FindDesiredAngle(Vector2 source, Vector2 target, bool isSourceMirrored, float projectileVelocityInMPerS)
		{
			// FELIX  Limit target to valid quarter/half/direction

			float distanceInM = Math.Abs(source.x - target.x);
			if (distanceInM > FindMaxRange(projectileVelocityInMPerS))
			{
				throw new InvalidProgramException();
			}

			float angleInRadians = (float) (0.5 * Math.Asin(Constants.GRAVITY * distanceInM / (projectileVelocityInMPerS * projectileVelocityInMPerS)));
			float angleInDegrees = angleInRadians * Mathf.Rad2Deg;

			Logging.Log(Tags.TURRET_BARREL_CONTROLLER, $"ArtilleryAngleCalculator.FindDesiredAngle() {angleInDegrees}*");

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
