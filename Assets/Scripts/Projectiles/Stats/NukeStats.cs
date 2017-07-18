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
	public class NukeStats : ProjectileStats
	{
		public NukeController NukePrefab { get; private set; }
		public float CruisingAltitudeInM { get; private set; }
		public Vector2 InitialVelocityInMPerS { get; private set; }

		public NukeStats(NukeController nukePrefab, float damage, float maxVelocityInMPerS, float cruisingAltitudeInM)
			: base(damage, maxVelocityInMPerS, ignoreGravity: true)
		{
			NukePrefab = nukePrefab;
			CruisingAltitudeInM = cruisingAltitudeInM;
			InitialVelocityInMPerS = new Vector2(0, 0);
		}
	}
}
