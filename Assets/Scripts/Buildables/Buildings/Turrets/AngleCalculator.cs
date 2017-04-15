using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public interface IAngleCalculator
	{
		float FindDesiredAngle(Vector2 source, Vector2 target, bool isSourceMirrored, float projectileVelocityInMPerS);
		float FindDirectionMultiplier(float currentAngleInDegrees, float desiredAngleInDegrees);
	}

	public class AngleCalculator : MonoBehaviour, IAngleCalculator
	{
		/// <summary>
		/// Assumes:
		/// 1. Shells are NOT affected by gravity
		/// 2. Targets do not move
		/// </summary>
		public virtual float FindDesiredAngle(Vector2 source, Vector2 target, bool isSourceMirrored, float projectileVelocityInMPerS)
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
				float xDiff = Math.Abs(source.x - target.x);
				float yDiff = Math.Abs(source.y - target.y);
				float angleInDegrees = Mathf.Atan(yDiff / xDiff) * Mathf.Rad2Deg;

				// Different x and y axes, so need to calculate the angle
				if (source.x < target.y)
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

			Logging.Log(Tags.TURRET_BARREL_CONTROLLER, $"AngleCalculator.FindDesiredAngle() {desiredAngleInDegrees}*");
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
