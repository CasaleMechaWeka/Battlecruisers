using BattleCruisers.Buildables;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Projectiles
{
	/// <summary>
	/// The RocketController wants the behaviour of both:
	/// 1. ProjectileController
	/// 2. Target
	/// But it can only subclass one of these.  Hence subclass ProjectileController, and
	/// have a child game object deriving of Target, to get both behaviours.
	/// </summary>
	public class RocketTarget : Target
	{
		private Rigidbody2D _rigidBody;

		public override TargetType TargetType { get { return TargetType.Rocket; } }
		public override Vector2 Velocity { get { return _rigidBody.velocity; } }

		public void Initialise(Faction faction, Rigidbody2D rigidBody)
		{
			Faction = faction;
			_rigidBody = rigidBody;
		}
	}
}