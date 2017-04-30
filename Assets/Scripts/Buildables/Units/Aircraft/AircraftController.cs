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
		private Vector2 _patrollingVelocity;
		private bool _isPatrolling;

		private const float POSITION_EQUALITY_MARGIN = 0.1f;
		private const float SMOOTH_TIME_MULTIPLIER = 2;

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
				_patrollingSmoothTime = distance / maxVelocityInMPerS / SMOOTH_TIME_MULTIPLIER;

				Logging.Log(Tags.BOMBER, $"Setting new patrol point {_targetPatrolPoint}");
			}
		}

		public override Vector2 Velocity
		{
			get
			{
				return _isPatrolling ? _patrollingVelocity : base.Velocity;
			}
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			if (_isPatrolling)
			{
				Patrol();
			}
		}

		private void Patrol()
		{
			Vector2 positionAsVector2 = new Vector2(transform.position.x, transform.position.y);
			bool isInPosition = (positionAsVector2 - TargetPatrolPoint).magnitude < POSITION_EQUALITY_MARGIN;
			if (!isInPosition)
			{
				transform.position = Vector2.SmoothDamp(transform.position, TargetPatrolPoint, ref _patrollingVelocity, _patrollingSmoothTime, maxVelocityInMPerS, Time.deltaTime);

				Debug.Log($"_patrollingVelocity: {_patrollingVelocity}  maxVelocityInMPerS: {maxVelocityInMPerS}");
			}
			else
			{
				Logging.Log(Tags.BOMBER, $"OnUpdate():  Reached patrol point {_targetPatrolPoint}");
				TargetPatrolPoint = FindNextPatrolPoint();
			}
		}

		public void StartPatrolling()
		{
			Assert.IsTrue(PatrolPoints != null);
			TargetPatrolPoint = FindNearestPatrolPoint();
			_isPatrolling = true;
		}

		public void StopPatrolling()
		{
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
