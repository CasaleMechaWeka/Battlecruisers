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
	public class MissileStats : ProjectileStats
	{
		private const float INITIAL_VELOCITY_MULTIPLIER = 0.5f;

		public MissileController MissilePrefab { get; private set; }
		public float InitialVelocityInMPerS { get { return MaxVelocityInMPerS * INITIAL_VELOCITY_MULTIPLIER; } }

		public MissileStats(MissileController missilePrefab, float damage, float maxVelocityInMPerS)
			: base(damage, maxVelocityInMPerS, ignoreGravity: true)
		{
			MissilePrefab = missilePrefab;
		}
	}
}
