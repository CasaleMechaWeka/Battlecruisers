using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Projectiles;
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
		// FELIX  Reset to false when we switch xVelocity (start a new bombing run)
		private bool _haveDroppedBombOnRun;

		public BomberStats bomberStats;
		public BombSpawnerController bombSpawner;

		private const float POSITION_EQUALITY_MARGIN = 0.1f;
		private const float SMOOTH_TIME_MULTIPLIER = 2;
		private const float TURN_AROUND_DISTANCE_MULTIPLIER = 2;

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

		protected override void OnAwake()
		{
			base.OnAwake();

			_haveDroppedBombOnRun = false;

			bool ignoreGravity = false;
			ShellStats shellStats = new ShellStats(bomberStats.bombPrefab, bomberStats.damage, ignoreGravity, velocityInMPerS);
			bombSpawner.Initialise(Faction, shellStats);
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
					Logging.Log(Tags.BOMBER, $"OnUpdate():  Reached patrol point {_targetPatrolPoint}");
					TargetPatrolPoint = FindNextPatrolPoint();
				}
			}

			// Bomb target
			if (Target != null)
			{
				if (_haveDroppedBombOnRun)
				{
					if (IsReadyToTurnAround(transform.position, Target.transform.position, velocityInMPerS, rigidBody.velocity.x))
					{
						// FELIX  Gradually turn around :P
						Vector2 currentVelocity = rigidBody.velocity;
						rigidBody.velocity = currentVelocity * -1;
						
						// FELIX  Only reset this after we have changed velocity direction!
						_haveDroppedBombOnRun = false;
					}
				}
				else
				{
					if (IsOnTarget(transform.position, Target.transform.position, rigidBody.velocity.x))
					{
						bombSpawner.SpawnShell(rigidBody.velocity.x);
						_haveDroppedBombOnRun = true;
					}
				}
			}
		}

		/// <returns>
		/// True if the bomber has overlown the target enough so that it can turn around
		/// and have enough space for the next bombing run.  False otherwise.
		/// </returns>
		private bool IsReadyToTurnAround(Vector2 planePosition, Vector2 targetPosition, float absoluteMaxXVelocity, float currentXVelocity)
		{
			if (currentXVelocity == 0)
			{
				return false;
			}

			float absoluteLeadDistance = FindLeadDistance(planePosition, targetPosition, absoluteMaxXVelocity);
			float turnAroundDistance = absoluteLeadDistance * TURN_AROUND_DISTANCE_MULTIPLIER;
			float xTurnAroundPosition = currentXVelocity > 0 ? targetPosition.x + turnAroundDistance : targetPosition.x - turnAroundDistance;

			Logging.Log(Tags.BOMBER, $"IsReadyToTurnAround():  planePosition.x: {planePosition.x}  xTurnAroundPosition: {xTurnAroundPosition}");

			return 
				((currentXVelocity > 0 && planePosition.x >= xTurnAroundPosition)
				|| (currentXVelocity < 0 && planePosition.x <= xTurnAroundPosition));
		}

		/// <summary>
		/// Assumes target is stationary.
		/// </summary>
		private bool IsOnTarget(Vector2 planePosition, Vector2 targetPosition, float planeXVelocityInMPerS)
		{
			Logging.Log(Tags.BOMBER, $"IsOnTarget():  targetPosition: {targetPosition}  planePosition: {planePosition}  planeXVelocityInMPerS: {planeXVelocityInMPerS}");

			float leadDistance = FindLeadDistance(planePosition, targetPosition, planeXVelocityInMPerS);
			float xDropPosition = planePosition.x + leadDistance;

			return
				((planeXVelocityInMPerS > 0 && xDropPosition >= targetPosition.x)
				|| (planeXVelocityInMPerS < 0 && xDropPosition <= targetPosition.x));
		}

		/// <summary>
		/// Note:
		/// 1. Will be negative if xVelocityInMPerS is negative!
		/// 2. Assumes the target is stationary.
		/// </summary>
		/// <returns>>
		/// The y distance before the target where the bomb needs to be dropped
		/// for it to land on the target.
		/// </returns>
		private float FindLeadDistance(Vector2 planePosition, Vector2 targetPosition, float xVelocityInMPerS)
		{
			float yDifference = planePosition.y - targetPosition.y;
			Assert.IsTrue(yDifference > 0);
			float timeBombWillTravel = FindTimeBombWillTravel(yDifference);
			return xVelocityInMPerS * timeBombWillTravel;
		}

		private float FindTimeBombWillTravel(float verticalDistanceInM)
		{
			return Mathf.Sqrt(2 * verticalDistanceInM / Constants.GRAVITY);
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
