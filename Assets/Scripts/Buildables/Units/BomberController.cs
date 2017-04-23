using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units
{
	public class BomberController : Unit
	{
		private float _smoothTime;
		private Vector3 _velocity;

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
				_smoothTime = distance / velocityInMPerS / SMOOTH_TIME_MULTIPLIER;

				Logging.Log(Tags.BOMBER, $"Setting new patrol point {_targetPatrolPoint}");
			}
		}

		private GameObject _target;
		public GameObject Target 
		{ 
			private get { return _target; }
			set
			{
				_target = value;

				// FELIX  Get to right height before applying horizontal velocity

				// FELIX  Smoothly accelerate to top speed
				// FELIX  Also do this for boats :)
				float xVelocity = velocityInMPerS;
				if (Target.transform.position.x < transform.position.x)
				{
					xVelocity *= -1;
				}
				rigidBody.velocity = new Vector2(xVelocity, 0);
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
					transform.position = Vector3.SmoothDamp(transform.position, TargetPatrolPoint, ref _velocity, _smoothTime, velocityInMPerS);
				}
				else
				{
					Logging.Log(Tags.BOMBER, $"Reached patrol point {_targetPatrolPoint}");
					TargetPatrolPoint = FindNextPatrolPoint();
				}
			}

			// FELIX
//			// Have target
//			if (Target != null)
//			{
//				
//			}
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
