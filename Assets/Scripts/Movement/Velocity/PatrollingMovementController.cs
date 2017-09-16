using System.Collections.Generic;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Movement.Velocity
{
    public class PatrollingMovementController : MovementController
	{
		private readonly Rigidbody2D _rigidBody;
        // FELIX  Move to parent class?
        private readonly IVelocityProvider _maxVelocityProvider;
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

        public PatrollingMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, IList<IPatrolPoint> patrolPoints)
		{
			Assert.IsNotNull(rigidBody);
            Assert.IsTrue(maxVelocityProvider.VelocityInMPerS > 0);
			Assert.IsTrue(patrolPoints.Count >= MIN_NUM_OF_PATROL_POINTS);

			_rigidBody = rigidBody;
			_maxVelocityProvider = maxVelocityProvider;
			_patrolPoints = patrolPoints;

			_targetPatrolPoint = patrolPoints[0];
		}

		public override void AdjustVelocity()
		{
            Assert.AreEqual(new Vector2(0, 0), _rigidBody.velocity, 
                "Patrolling directly manipulates the game object's position.  If the rigidbody has a non-zero veolcity this seriously messes with things (as I found out :P)");

			bool isInPosition = Vector2.Distance(_rigidBody.transform.position, _targetPatrolPoint.Position) <= POSITION_EQUALITY_MARGIN;
			if (!isInPosition)
			{
				Vector2 oldPatrollingVelocity = _patrollingVelocity;

                Vector2 moveToPosition 
                    = Vector2.SmoothDamp(
                        _rigidBody.transform.position, 
                        _targetPatrolPoint.Position, 
                        ref _patrollingVelocity, 
                        DEFAULT_SMOOTH_TIME_IN_S, 
                        _maxVelocityProvider.VelocityInMPerS, 
                        Time.deltaTime);
                
				_rigidBody.MovePosition(moveToPosition);

                Logging.Log(Tags.MOVEMENT, 
                    string.Format("Patrol():  moveToPosition: {0}  targetPosition: {1}  _patrollingVelocity: {2}  _patrollingVelocity.magnitude: {3}  PatrollingVelocity: {4}  _patrollingSmoothTime: {5}  Time.deltaTime: {6}",
					moveToPosition, _targetPatrolPoint, _patrollingVelocity, _patrollingVelocity.magnitude, _maxVelocityProvider, DEFAULT_SMOOTH_TIME_IN_S, Time.deltaTime));

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

        public override void Activate()
        {
			// Patrolling directly manipulates the game object's position.  If the rigidbody has a 
            // non-zero veolcity this seriously messes with things.  Hence, ensure the rigidbody has
            // a zero velocity :)
			_rigidBody.velocity = new Vector2(0, 0);
        }
	}
}
