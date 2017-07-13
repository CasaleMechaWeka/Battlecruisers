using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Movement.Velocity
{
	public class PatrollingMovementController : IMovementController
	{
		private readonly Rigidbody2D _rigidBody;
		private readonly float _maxPatrollilngVelocityInMPerS;
		private readonly IList<Vector2> _patrolPoints;

		private Vector2 _patrollingVelocity;
		private Vector2 _targetPatrolPoint;

		private const float POSITION_EQUALITY_MARGIN = 0.1f;
		private const float DEFAULT_SMOOTH_TIME_IN_S = 1;
		private const float MIN_NUM_OF_PATROL_POINTS = 2;

		public Vector2 Velocity
		{
			get { return _patrollingVelocity; }
			set { _patrollingVelocity = value; }
		}

		public event EventHandler<XDirectionChangeEventArgs> DirectionChanged;

		public PatrollingMovementController(Rigidbody2D rigidBody, float maxPatrollilngVelocityInMPerS, IList<Vector2> patrolPoints)
		{
			Assert.IsNotNull(rigidBody);
			Assert.IsTrue(maxPatrollilngVelocityInMPerS > 0);
			Assert.IsTrue(patrolPoints.Count >= MIN_NUM_OF_PATROL_POINTS);

			_rigidBody = rigidBody;
			_maxPatrollilngVelocityInMPerS = maxPatrollilngVelocityInMPerS;
			_patrolPoints = patrolPoints;

			_targetPatrolPoint = FindNearestPatrolPoint();
		}

		public void AdjustVelocity()
		{
			Assert.AreEqual(new Vector2(0, 0), _rigidBody.velocity, "Patrolling directly manipulates the game object's position.  If the rigidbody has a non-zero veolcity this seriously messes with things (as I found out :P");

			bool isInPosition = Vector2.Distance(_rigidBody.transform.position, _targetPatrolPoint) <= POSITION_EQUALITY_MARGIN;
			if (!isInPosition)
			{
				Vector2 oldPatrollingVelocity = _patrollingVelocity;

				Vector2 moveToPosition = Vector2.SmoothDamp(_rigidBody.transform.position, _targetPatrolPoint, ref _patrollingVelocity, DEFAULT_SMOOTH_TIME_IN_S, _maxPatrollilngVelocityInMPerS, Time.deltaTime);
				_rigidBody.MovePosition(moveToPosition);

				Logging.Log(Tags.AIRCRAFT, string.Format("Patrol():  moveToPosition: {0}  targetPosition: {1}  _patrollingVelocity: {2}  _patrollingVelocity.magnitude: {3}  PatrollingVelocity: {4}  _patrollingSmoothTime: {5}  Time.deltaTime: {6}",
					moveToPosition, _targetPatrolPoint, _patrollingVelocity, _patrollingVelocity.magnitude, _maxPatrollilngVelocityInMPerS, DEFAULT_SMOOTH_TIME_IN_S, Time.deltaTime));

				HandleDirectionChange(oldPatrollingVelocity, _patrollingVelocity);
			}
			else
			{
				_targetPatrolPoint = FindNextPatrolPoint();
			}
		}

		protected void HandleDirectionChange(Vector2 oldVelocity, Vector2 currentVelocity)
		{
			if (DirectionChanged != null)
			{
				Direction? newDirection = null;

				if (oldVelocity.x > 0 && currentVelocity.x < 0)
				{
					newDirection = Direction.Left;
				}
				else if (oldVelocity.x < 0 && currentVelocity.x > 0)
				{
					newDirection = Direction.Right;
				}

				if (newDirection != null)
				{
					DirectionChanged.Invoke(this, new XDirectionChangeEventArgs((Direction)newDirection));
				}
			}
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
