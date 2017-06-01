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
	public class ProjectileController : MonoBehaviour
	{
		private IProjectileStats _projectileStats;
		private ITargetFilter _targetFilter;

		// FELIX  Find programmatically
		public Rigidbody2D rigidBody;

		public void Initialise(IProjectileStats projectileStats, Vector2 velocityInMPerS, ITargetFilter targetFilter)
		{
			_projectileStats = projectileStats;
			_targetFilter = targetFilter;
			rigidBody.velocity = velocityInMPerS;
			rigidBody.gravityScale = _projectileStats.IgnoreGravity ? 0 : 1;
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			Logging.Log(Tags.SHELLS, "MissileController.OnTriggerEnter2D()");

			ITarget target = collider.gameObject.GetComponent<ITarget>();

			if (target != null && _targetFilter.IsMatch(target))
			{
				target.TakeDamage(_projectileStats.Damage);
				CleanUp();
			}
		}

		protected virtual void CleanUp()
		{
			Destroy(gameObject);
		}
	}
}