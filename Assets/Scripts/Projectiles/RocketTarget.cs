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
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles
{
	public class RocketTarget : Target
	{
		private Rigidbody2D _rigidBody;

		public override TargetType TargetType { get { return TargetType.Rocket; } }
		public override Vector2 Velocity { get { return _rigidBody.velocity; } }

		public void Initialise(Faction faction, Rigidbody2D rigidBody)
		{
			StaticInitialise();

			Faction = faction;
			_rigidBody = rigidBody;
		}

		// All RocketTarget gameObjects are wrapped by a RocketController gameObject.
		// Hence, we need to destroy the parent gameObject.
		protected override void InternalDestroy()
		{
			Assert.IsNotNull(transform.parent);
			Destroy(transform.parent.gameObject);
		}
	}
}