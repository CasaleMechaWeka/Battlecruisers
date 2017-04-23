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
		private Vector3 _patrollingVelocity;

		private const float POSITION_EQUALITY_MARGIN = 0.1f;
		private const float SMOOTH_TIME_MULTIPLIER = 2;

		private IList<Vector3> _patrolPoints;
		public IList<Vector3> PatrolPoints
		{
			private get { return _patrolPoints; }
			set
			{
				Assert.IsTrue(value.Count >= 2);
				_patrolPoints = value;
			}
		}

		private Vector3 _targetPatrolPoint;
		private Vector3 TargetPatrolPoint
		{
			get { return _targetPatrolPoint; }
			set
			{
				_targetPatrolPoint = value;
				float distance = Vector3.Distance(transform.position, _targetPatrolPoint);
				_patrollingSmoothTime = distance / maxVelocityInMPerS / SMOOTH_TIME_MULTIPLIER;

				Logging.Log(Tags.BOMBER, $"Setting new patrol point {_targetPatrolPoint}");
			}
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			// Patrolling
			if (TargetPatrolPoint != default(Vector3))
			{
				bool isInPosition = (transform.position - TargetPatrolPoint).magnitude < POSITION_EQUALITY_MARGIN;
				if (!isInPosition)
				{
					transform.position = Vector3.SmoothDamp(transform.position, TargetPatrolPoint, ref _patrollingVelocity, _patrollingSmoothTime, maxVelocityInMPerS);
				}
				else
				{
					Logging.Log(Tags.BOMBER, $"OnUpdate():  Reached patrol point {_targetPatrolPoint}");
					TargetPatrolPoint = FindNextPatrolPoint();
				}
			}
		}

		public void StartPatrolling()
		{
			Assert.IsTrue(PatrolPoints != null);
			TargetPatrolPoint = FindNearestPatrolPoint();
		}

		private Vector3 FindNearestPatrolPoint()
		{
			float minDistance = float.MaxValue;
			Vector3 closestPatrolPoint = default(Vector3);

			foreach (Vector3 patrolPoint in _patrolPoints)
			{
				float distance = Vector3.Distance(transform.position, patrolPoint);
				if (distance < minDistance)
				{
					minDistance = distance;
					closestPatrolPoint = patrolPoint;
				}
			}

			return closestPatrolPoint;
		}

		private Vector3 FindNextPatrolPoint()
		{
			int currentIndex = _patrolPoints.IndexOf(TargetPatrolPoint);
			Assert.IsTrue(currentIndex != -1);
			int nextIndex = currentIndex == _patrolPoints.Count - 1 ? 0 : currentIndex + 1;
			return _patrolPoints[nextIndex];
		}
	}
}
