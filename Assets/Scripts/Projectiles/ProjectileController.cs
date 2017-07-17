using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity;
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
	public class ProjectileController : MonoBehaviour
	{
		private IProjectileStats _projectileStats;
		private ITargetFilter _targetFilter;

		protected Rigidbody2D _rigidBody;
		protected IMovementController _movementController;

		public void Initialise(IProjectileStats projectileStats, Vector2 velocityInMPerS, ITargetFilter targetFilter)
		{
			_rigidBody = gameObject.GetComponent<Rigidbody2D>();
			Assert.IsNotNull(_rigidBody);

			_projectileStats = projectileStats;
			_targetFilter = targetFilter;
			_rigidBody.velocity = velocityInMPerS;
			_rigidBody.gravityScale = _projectileStats.IgnoreGravity ? 0 : 1;
		}

		void FixedUpdate()
		{
			if (_movementController != null)
			{
				_movementController.AdjustVelocity();
				
				// Adjust game object to point in direction it's travelling
				transform.right = _rigidBody.velocity;
			}
		}

		// FELIX  Area of effect damage?
		void OnTriggerEnter2D(Collider2D collider)
		{
			Logging.Log(Tags.SHELLS, "ProjectileController.OnTriggerEnter2D()");

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