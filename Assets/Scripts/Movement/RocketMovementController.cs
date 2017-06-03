using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Movement
{
	// FELIX  Create test case!

	/// <summary>
	/// 1. Moves above source to cruising altitude
	/// 2. Moved horizontally towards target
	/// 3. Once above target drops down to hit target
	/// </summary>
	public class RocketMovementController : HomingMovementController
	{
		private readonly float _cruisingAltitudeInM;
		private readonly float _cruisingAltitidueMarginInM;

		private Queue<Vector2> _targetPoints;
		private Vector2 _currentTargetPoint;

		private const float CRUISING_POINTS_OFFSET_PROPORTION = 0.1f;
		private const float CRUISING_ALTITUDE_MARGIN_PROPORTION = 0.2f;
		private const float MIN_HORIZONTAL_DISTANCE_IN_M = 10;

		public RocketMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, float cruisingAltitudeInM)
			: base(rigidBody, maxVelocityInMPerS) 
		{ 
			Assert.IsTrue(cruisingAltitudeInM > rigidBody.position.y);

			_cruisingAltitudeInM = cruisingAltitudeInM;
			_cruisingAltitidueMarginInM = _cruisingAltitudeInM * CRUISING_ALTITUDE_MARGIN_PROPORTION;
		}

		/// <summary>
		/// Determine ascent and descent points, and set current target point as the ascent point.
		/// </summary>
		protected override void OnTargetSet()
		{
			Assert.IsNull(_targetPoints, "OnTargetSet() called more than once :(");
			Assert.IsNotNull(Target);

			_targetPoints = new Queue<Vector2>();

			float horizontalDistanceToTarget = Mathf.Abs(_rigidBody.position.x - Target.Position.x);
			Assert.IsTrue(horizontalDistanceToTarget >= MIN_HORIZONTAL_DISTANCE_IN_M);

			float cruisingPointsXOffset = CRUISING_POINTS_OFFSET_PROPORTION * horizontalDistanceToTarget;

			if (_rigidBody.position.x < Target.Position.x)
			{
				_targetPoints.Enqueue(new Vector2(_rigidBody.position.x + cruisingPointsXOffset, _cruisingAltitudeInM));
				_targetPoints.Enqueue(new Vector2(Target.Position.x - cruisingPointsXOffset, _cruisingAltitudeInM));
			}
			else
			{
				_targetPoints.Enqueue(new Vector2(_rigidBody.position.x - cruisingPointsXOffset, _cruisingAltitudeInM));
				_targetPoints.Enqueue(new Vector2(Target.Position.x + cruisingPointsXOffset, _cruisingAltitudeInM));
			}

			_targetPoints.Enqueue(Target.Position);

			_currentTargetPoint = _targetPoints.Dequeue();
		}

		protected override Vector2 FindTargetPosition()
		{
			Assert.IsNotNull(_targetPoints, "FindTargetPosition() called before OnTargetSet() :(");

			float distanceFromCurrentTargetPoint = Vector2.Distance(_rigidBody.position, _currentTargetPoint);
			if (distanceFromCurrentTargetPoint <= _cruisingAltitidueMarginInM
				&& _targetPoints.Count != 0)
			{
				_currentTargetPoint = _targetPoints.Dequeue();
			}

			return _currentTargetPoint;
		}
	}
}

