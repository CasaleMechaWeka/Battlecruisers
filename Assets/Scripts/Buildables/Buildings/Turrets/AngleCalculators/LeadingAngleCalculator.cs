using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
	public class LeadingAngleCalculator : AngleCalculator
	{
		/// <summary>
		/// Assumes:
		/// 1. Shells are NOT affected by gravity
		/// 
		/// Does try to compensate for target movement.
		/// </summary>
		public override float FindDesiredAngle(Vector2 source, Vector2 target, bool isSourceMirrored, float projectileVelocityInMPerS, Vector2 targetVelocity)
		{
			Vector2 projectedTargetPosition = PredictTargetPosition(source, target, projectileVelocityInMPerS, targetVelocity);
			return base.FindDesiredAngle(source, projectedTargetPosition, isSourceMirrored, projectileVelocityInMPerS, targetVelocity);
		}

		private Vector2 PredictTargetPosition(Vector2 source, Vector2 target, float projectileVelocityInMPerS, Vector2 targetVelocity)
		{
			float distance = Vector2.Distance(source, target);
			float timeToTargetEstimate = distance / projectileVelocityInMPerS;

			float projectedX = target.x + targetVelocity.x * timeToTargetEstimate;
			float projectedY = target.y + targetVelocity.y * timeToTargetEstimate;
			return new Vector2(projectedX, projectedY);
		}
	}
}
