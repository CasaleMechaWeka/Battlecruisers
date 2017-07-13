using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Movement.Velocity
{
	public class PatrollingMovementController : IMovementController
	{
		private readonly Rigidbody2D _rigidBody;

		// FELIX   Remove unused
		private float _patrollingSmoothTime;
		protected Vector2 _patrollingVelocity;
		protected bool _isPatrolling;
		private Vector2 _lastPatrolPoint;
		private Vector2 _targetPatrolPoint;

		private const float POSITION_EQUALITY_MARGIN = 0.1f;
		private const float SMOOTH_TIME_MULTIPLIER = 2;
		private const float DEFAULT_SMOOTH_TIME_IN_S = 1;

		#region Properties
		private IList<Vector2> _patrolPoints;
		public IList<Vector2> PatrolPoints
		{
			protected get { return _patrolPoints; }
			set
			{
				Assert.IsTrue(value.Count >= 2);
				_patrolPoints = value;
			}
		}

		#endregion Properties

		public PatrollingMovementController(Rigidbody2D rigidBody)
		{
			_rigidBody = rigidBody;
		}

		public void AdjustVelocity()
		{
			bool isInPosition = Vector2.Distance(_rigidBody.transform.position, _targetPatrolPoint) <= POSITION_EQUALITY_MARGIN;
			if (!isInPosition)
			{
				Vector2 oldPatrollingVelocity = _patrollingVelocity;

				Vector2 moveToPosition = Vector2.SmoothDamp(_rigidBody.transform.position, _targetPatrolPoint, ref _patrollingVelocity, _patrollingSmoothTime, PatrollingVelocity, Time.deltaTime);
				_rigidBody.MovePosition(moveToPosition);

				Logging.Log(Tags.AIRCRAFT, string.Format("Patrol():  moveToPosition: {0}  targetPosition: {1}  _patrollingVelocity: {2}  _patrollingVelocity.magnitude: {3}  PatrollingVelocity: {4}  _patrollingSmoothTime: {5}  Time.deltaTime: {6}",
					moveToPosition, _targetPatrolPoint, _patrollingVelocity, _patrollingVelocity.magnitude, PatrollingVelocity, _patrollingSmoothTime, Time.deltaTime));

				UpdateFacingDirection(oldPatrollingVelocity, _patrollingVelocity);
			}
			else
			{
				Logging.Log(Tags.AIRCRAFT, "Patrol():  Reached patrol point " + _targetPatrolPoint);

				Vector2 patrolPointReached = _targetPatrolPoint;
				_targetPatrolPoint = FindNextPatrolPoint();
				OnPatrolPointReached(patrolPointReached);
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

		protected virtual void OnPatrolPointReached(Vector2 patrolPointReached) { }

		public void StartPatrolling()
		{
			Assert.IsTrue(PatrolPoints != null);
			Assert.AreEqual(new Vector2(0, 0), _rigidBody.velocity, "Patrolling directly manipulates the game object's position.  If the rigidbody has a non-zero veolcity this seriously messes with things (as I found out :P");

			if (PatrolPoints.Contains(_lastPatrolPoint))
			{
				// Resume patrolling
				_targetPatrolPoint = _lastPatrolPoint;
			}
			else
			{
				_targetPatrolPoint = FindNearestPatrolPoint();
			}

			_isPatrolling = true;
		}

		public void StopPatrolling()
		{
			_lastPatrolPoint = _targetPatrolPoint;
			_targetPatrolPoint = default(Vector2);
			_isPatrolling = false;
		}

		private Vector2 FindNearestPatrolPoint()
		{
			float minDistance = float.MaxValue;
			Vector2 closestPatrolPoint = default(Vector2);

			foreach (Vector2 patrolPoint in _patrolPoints)
			{
				float distance = Vector2.Distance(_rigidBody.transform.position, patrolPoint);
				if (distance < minDistance)
				{
					minDistance = distance;
					closestPatrolPoint = patrolPoint;
				}
			}

			return closestPatrolPoint;
		}

		/// <summary>
		/// NOTE:  Assumes there are no duplicate patrol points.
		/// </summary>
		private Vector2 FindNextPatrolPoint()
		{
			int currentIndex = _patrolPoints.IndexOf(_targetPatrolPoint);
			Assert.IsTrue(currentIndex != -1);
			int nextIndex = currentIndex == _patrolPoints.Count - 1 ? 0 : currentIndex + 1;
			return _patrolPoints[nextIndex];
		}
	}
}
