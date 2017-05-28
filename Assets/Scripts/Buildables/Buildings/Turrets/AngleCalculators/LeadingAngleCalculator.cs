using BattleCruisers.Movement.Predictors;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
	public class LeadingAngleCalculator : AngleCalculator
	{
		protected override bool LeadsTarget { get { return true; } }

		public LeadingAngleCalculator(float projectileVelocityInMPerS, bool isSourceMirrored, ITargetPositionPredictorFactory targetPositionPredictorFactory)
			: base(projectileVelocityInMPerS, isSourceMirrored, targetPositionPredictorFactory) 
		{ 
			_targetPositionPredictor = _targetPositionPredictorFactory.CreateLinearPredictor();
		}
	}
}
