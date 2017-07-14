using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Movement.Velocity
{
	public class PatrollingMovementController : MovementController
	{
		private readonly Rigidbody2D _rigidBody;
		private readonly float _maxPatrollilngVelocityInMPerS;
		private readonly IList<IPatrolPoint> _patrolPoints;

		private Vector2 _patrollingVelocity;
		private IPatrolPoint _targetPatrolPoint;

		private const float POSITION_EQUALITY_MARGIN = 0.1f;
		private const float DEFAULT_SMOOTH_TIME_IN_S = 1;
		private const float MIN_NUM_OF_PATROL_POINTS = 2;

		public override Vector2 Velocity
		{
			get { return _patrollingVelocity; }
			set { _patrollingVelocity = value; }
		}

		public PatrollingMovementController(Rigidbody2D rigidBody, float maxPatrollilngVelocityInMPerS, IList<IPatrolPoint> patrolPoints)
		{
			Assert.IsNotNull(rigidBody);
			Assert.IsTrue(maxPatrollilngVelocityInMPerS > 0);
			Assert.IsTrue(patrolPoints.Count >= MIN_NUM_OF_PATROL_POINTS);

			_rigidBody = rigidBody;
			_maxPatrollilngVelocityInMPerS = maxPatrollilngVelocityInMPerS;
			_patrolPoints = patrolPoints;

			_targetPatrolPoint = FindNearestPatrolPoint();
		}

		public override void AdjustVelocity()
		{
			Assert.AreEqual(new Vector2(0, 0), _rigidBody.velocity, "Patrolling directly manipulates the game object's position.  If the rigidbody has a non-zero veolcity this seriously messes with things (as I found out :P");

			bool isInPosition = Vector2.Distance(_rigidBody.transform.position, _targetPatrolPoint.Position) <= POSITION_EQUALITY_MARGIN;
			if (!isInPosition)
			{
				Vector2 oldPatrollingVelocity = _patrollingVelocity;

				Vector2 moveToPosition = Vector2.SmoothDamp(_rigidBody.transform.position, _targetPatrolPoint.Position, ref _patrollingVelocity, DEFAULT_SMOOTH_TIME_IN_S, _maxPatrollilngVelocityInMPerS, Time.deltaTime);
				_rigidBody.MovePosition(moveToPosition);

				Logging.Log(Tags.AIRCRAFT, string.Format("Patrol():  moveToPosition: {0}  targetPosition: {1}  _patrollingVelocity: {2}  _patrollingVelocity.magnitude: {3}  PatrollingVelocity: {4}  _patrollingSmoothTime: {5}  Time.deltaTime: {6}",
					moveToPosition, _targetPatrolPoint, _patrollingVelocity, _patrollingVelocity.magnitude, _maxPatrollilngVelocityInMPerS, DEFAULT_SMOOTH_TIME_IN_S, Time.deltaTime));

				HandleDirectionChange(oldPatrollingVelocity, _patrollingVelocity);
			}
			else
			{
				OnPatrolPointReached();
			}
		}

		private void OnPatrolPointReached()
		{
			IPatrolPoint reachedPatrolPoint = _targetPatrolPoint;
			_targetPatrolPoint = FindNextPatrolPoint();

			if (reachedPatrolPoint.RemoveOnceReached)
			{
				_patrolPoints.Remove(reachedPatrolPoint);
			}

			reachedPatrolPoint.ActionOnReached.Invoke();
		}

		// FELIX  Use LINQ!
		private IPatrolPoint FindNearestPatrolPoint()
		{
			float minDistance = float.MaxValue;
			IPatrolPoint closestPatrolPoint = null;

			foreach (IPatrolPoint patrolPoint in _patrolPoints)
			{
				float distance = Vector2.Distance(_rigidBody.transform.position, patrolPoint.Position);
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
		private IPatrolPoint FindNextPatrolPoint()
		{
			int currentIndex = _patrolPoints.IndexOf(_targetPatrolPoint);
			Assert.IsTrue(currentIndex != -1);
			int nextIndex = currentIndex == _patrolPoints.Count - 1 ? 0 : currentIndex + 1;
			return _patrolPoints[nextIndex];
		}
	}
}
