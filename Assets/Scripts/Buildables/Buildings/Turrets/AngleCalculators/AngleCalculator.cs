using System;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public class AngleCalculator : IAngleCalculator
	{
		protected virtual bool LeadsTarget { get { return false; } }
		protected virtual bool MustFaceTarget { get { return false; } }

        public float FindDesiredAngle(Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored, float projectileVelocityInMPerS)
		{
            if (MustFaceTarget && !Helper.IsFacingTarget(targetPosition, sourcePosition, isSourceMirrored))
			{
                throw new ArgumentException("Source does not face target :(  source: " + sourcePosition + "  target: " + targetPosition + "  isSourceMirrored: " + isSourceMirrored);
            }

            return CalculateDesiredAngle(sourcePosition, targetPosition, isSourceMirrored, projectileVelocityInMPerS);
        }

		/// <summary>
		/// Assumes shells are NOT affected by gravity
		/// </summary>
		protected virtual float CalculateDesiredAngle(Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored, float projectileVelocityInMPerS)
		{
			if (sourcePosition == targetPosition)
			{
				throw new ArgumentException();
			}

			float desiredAngleInDegrees;

			if (sourcePosition.x == targetPosition.x)
			{
				// On same x-axis
				desiredAngleInDegrees = sourcePosition.y < targetPosition.y ? 90 : 270;
			}
			else if (sourcePosition.y == targetPosition.y)
			{
				// On same y-axis
				if (sourcePosition.x < targetPosition.x)
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
				float xDiff = Math.Abs(sourcePosition.x - targetPosition.x);
				float yDiff = Math.Abs(sourcePosition.y - targetPosition.y);
				float angleInDegrees = Mathf.Atan(yDiff / xDiff) * Mathf.Rad2Deg;
				Logging.Log(Tags.ANGLE_CALCULATORS, "angleInDegrees: " + angleInDegrees);

				if (sourcePosition.x < targetPosition.x)
				{
					// Source is to left of target
					if (sourcePosition.y < targetPosition.y)
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
					if (sourcePosition.y < targetPosition.y)
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
	}
}
