using BattleCruisers.Buildables;
using BattleCruisers.Movement;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Projectiles
{
	public class MissileStats
	{
		public float Damage { get; private set; }
		public float MaxVelocityInMPerS { get; private set; }

		public MissileStats(float damage, float maxVelocityInMPerS)
		{
			Damage = damage;
			MaxVelocityInMPerS = maxVelocityInMPerS;
		}
	}

	// FELIX:  Extract common functionality with ShellController
	public class MissileController : MonoBehaviour
	{
		private ITarget _target;
		private ITargetFilter _targetFilter;
		private MissileStats _missileStats;
		private IHomingMovementController _movementController;
		private Vector2 _velocity;

		public Rigidbody2D rigidBody;

		private const float VELOCITY_EQUALITY_MARGIN = 0.1f;
		protected const float MAX_VELOCITY_SMOOTH_TIME = 1;

		public void Initialise(ITarget target, ITargetFilter targetFilter, MissileStats missileStats, Vector2 initialVelocityInMPerS, IMovementControllerFactory movementControllerFactory)
		{
			_target = target;
			_targetFilter = targetFilter;
			_missileStats = missileStats;
			rigidBody.velocity = initialVelocityInMPerS;

			_movementController = movementControllerFactory.CreateMissileMovementController(rigidBody, missileStats.MaxVelocityInMPerS);
			_movementController.Target = _target;

			_target.Destroyed += Target_Destroyed;
		}

		void FixedUpdate()
		{
			_movementController.AdjustVelocity();

			// Adjust game object to point in direction it's travelling
			transform.right = rigidBody.velocity;
		}

		// FELIX  Don't instantly destroy missile, let it go until some maximum range/time
		private void Target_Destroyed(object sender, DestroyedEventArgs e)
		{
			CleanUp();
		}

		void OnTriggerEnter2D(Collider2D collider)
		{
			Logging.Log(Tags.SHELLS, "MissileController.OnTriggerEnter2D()");

			ITarget target = collider.gameObject.GetComponent<ITarget>();

			if (target != null && _targetFilter.IsMatch(target))
			{
				target.TakeDamage(_missileStats.Damage);
				CleanUp();
			}
		}

		private void CleanUp()
		{
			_movementController.Target = null;
			_target.Destroyed -= Target_Destroyed;
			Destroy(gameObject);
		}
	}
}