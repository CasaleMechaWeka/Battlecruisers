using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
	public class LeadingAngleCalculator : AngleCalculator
	{
		/// <summary>
		/// Assumes shells are NOT affected by gravity
		/// </summary>
		protected override float EstimateTimeToTarget(Vector2 source, Vector2 target, float projectileVelocityInMPerS, float currentAngleInDegrees)
		{
			float distance = Vector2.Distance(source, target);
			return distance / projectileVelocityInMPerS;
		}
	}
}
