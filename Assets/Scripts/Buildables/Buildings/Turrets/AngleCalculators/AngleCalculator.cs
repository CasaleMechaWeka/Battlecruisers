using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
	public interface IAngleCalculator
	{
		// Cannot use both namespaces and optional parameters in MonoBehaviour scripts :D
		// Otherwise I would use optional parameters for the last two parameters.
		// https://forum.unity3d.com/threads/script-can-use-namespace-or-optional-parameters-but-not-both.164563/
		float FindDesiredAngle(Vector2 source, Vector2 target, bool isSourceMirrored, float projectileVelocityInMPerS, Vector2 targetVelocity, float currentAngleInRadians);
		float FindDirectionMultiplier(float currentAngleInRadians, float desiredAngleInDegrees);
	}

	public class AngleCalculator : MonoBehaviour, IAngleCalculator
	{
		public virtual bool LeadsTarget { get { return false; } }

		// FELIX  Use FacingDirection instead of isSourceMirrored param?
		public float FindDesiredAngle(Vector2 source, Vector2 target, bool isSourceMirrored, float projectileVelocityInMPerS, Vector2 targetVelocity, float currentAngleInRadians)
		{
			if (LeadsTarget)
			{
				target = PredictTargetPosition(source, target, projectileVelocityInMPerS, targetVelocity, currentAngleInRadians);
			}

			return CalculateDesiredAngle(source, target, isSourceMirrored, projectileVelocityInMPerS, targetVelocity);
		}

		private Vector2 PredictTargetPosition(Vector2 source, Vector2 target, float projectileVelocityInMPerS, Vector2 targetVelocity, float currentAngleInRadians)
		{
			float distance = Vector2.Distance(source, target);
			float timeToTargetEstimate = EstimateTimeToTarget(source, target, projectileVelocityInMPerS, currentAngleInRadians);

			float projectedX = target.x + targetVelocity.x * timeToTargetEstimate;
			float projectedY = target.y + targetVelocity.y * timeToTargetEstimate;

			Vector2 projectedPosition = new Vector2(projectedX, projectedY);
			Logging.Log(Tags.ANGLE_CALCULATORS, string.Format("target: {0}  projectedPosition: {1}  targetVelocity: {2}  timeToTargetEstimate: {3}", target, projectedPosition, targetVelocity, timeToTargetEstimate));
			return projectedPosition;
		}

		protected virtual float EstimateTimeToTarget(Vector2 source, Vector2 target, float projectileVelocityInMPerS, float currentAngleInRadians)
		{
			return 0;
		}

		/// <summary>
		/// Assumes shells are NOT affected by gravity
		/// </summary>
		protected virtual float CalculateDesiredAngle(Vector2 source, Vector2 target, bool isSourceMirrored, float projectileVelocityInMPerS, Vector2 targetVelocity)
		{
			if (source == target)
			{
				throw new ArgumentException();
			}

			float desiredAngleInDegrees;

			if (source.x == target.x)
			{
				// On same x-axis
				desiredAngleInDegrees = source.y < target.y ? 90 : 270;
			}
			else if (source.y == target.y)
			{
				// On same y-axis
				if (source.x < target.x)
				{
					desiredAngleInDegrees = isSourceMirrored ? 180 : 0;
				}
				else
				{
					desiredAngleInDegrees = isSourceMirrored ? 0 : 180;
				}
			}
			else
			{
				// Different x and y axes, so need to calculate the angle
				float xDiff = Math.Abs(source.x - target.x);
				float yDiff = Math.Abs(source.y - target.y);
				float angleInDegrees = Mathf.Atan(yDiff / xDiff) * Mathf.Rad2Deg;
				Logging.Log(Tags.ANGLE_CALCULATORS, "angleInDegrees: " + angleInDegrees);

				if (source.x < target.x)
				{
					// Source is to left of target
					if (source.y < target.y)
					{
						// Source is below target
						desiredAngleInDegrees = isSourceMirrored ? 180 - angleInDegrees : angleInDegrees;
					}
					else
					{
						// Source is above target
						desiredAngleInDegrees = isSourceMirrored ? 180 + angleInDegrees : 360 - angleInDegrees;
					}
				}
				else
				{
					// Source is to right of target
					if (source.y < target.y)
					{
						// Source is below target
						desiredAngleInDegrees = isSourceMirrored ? angleInDegrees : 180 - angleInDegrees;
					}
					else
					{
						// Source is above target
						desiredAngleInDegrees = isSourceMirrored ? 360 - angleInDegrees : 180 + angleInDegrees;
					}
				}
			}

			Logging.Log(Tags.ANGLE_CALCULATORS, "AngleCalculator.FindDesiredAngle() " + desiredAngleInDegrees + "*");
			return desiredAngleInDegrees;
		}

		/// <returns>
		/// 1 if it is shorter to rotate anti-clockwise, -1 if it is shorter to rotate clockwise, 
		/// 0 if the desired angle is the same as the current angle.
		/// </returns>
		public virtual float FindDirectionMultiplier(float currentAngleInRadians, float desiredAngleInDegrees)
		{
			if (currentAngleInRadians == desiredAngleInDegrees)
			{
				return 0;
			}

			float distance = Math.Abs(currentAngleInRadians - desiredAngleInDegrees);

			if (desiredAngleInDegrees > currentAngleInRadians)
			{
				return distance < 180 ? 1 : -1;
			}
			else
			{
				return distance < 180 ? -1 : 1;
			}
		}
	}
}
