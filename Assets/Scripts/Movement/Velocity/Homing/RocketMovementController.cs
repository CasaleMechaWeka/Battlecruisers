using BattleCruisers.Buildables;
using BattleCruisers.Targets;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Movement.Velocity.Homing
{
	/// <summary>
	/// 1. Moves above source to cruising altitude
	/// 2. Moved horizontally towards target
	/// 3. Once above target drops down to hit target
	/// </summary>
	public class RocketMovementController : HomingMovementController
	{
		private readonly float _cruisingAltitudeInM;
		private readonly float _cruisingAltitidueMarginInM;

		private Queue<Vector2> _flightPoints;
		private Vector2 _currentTargetPoint;

		private const float CRUISING_POINTS_OFFSET_PROPORTION = 0.25f;
		private const float CRUISING_ALTITUDE_MARGIN_PROPORTION = 0.25f;
		private const float MIN_HORIZONTAL_DISTANCE_IN_M = 10;

		public RocketMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, ITargetProvider targetProvider, float cruisingAltitudeInM)
			: base(rigidBody, maxVelocityInMPerS, targetProvider) 
		{ 
			Assert.IsTrue(cruisingAltitudeInM > rigidBody.position.y);

			_cruisingAltitudeInM = cruisingAltitudeInM;
			_cruisingAltitidueMarginInM = _cruisingAltitudeInM * CRUISING_ALTITUDE_MARGIN_PROPORTION;

			CreateFlightPoints();
		}

		/// <summary>
		/// Determine ascent and descent points, and set current target point as the ascent point.
		/// </summary>
		private void CreateFlightPoints()
		{
			Assert.IsNull(_flightPoints, "OnTargetSet() called more than once :(");

			ITarget target = _targetProvider.Target;
			Assert.IsNotNull(target);

			_flightPoints = new Queue<Vector2>();

			float horizontalDistanceToTarget = Mathf.Abs(_rigidBody.position.x - target.Position.x);
			Assert.IsTrue(horizontalDistanceToTarget >= MIN_HORIZONTAL_DISTANCE_IN_M);

			float cruisingPointsXOffset = CRUISING_POINTS_OFFSET_PROPORTION * horizontalDistanceToTarget;

			if (_rigidBody.position.x < target.Position.x)
			{
				_flightPoints.Enqueue(new Vector2(_rigidBody.position.x + cruisingPointsXOffset, _cruisingAltitudeInM));
				_flightPoints.Enqueue(new Vector2(target.Position.x - cruisingPointsXOffset, _cruisingAltitudeInM));
			}
			else
			{
				_flightPoints.Enqueue(new Vector2(_rigidBody.position.x - cruisingPointsXOffset, _cruisingAltitudeInM));
				_flightPoints.Enqueue(new Vector2(target.Position.x + cruisingPointsXOffset, _cruisingAltitudeInM));
			}

			_flightPoints.Enqueue(target.Position);

			_currentTargetPoint = _flightPoints.Dequeue();
		}

		protected override Vector2 FindTargetPosition()
		{
			Assert.IsNotNull(_flightPoints, "FindTargetPosition() called before OnTargetSet() :(");

			float distanceFromCurrentTargetPoint = Vector2.Distance(_rigidBody.position, _currentTargetPoint);
			if (distanceFromCurrentTargetPoint <= _cruisingAltitidueMarginInM
				&& _flightPoints.Count != 0)
			{
				_currentTargetPoint = _flightPoints.Dequeue();
			}

			return _currentTargetPoint;
		}
	}
}

