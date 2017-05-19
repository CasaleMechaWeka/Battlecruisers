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
		float FindDesiredAngle(Vector2 source, Vector2 target, bool isSourceMirrored, float projectileVelocityInMPerS, Vector2 targetVelocity);
		float FindDirectionMultiplier(float currentAngleInDegrees, float desiredAngleInDegrees);
	}

	public class AngleCalculator : MonoBehaviour, IAngleCalculator
	{
		// FELIX  Use FacingDirection instead of isSourceMirrored param?

		/// <summary>
		/// Assumes:
		/// 1. Shells are NOT affected by gravity
		/// 2. Targets do not move
		/// </summary>
		public virtual float FindDesiredAngle(Vector2 source, Vector2 target, bool isSourceMirrored, float projectileVelocityInMPerS, Vector2 targetVelocity)
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
		public virtual float FindDirectionMultiplier(float currentAngleInDegrees, float desiredAngleInDegrees)
		{
			if (currentAngleInDegrees == desiredAngleInDegrees)
			{
				return 0;
			}

			float distance = Math.Abs(currentAngleInDegrees - desiredAngleInDegrees);

			if (desiredAngleInDegrees > currentAngleInDegrees)
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
