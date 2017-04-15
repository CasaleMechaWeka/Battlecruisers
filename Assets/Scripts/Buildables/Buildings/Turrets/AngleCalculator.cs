using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public interface IAngleCalculator
	{
		float FindDesiredAngle(Vector2 source, Vector2 target, bool isSourceMirrored);
	}

	public class AngleCalculator : MonoBehaviour 
	{
		// FELIX  Unit test!
		/// <summary>
		/// Assumes:
		/// 1. Shells are NOT affected by gravity
		/// 2. Targets do not move
		/// </summary>
		public virtual float FindDesiredAngle(Vector2 source, Vector2 target, bool isSourceMirrored, float projectileVelocityInMPerS)
		{
			Assert.AreNotEqual(source, target);

			float desiredAngleInDegrees;

			if (source.x == target.x)
			{
				// On same x-axis
				desiredAngleInDegrees = source.y < target.y ? 90 : -90;
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
	}
}
