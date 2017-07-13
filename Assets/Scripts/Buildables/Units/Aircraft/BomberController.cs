using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft
{
	public class BomberController : AircraftController, ITargetConsumer
	{
		private float _velocitySmoothTime;
		private Vector2 _velocity;
		private bool _haveDroppedBombOnRun;
		private Vector2 _targetCruisingHeight;
		private ITargetProcessor _targetProcessor;
		private BombSpawner _bombSpawner;

		public BomberStats bomberStats;
		public float cruisingAltitudeInM;

		private const float CRUISING_HEIGHT_EQUALITY_MARGIN = 0.2f;
		private const float VELOCITY_EQUALITY_MARGIN = 0.1f;
		private const float SMOOTH_TIME_MULTIPLIER = 2;
		private const float TURN_AROUND_DISTANCE_MULTIPLIER = 2;
		private const float AVERAGE_FIRE_RATE_PER_S = 0.2f;

		#region Properties
		private ITarget _target;
		public ITarget Target
		{ 
			get { return _target; }
			set
			{
				_target = value;

				if (_target != null)
				{
					float xVelocity = maxVelocityInMPerS;
					if (_target.Position.x < transform.position.x)
					{
						xVelocity *= -1;
					}
					TargetVelocity = new Vector2(xVelocity, 0);
				}
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

		private bool IsAtCruisingHeight
		{
			get
			{
				return Mathf.Abs(transform.position.y - cruisingAltitudeInM) <= CRUISING_HEIGHT_EQUALITY_MARGIN;
			}
		}

		public override float Damage 
		{ 
			get 
			{ 
				return bomberStats.damage * AVERAGE_FIRE_RATE_PER_S;
			} 
		}
		#endregion Properties

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			Assert.IsNotNull(bomberStats);
			
			_haveDroppedBombOnRun = false;
			_attackCapabilities.Add(TargetType.Cruiser);
			_attackCapabilities.Add(TargetType.Buildings);
			
			_bombSpawner = gameObject.GetComponentInChildren<BombSpawner>();
			Assert.IsNotNull(_bombSpawner);
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

			bool ignoreGravity = false;
			ShellStats shellStats = new ShellStats(bomberStats.bombPrefab, bomberStats.damage, ignoreGravity, maxVelocityInMPerS);
			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
			ITargetFilter targetFilter = _targetsFactory.CreateTargetFilter(enemyFaction, _attackCapabilities);

			_bombSpawner.Initialise(shellStats, targetFilter);
		}
		
		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			Assert.IsTrue(cruisingAltitudeInM > transform.position.y);

			PatrolPoints = _aircraftProvider.FindBomberPatrolPoints(cruisingAltitudeInM);
			// FELIX
//			StartPatrolling();

			_targetProcessor = _targetsFactory.BomberTargetProcessor;
			_targetProcessor.AddTargetConsumer(this);
		}

		protected override void OnFixedUpdate()
		{
			base.OnFixedUpdate();

			if (IsAtCruisingHeight)
			{
				Assert.IsNotNull(Target);

				// FELIX
//				StopPatrolling();

				if (rigidBody.velocity != TargetVelocity)
				{
					AdjustVelocity();
				}

				TryBombTarget();
			}
		}

		private void AdjustVelocity()
		{
			Logging.Verbose(Tags.AIRCRAFT, string.Format("AdjustVelocity():  rigidBody.velocity: {0}  TargetVelocity: {1}", rigidBody.velocity, TargetVelocity));

			Vector2 oldVelocity = rigidBody.velocity;

			if ((rigidBody.velocity - TargetVelocity).magnitude <= VELOCITY_EQUALITY_MARGIN)
			{
				rigidBody.velocity = TargetVelocity;
			}
			else
			{
				rigidBody.velocity = Vector2.SmoothDamp(rigidBody.velocity, TargetVelocity, ref _velocity, _velocitySmoothTime, maxVelocityInMPerS, Time.deltaTime);
			}

			// FELIX
//			UpdateFacingDirection(oldVelocity, rigidBody.velocity);
		}

		private void TryBombTarget()
		{
			if (_haveDroppedBombOnRun)
			{
				if (IsReadyToTurnAround(transform.position, Target.GameObject.transform.position, maxVelocityInMPerS, TargetVelocity.x))
				{
					Logging.Log(Tags.AIRCRAFT, "TryBombTarget():  About to turn around");

					TurnAround();
					_haveDroppedBombOnRun = false;
				}
			}
			else if (IsDirectionCorrect(rigidBody.velocity.x, TargetVelocity.x)
				&& IsOnTarget(transform.position, Target.GameObject.transform.position, rigidBody.velocity.x))
			{
				Logging.Log(Tags.AIRCRAFT, "TryBombTarget():  About to drop bomb");

				_bombSpawner.SpawnShell(rigidBody.velocity.x);
				_haveDroppedBombOnRun = true;
			}
		}
		
		/// <returns>
		/// True if the bomber has overflown the target enough so that it can turn around
		/// and have enough space for the next bombing run.  False otherwise.
		/// </returns>
		private bool IsReadyToTurnAround(Vector2 planePosition, Vector2 targetPosition, float absoluteMaxXVelocity, float targetXVelocity)
		{
			Assert.IsTrue(targetXVelocity != 0);

			float absoluteLeadDistance = FindLeadDistance(planePosition, targetPosition, absoluteMaxXVelocity);
			float turnAroundDistance = absoluteLeadDistance * TURN_AROUND_DISTANCE_MULTIPLIER;
			float xTurnAroundPosition = targetXVelocity > 0 ? targetPosition.x + turnAroundDistance : targetPosition.x - turnAroundDistance;

			Logging.Verbose(Tags.AIRCRAFT, string.Format("IsReadyToTurnAround():  planePosition.x: {0}  xTurnAroundPosition: {1}", planePosition.x, xTurnAroundPosition));

			return 
				((targetXVelocity > 0 && planePosition.x >= xTurnAroundPosition)
				|| (targetXVelocity < 0 && planePosition.x <= xTurnAroundPosition));
		}

		private void TurnAround()
		{
			Vector2 newTargetVelocity = new Vector2(maxVelocityInMPerS, 0);
			if (rigidBody.velocity.x > 0)
			{
				newTargetVelocity *= -1;
			}
			TargetVelocity = newTargetVelocity;
		}

		/// <summary>
		/// Assumes target is stationary.
		/// </summary>
		private bool IsOnTarget(Vector2 planePosition, Vector2 targetPosition, float planeXVelocityInMPerS)
		{
			Logging.Verbose(Tags.AIRCRAFT, string.Format("IsOnTarget():  targetPosition: {0}  planePosition: {1}  planeXVelocityInMPerS: {2}", targetPosition, planePosition, planeXVelocityInMPerS));

			float leadDistance = FindLeadDistance(planePosition, targetPosition, planeXVelocityInMPerS);
			float xDropPosition = targetPosition.x - leadDistance;

			return
				((planeXVelocityInMPerS > 0 && planePosition.x >= xDropPosition)
				|| (planeXVelocityInMPerS < 0 && planePosition.x <= xDropPosition));
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

		protected override void OnDestroyed()
		{
			base.OnDestroyed();

			if (BuildableState == BuildableState.Completed)
			{
				_targetProcessor.RemoveTargetConsumer(this);
				_targetProcessor = null;
			}
		}
	}
}
