using BattleCruisers.Movement.Predictors;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
	public class StaticAngleCalculator : AngleCalculator
	{
		private readonly float _desiredAngleInDegrees;

		public StaticAngleCalculator(ITargetPositionPredictorFactory targetPositionPredictorFactory, float desiredAngleInDegrees)
			: base(targetPositionPredictorFactory) 
		{ 
			_desiredAngleInDegrees = desiredAngleInDegrees;
		}

		protected override float CalculateDesiredAngle(Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored, float projectileVelocityInMPerS)
		{
			return _desiredAngleInDegrees;
		}
	}
}
