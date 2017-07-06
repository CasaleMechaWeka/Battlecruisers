using BattleCruisers.Movement.Predictors;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
	public class AngleCalculator : IAngleCalculator
	{
		protected readonly ITargetPositionPredictorFactory _targetPositionPredictorFactory;
		protected ITargetPositionPredictor _targetPositionPredictor;

		protected virtual bool LeadsTarget { get { return false; } }
		protected virtual bool MustFaceTarget { get { return false; } }

		public AngleCalculator(ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			_targetPositionPredictorFactory = targetPositionPredictorFactory;
		}

		// FELIX  Use FacingDirection instead of isSourceMirrored param?
		public float FindDesiredAngle(Vector2 source, ITarget target, bool isSourceMirrored, float projectileVelocityInMPerS, float currentAngleInRadians)
		{
			Vector2 targetPosition = target.Position;

			if (MustFaceTarget)
			{
				CheckSourceIsFacingTarget(source, targetPosition, isSourceMirrored);
			}

			if (LeadsTarget)
			{
				Assert.IsNotNull(_targetPositionPredictor);
				targetPosition = _targetPositionPredictor.PredictTargetPosition(source, target, projectileVelocityInMPerS, currentAngleInRadians);
			}

			return CalculateDesiredAngle(source, targetPosition, isSourceMirrored, projectileVelocityInMPerS);
		}

		private void CheckSourceIsFacingTarget(Vector2 source, Vector2 target, bool isSourceMirrored)
		{
			if (isSourceMirrored && target.x >= source.x)
			{
				throw new ArgumentException("Source faces left, but target is to the right");
			}

			if (!isSourceMirrored && target.x <= source.x)
			{
				throw new ArgumentException("Source faces right, but target is to the left");
			}
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
