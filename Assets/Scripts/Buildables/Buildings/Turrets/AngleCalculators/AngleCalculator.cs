using BattleCruisers.Movement.Predictors;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
	public interface IAngleCalculator
	{
		// Cannot use both namespaces and optional parameters in MonoBehaviour scripts :D
		// Otherwise I would use optional parameters for the last two parameters.
		// https://forum.unity3d.com/threads/script-can-use-namespace-or-optional-parameters-but-not-both.164563/
		float FindDesiredAngle(Vector2 source, ITarget target, float currentAngleInRadians);
		float FindDirectionMultiplier(float currentAngleInRadians, float desiredAngleInDegrees);
	}

	public class AngleCalculator : IAngleCalculator
	{
		protected readonly float _projectileVelocityInMPerS;
		private readonly bool _isSourceMirrored;
		protected readonly ITargetPositionPredictorFactory _targetPositionPredictorFactory;
		protected ITargetPositionPredictor _targetPositionPredictor;

		protected virtual bool LeadsTarget { get { return false; } }
		protected virtual bool MustFaceTarget { get { return false; } }

		public AngleCalculator(float projectileVelocityInMPerS, bool isSourceMirrored, ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			_projectileVelocityInMPerS = projectileVelocityInMPerS;
			_isSourceMirrored = isSourceMirrored;
			_targetPositionPredictorFactory = targetPositionPredictorFactory;
		}

		// FELIX  Use FacingDirection instead of isSourceMirrored param?
		public float FindDesiredAngle(Vector2 source, ITarget target, float currentAngleInRadians)
		{
			Vector2 targetPosition = target.Position;

			if (MustFaceTarget)
			{
				CheckSourceIsFacingTarget(source, targetPosition);
			}

			if (LeadsTarget)
			{
				Assert.IsNotNull(_targetPositionPredictor);
				targetPosition = _targetPositionPredictor.PredictTargetPosition(source, target, _projectileVelocityInMPerS, currentAngleInRadians);
			}

			return CalculateDesiredAngle(source, targetPosition);
		}

		private void CheckSourceIsFacingTarget(Vector2 source, Vector2 target)
		{
			if (_isSourceMirrored && target.x >= source.x)
			{
				throw new ArgumentException("Source faces left, but target is to the right");
			}

			if (!_isSourceMirrored && target.x <= source.x)
			{
				throw new ArgumentException("Source faces right, but target is to the left");
			}
		}

		/// <summary>
		/// Assumes shells are NOT affected by gravity
		/// </summary>
		protected virtual float CalculateDesiredAngle(Vector2 source, Vector2 targetPosition)
		{
			if (source == targetPosition)
			{
				throw new ArgumentException();
			}

			float desiredAngleInDegrees;

			if (source.x == targetPosition.x)
			{
				// On same x-axis
				desiredAngleInDegrees = source.y < targetPosition.y ? 90 : 270;
			}
			else if (source.y == targetPosition.y)
			{
				// On same y-axis
				if (source.x < targetPosition.x)
				{
					desiredAngleInDegrees = _isSourceMirrored ? 180 : 0;
				}
				else
				{
					desiredAngleInDegrees = _isSourceMirrored ? 0 : 180;
				}
			}
			else
			{
				// Different x and y axes, so need to calculate the angle
				float xDiff = Math.Abs(source.x - targetPosition.x);
				float yDiff = Math.Abs(source.y - targetPosition.y);
				float angleInDegrees = Mathf.Atan(yDiff / xDiff) * Mathf.Rad2Deg;
				Logging.Log(Tags.ANGLE_CALCULATORS, "angleInDegrees: " + angleInDegrees);

				if (source.x < targetPosition.x)
				{
					// Source is to left of target
					if (source.y < targetPosition.y)
					{
						// Source is below target
						desiredAngleInDegrees = _isSourceMirrored ? 180 - angleInDegrees : angleInDegrees;
					}
					else
					{
						// Source is above target
						desiredAngleInDegrees = _isSourceMirrored ? 180 + angleInDegrees : 360 - angleInDegrees;
					}
				}
				else
				{
					// Source is to right of target
					if (source.y < targetPosition.y)
					{
						// Source is below target
						desiredAngleInDegrees = _isSourceMirrored ? angleInDegrees : 180 - angleInDegrees;
					}
					else
					{
						// Source is above target
						desiredAngleInDegrees = _isSourceMirrored ? 360 - angleInDegrees : 180 + angleInDegrees;
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
