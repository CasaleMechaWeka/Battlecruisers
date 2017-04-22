using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units
{
	public class BomberController : Unit
	{
		private IList<Vector3> _patrolPoints;
		private Vector3 _targetPatrolPoint;
		private float _smoothTime;
		private Vector3 _velocity;

		private const float POSITION_EQUALITY_MARGIN = 0.1f;

		public void Initialise(IList<Vector3> patrolPoints)
		{
			Assert.IsTrue(patrolPoints.Count >= 2);

			_patrolPoints = patrolPoints;
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			if (_targetPatrolPoint != default(Vector3))
			{
				bool isInPosition = (transform.position - _targetPatrolPoint).magnitude < POSITION_EQUALITY_MARGIN;
				if (!isInPosition)
				{
					transform.position = Vector3.SmoothDamp(transform.position, _targetPatrolPoint, ref _velocity, _smoothTime, velocityInMPerS);
				}
				else if (transform.position != _targetPatrolPoint)
				{
					transform.position = _targetPatrolPoint;
					Logging.Log(Tags.BOMBER, $"Reached patrol point {_targetPatrolPoint}");
				}
				else
				{
					// FELIX  Choose next patrol point
				}
			}
		}

		// FELIX TEMP
		public void TempStartPatrolling()
		{
			StartPatrolling();
		}

		private void StartPatrolling()
		{
			_targetPatrolPoint = FindNearestPatrolPoint();
			float distance = Vector3.Distance(transform.position, _targetPatrolPoint);
			_smoothTime = distance / velocityInMPerS;
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
			return new Vector3();
		}
	}
}
