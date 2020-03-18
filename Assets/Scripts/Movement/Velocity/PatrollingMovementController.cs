using System.Collections.Generic;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Movement.Velocity
{
    public class PatrollingMovementController : MovementController
	{
		private readonly Rigidbody2D _rigidBody;
		private readonly IList<IPatrolPoint> _patrolPoints;
        private readonly float _positionEqualityMarginInM;

		private Vector2 _patrollingVelocity;
		private IPatrolPoint _targetPatrolPoint;

        private const float MIN_POSITION_EQUALITY_MARGIN_IN_M = 0.1f;
		private const float DEFAULT_SMOOTH_TIME_IN_S = 1;
		private const float MIN_NUM_OF_PATROL_POINTS = 2;

		public override Vector2 Velocity
		{
			get { return _patrollingVelocity; }
			set { _patrollingVelocity = value; }
		}

        public PatrollingMovementController(
            Rigidbody2D rigidBody,
            IVelocityProvider maxVelocityProvider,
            IList<IPatrolPoint> patrolPoints,
            float positionEqualityMarginInM)
            : base(maxVelocityProvider)
		{
			Assert.IsNotNull(rigidBody);
			Assert.IsTrue(patrolPoints.Count >= MIN_NUM_OF_PATROL_POINTS);
            Assert.IsTrue(positionEqualityMarginInM >= MIN_POSITION_EQUALITY_MARGIN_IN_M);

			_rigidBody = rigidBody;
			_patrolPoints = patrolPoints;
            _positionEqualityMarginInM = positionEqualityMarginInM;

			_targetPatrolPoint = patrolPoints[0];
		}

		public override void AdjustVelocity()
		{
            Assert.AreEqual(new Vector2(0, 0), _rigidBody.velocity, 
                "Patrolling directly manipulates the game object's position.  If the rigidbody has a non-zero veolcity this seriously messes with things (as I found out :P)");

            bool isInPosition = Vector2.Distance(_rigidBody.transform.position, _targetPatrolPoint.Position) <= _positionEqualityMarginInM;
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
                        _time.DeltaTime);

                // NOTE:  Am not using _rigidBody.MovePosition(), because that reacts weirdly
                // for fighters when the fighter's rigidBody.transform.rotation is being
                // explicitly set (to get the fighter pointing the direction it's travelling).
                _rigidBody.transform.position = moveToPosition;

                // Have this inline so it will be stripped out when logs are excluded.
                Logging.Verbose(
                    Tags.MOVEMENT,
                    $"currentPosition:D {_rigidBody.transform.position}  moveToPosition: {moveToPosition}  targetPosition: {_targetPatrolPoint.Position}  " +
                    $"_patrollingVelocity: {_patrollingVelocity}  _patrollingVelocity.magnitude: {_patrollingVelocity.magnitude}  " +
                    $"PatrollingVelocity: {_maxVelocityProvider.VelocityInMPerS}  _patrollingSmoothTime: {DEFAULT_SMOOTH_TIME_IN_S}  " +
                    $"_time.DeltaTime: {_time.DeltaTime}  _rigidBody.transform.rotation.eulerAngles: {_rigidBody.transform.rotation.eulerAngles}");

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
