using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Projectiles;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Units.Aircraft
{
	public class AircraftController : Unit
	{
		private float _patrollingSmoothTime;
		protected Vector2 _patrollingVelocity;
		protected bool _isPatrolling;
		private Vector2 _lastPatrolPoint;

		private const float POSITION_EQUALITY_MARGIN = 0.1f;
		private const float SMOOTH_TIME_MULTIPLIER = 2;

		#region Properties
		public override TargetType TargetType { get { return TargetType.Aircraft; } }

		private IList<Vector2> _patrolPoints;
		public IList<Vector2> PatrolPoints
		{
			private get { return _patrolPoints; }
			set
			{
				Assert.IsTrue(value.Count >= 2);
				_patrolPoints = value;
			}
		}

		private Vector2 _targetPatrolPoint;
		private Vector2 TargetPatrolPoint
		{
			get { return _targetPatrolPoint; }
			set
			{
				_targetPatrolPoint = value;
				float distance = Vector2.Distance(transform.position, _targetPatrolPoint);
				_patrollingSmoothTime = distance / PatrollingVelocity / SMOOTH_TIME_MULTIPLIER;

				Logging.Log(Tags.AIRCRAFT, $"set_TargetPatrolPoint: {_targetPatrolPoint}  _patrollingSmoothTime: {_patrollingSmoothTime}");
			}
		}

		public override Vector2 Velocity
		{
			get
			{
				return _isPatrolling ? _patrollingVelocity : base.Velocity;
			}
		}

		protected virtual float PatrollingVelocity { get { return maxVelocityInMPerS; } }
		#endregion Properties

		protected override void OnFixedUpdate()
		{
			base.OnFixedUpdate();

			if (_isPatrolling)
			{
				Patrol();
			}
		}

		private void Patrol()
		{
			bool isInPosition = Vector2.Distance(transform.position, TargetPatrolPoint) <= POSITION_EQUALITY_MARGIN;
			if (!isInPosition)
			{
				Vector2 oldPatrollingVelocity = _patrollingVelocity;
				transform.position = Vector2.SmoothDamp(transform.position, TargetPatrolPoint, ref _patrollingVelocity, _patrollingSmoothTime, PatrollingVelocity, Time.deltaTime);

				Logging.Log(Tags.AIRCRAFT, $"Patrol():  currentPosition: {transform.position}  targetPosition: {TargetPatrolPoint}  "
					+ $"_patrollingVelocity: {_patrollingVelocity}  PatrollingVelocity: {PatrollingVelocity}  _patrollingSmoothTime: {_patrollingSmoothTime}  Time.deltaTime: {Time.deltaTime}");

				UpdateFacingDirection(oldPatrollingVelocity, _patrollingVelocity);
			}
			else
			{
				Logging.Log(Tags.AIRCRAFT, $"Patrol():  Reached patrol point {_targetPatrolPoint}");
				TargetPatrolPoint = FindNextPatrolPoint();
			}
		}

		protected void UpdateFacingDirection(Vector2 oldVelocity, Vector2 currentVelocity)
		{
			if (oldVelocity.x > 0 && currentVelocity.x < 0)
			{
				FacingDirection = Direction.Left;
			}
			else if (oldVelocity.x < 0 && currentVelocity.x > 0)
			{
				FacingDirection = Direction.Right;
			}
		}

		public void StartPatrolling()
		{
			Assert.IsTrue(PatrolPoints != null);
			Assert.AreEqual(new Vector2(0, 0), rigidBody.velocity, "Patrolling directly manipulates the game object's position.  If the rigidbody has a non-zero veolcity this seriously messes with things (as I found out :P");

			if (PatrolPoints.Contains(_lastPatrolPoint))
			{
				// Resume patrolling
				TargetPatrolPoint = _lastPatrolPoint;
			}
			else
			{
				TargetPatrolPoint = FindNearestPatrolPoint();
			}

			_isPatrolling = true;
		}

		public void StopPatrolling()
		{
			_lastPatrolPoint = TargetPatrolPoint;
			TargetPatrolPoint = default(Vector2);
			_isPatrolling = false;
		}

		private Vector2 FindNearestPatrolPoint()
		{
			float minDistance = float.MaxValue;
			Vector2 closestPatrolPoint = default(Vector2);

			foreach (Vector2 patrolPoint in _patrolPoints)
			{
				float distance = Vector2.Distance(transform.position, patrolPoint);
				if (distance < minDistance)
				{
					minDistance = distance;
					closestPatrolPoint = patrolPoint;
				}
			}

			return closestPatrolPoint;
		}

		private Vector2 FindNextPatrolPoint()
		{
			int currentIndex = _patrolPoints.IndexOf(TargetPatrolPoint);
			Assert.IsTrue(currentIndex != -1);
			int nextIndex = currentIndex == _patrolPoints.Count - 1 ? 0 : currentIndex + 1;
			return _patrolPoints[nextIndex];
		}
	}
}
