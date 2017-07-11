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
	public interface IProjectileStats
	{
		float Damage { get; }
		float MaxVelocityInMPerS { get; }
		bool IgnoreGravity { get; }
	}

	public abstract class ProjectileStats : IProjectileStats
	{
		public float Damage { get; private set; }
		public float MaxVelocityInMPerS { get; private set; }
		public bool IgnoreGravity { get; private set; }

		public ProjectileStats(float damage, float maxVelocityInMPerS, bool ignoreGravity)
		{
			Damage = damage;
			MaxVelocityInMPerS = maxVelocityInMPerS;
			IgnoreGravity = ignoreGravity;
		}
	}
}
