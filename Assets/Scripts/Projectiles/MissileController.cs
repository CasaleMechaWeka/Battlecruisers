using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Buildables;
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
		private Vector2 _velocity;

		public Rigidbody2D rigidBody;

		private const float VELOCITY_EQUALITY_MARGIN = 0.1f;
		private const float VELOCITY_SMOOTH_TIME = 1;

		public void Initialise(ITarget target, ITargetFilter targetFilter, MissileStats missileStats, Vector2 initialVelocityInMPerS)
		{
			_target = target;
			_targetFilter = targetFilter;
			_missileStats = missileStats;
			rigidBody.velocity = initialVelocityInMPerS;

			_target.Destroyed += Target_Destroyed;
		}

		void FixedUpdated()
		{
			AdjustVelocity();

			// Adjust game object to point in direction it's travelling
			transform.right = rigidBody.velocity;
		}

		// FELIX  Avoid massive duplicate functionality with FighterController
		private void AdjustVelocity()
		{
			Vector2 sourcePosition = transform.position;
			Vector2 targetPosition = _target.GameObject.transform.position;

			Vector2 desiredVelocity = FindDesiredVelocity(sourcePosition, targetPosition, _missileStats.MaxVelocityInMPerS);

			if (Math.Abs(rigidBody.velocity.x - desiredVelocity.x) <= VELOCITY_EQUALITY_MARGIN
				&& Math.Abs(rigidBody.velocity.y - desiredVelocity.y) <= VELOCITY_EQUALITY_MARGIN)
			{
				rigidBody.velocity = desiredVelocity;
			}
			else
			{
				Logging.Log(Tags.AIRCRAFT, string.Format("AdjustVelocity():  rigidBody.velocity: {0}  desiredVelocity: {1}  _velocitySmoothTime: {2}  maxVelocityInMPerS: {3}", 
					rigidBody.velocity, desiredVelocity, VELOCITY_SMOOTH_TIME, _missileStats.MaxVelocityInMPerS));

				rigidBody.velocity = Vector2.SmoothDamp(rigidBody.velocity, desiredVelocity, ref _velocity, VELOCITY_SMOOTH_TIME, _missileStats.MaxVelocityInMPerS, Time.deltaTime);
			}
		}

		private Vector2 FindDesiredVelocity(Vector2 sourcePosition, Vector2 targetPosition, float maxVelocityInMPerS)
		{
			Vector2 desiredVelocity = new Vector2(0, 0);

			if (sourcePosition == targetPosition)
			{
				return desiredVelocity;
			}

			if (sourcePosition.x == targetPosition.x)
			{
				// On same x-axis
				desiredVelocity.y = sourcePosition.y < targetPosition.y ? maxVelocityInMPerS : -maxVelocityInMPerS;
			}
			else if (sourcePosition.y == targetPosition.y)
			{
				// On same y-axis
				desiredVelocity.x = sourcePosition.x < targetPosition.x ? maxVelocityInMPerS : -maxVelocityInMPerS;
			}
			else
			{
				// Different x and y axes, so need to calculate the angle
				float xDiff = Math.Abs(sourcePosition.x - targetPosition.x);
				float yDiff = Math.Abs(sourcePosition.y - targetPosition.y);
				float angleInRadians = Mathf.Atan(yDiff / xDiff);
				float angleInDegrees = angleInRadians * Mathf.Rad2Deg;

				float velocityX = Mathf.Cos(angleInRadians) * maxVelocityInMPerS;
				float velocityY = Mathf.Sin(angleInRadians) * maxVelocityInMPerS;
				Logging.Log(Tags.AIRCRAFT, string.Format("FighterController.FindDesiredVelocity()  angleInDegrees: {0}  velocityX: {1}  velocityY: {2}",
					angleInDegrees, velocityX, velocityY));

				if (sourcePosition.x > targetPosition.x)
				{
					// Source is to right of target
					velocityX *= -1;
				}

				if (sourcePosition.y > targetPosition.y)
				{
					// Source is above target
					velocityY *= -1;
				}

				desiredVelocity.x = velocityX;
				desiredVelocity.y = velocityY;
			}

			Logging.Log(Tags.AIRCRAFT, "FighterController.FindDesiredVelocity() " + desiredVelocity);
			return desiredVelocity;
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
			_target.Destroyed -= Target_Destroyed;
			Destroy(gameObject);
		}
	}
}