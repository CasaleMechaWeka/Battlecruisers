using BattleCruisers.Buildables.Units;
using BattleCruisers.Projectiles;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Units.Aircraft
{
	public class BomberController : AircraftController
	{
		private float _velocitySmoothTime;
		private Vector2 _velocity;
		// FELIX  Reset to false when we switch xVelocity (start a new bombing run)
		private bool _haveDroppedBombOnRun;

		public BomberStats bomberStats;
		public BombSpawnerController bombSpawner;

		private const float VELOCITY_EQUALITY_MARGIN = 0.1f;
		private const float SMOOTH_TIME_MULTIPLIER = 2;
		private const float TURN_AROUND_DISTANCE_MULTIPLIER = 2;

		private GameObject _target;
		public GameObject Target 
		{ 
			private get { return _target; }
			set
			{
				_target = value;

				// FELIX  Get to right height before applying horizontal velocity 

				// FELIX  Also do gradual accelleration for boats :)
				float xVelocity = maxVelocityInMPerS;
				if (Target.transform.position.x < transform.position.x)
				{
					xVelocity *= -1;
				}
				TargetVelocity = new Vector2(xVelocity, 0);
			}
		}

		private Vector2 _targetVelocity;
		private Vector2 TargetVelocity
		{
			get { return _targetVelocity; }
			set
			{
				_targetVelocity = value;
				float velocityChange = (rigidBody.velocity - _targetVelocity).magnitude;
				_velocitySmoothTime = velocityChange / maxVelocityInMPerS;
			}
		}

		protected override void OnAwake()
		{
			base.OnAwake();

			_haveDroppedBombOnRun = false;

			bool ignoreGravity = false;
			ShellStats shellStats = new ShellStats(bomberStats.bombPrefab, bomberStats.damage, ignoreGravity, maxVelocityInMPerS);
			bombSpawner.Initialise(Faction, shellStats);
		}

		// FELIX  Refactor this monstrosity!
		protected override void OnUpdate()
		{
			base.OnUpdate();

			// Adjust velocity
			if (rigidBody.velocity != TargetVelocity)
			{
//					Logging.Log(Tags.BOMBER, $"OnUpdate():  rigidBody.velocity: {rigidBody.velocity}  TargetVelocity: {TargetVelocity}");

				if ((rigidBody.velocity - TargetVelocity).magnitude <= VELOCITY_EQUALITY_MARGIN)
				{
					rigidBody.velocity = TargetVelocity;
				}
				else
				{
					rigidBody.velocity = Vector2.SmoothDamp(rigidBody.velocity, TargetVelocity, ref _velocity, _velocitySmoothTime, maxVelocityInMPerS, Time.deltaTime);
				}
			}

			// Bomb target
			if (Target != null)
			{
				if (_haveDroppedBombOnRun)
				{
					if (IsReadyToTurnAround(transform.position, Target.transform.position, maxVelocityInMPerS, TargetVelocity.x))
					{
						Logging.Log(Tags.BOMBER, $"Update():  About to turn around");

						Vector2 newTargetVelocity = new Vector2(maxVelocityInMPerS, 0);
						if (rigidBody.velocity.x > 0)
						{
							newTargetVelocity *= -1;
						}
						TargetVelocity = newTargetVelocity;

						_haveDroppedBombOnRun = false;
					}
				}
				else if (IsDirectionCorrect(rigidBody.velocity.x, TargetVelocity.x)
					&& IsOnTarget(transform.position, Target.transform.position, rigidBody.velocity.x))
				{
					Logging.Log(Tags.BOMBER, $"Update():  About to drop bomb");

					bombSpawner.SpawnShell(rigidBody.velocity.x);
					_haveDroppedBombOnRun = true;
				}
			}
		}

		/// <returns>
		/// True if the bomber has overlown the target enough so that it can turn around
		/// and have enough space for the next bombing run.  False otherwise.
		/// </returns>
		private bool IsReadyToTurnAround(Vector2 planePosition, Vector2 targetPosition, float absoluteMaxXVelocity, float targetXVelocity)
		{
			Assert.IsTrue(targetXVelocity != 0);

			float absoluteLeadDistance = FindLeadDistance(planePosition, targetPosition, absoluteMaxXVelocity);
			float turnAroundDistance = absoluteLeadDistance * TURN_AROUND_DISTANCE_MULTIPLIER;
			float xTurnAroundPosition = targetXVelocity > 0 ? targetPosition.x + turnAroundDistance : targetPosition.x - turnAroundDistance;

//			Logging.Log(Tags.BOMBER, $"IsReadyToTurnAround():  planePosition.x: {planePosition.x}  xTurnAroundPosition: {xTurnAroundPosition}");

			return 
				((targetXVelocity > 0 && planePosition.x >= xTurnAroundPosition)
				|| (targetXVelocity < 0 && planePosition.x <= xTurnAroundPosition));
		}

		/// <summary>
		/// Assumes target is stationary.
		/// </summary>
		private bool IsOnTarget(Vector2 planePosition, Vector2 targetPosition, float planeXVelocityInMPerS)
		{
//			Logging.Log(Tags.BOMBER, $"IsOnTarget():  targetPosition: {targetPosition}  planePosition: {planePosition}  planeXVelocityInMPerS: {planeXVelocityInMPerS}");

			float leadDistance = FindLeadDistance(planePosition, targetPosition, planeXVelocityInMPerS);
			float xDropPosition = planePosition.x + leadDistance;

			return
				((planeXVelocityInMPerS > 0 && xDropPosition >= targetPosition.x)
				|| (planeXVelocityInMPerS < 0 && xDropPosition <= targetPosition.x));
		}

		private bool IsDirectionCorrect(float currentXVelocity, float targetXVelocity)
		{
			return currentXVelocity * targetXVelocity > 0;
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
	}
}
