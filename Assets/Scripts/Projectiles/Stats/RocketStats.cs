using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Projectiles.Stats
{
	// FELIX  Extract common property with MissileController?
	public class RocketStats : ProjectileStats
	{
		private const float INITIAL_VELOCITY_MULTIPLIER = 0.15f;

		public RocketController RocketPrefab { get; private set; }
		public float CruisingAltitudeInM { get; private set; }
		public float InitialVelocityInMPerS { get { return MaxVelocityInMPerS * INITIAL_VELOCITY_MULTIPLIER; } }

		public RocketStats(RocketController rocketPrefab, float damage, float maxVelocityInMPerS, float cruisingAltitudeInM)
			: base(damage, maxVelocityInMPerS, ignoreGravity: true)
		{
			RocketPrefab = rocketPrefab;
			CruisingAltitudeInM = cruisingAltitudeInM;
		}
	}
}
