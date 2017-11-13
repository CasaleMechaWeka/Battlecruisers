using System.Collections.Generic;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public class BomberController : AircraftController, ITargetConsumer
	{
		private BombSpawner _bombSpawner;
        private IProjectileStats _bombStats;
        private ITargetProcessor _targetProcessor;
        private IBomberMovementController _bomberMovementControler;
		private bool _haveDroppedBombOnRun;
        private bool _isAtCruisingHeight;

		public float cruisingAltitudeInM;

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
                SetTargetVelocity();
			}
		}

		public override float Damage 
		{ 
			get 
			{ 
                return _bombStats.Damage * AVERAGE_FIRE_RATE_PER_S;
			} 
		}
		#endregion Properties

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			_haveDroppedBombOnRun = false;
			_isAtCruisingHeight = false;

			_attackCapabilities.Add(TargetType.Cruiser);
			_attackCapabilities.Add(TargetType.Buildings);
			
			_bombSpawner = gameObject.GetComponentInChildren<BombSpawner>();
			Assert.IsNotNull(_bombSpawner);

            ProjectileStats stats = GetComponent<ProjectileStats>();
            _bombStats = new ProjectileStatsWrapper(stats);
            Assert.IsNotNull(_bombStats);
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            ITargetFilter targetFilter = _targetsFactory.CreateTargetFilter(enemyFaction, _attackCapabilities);

            _bombSpawner.Initialise(_bombStats, targetFilter, _factoryProvider);

            _bomberMovementControler = _movementControllerFactory.CreateBomberMovementController(rigidBody, maxVelocityProvider: this);
		}
		
		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			Assert.IsTrue(cruisingAltitudeInM > transform.position.y);

			_targetProcessor = _targetsFactory.BomberTargetProcessor;
			_targetProcessor.AddTargetConsumer(this);
            _targetProcessor.StartProcessingTargets();

            _spriteChooser = _factoryProvider.SpriteChooserFactory.CreateBomberSpriteChooser(this);
		}

		protected override IList<IPatrolPoint> GetPatrolPoints()
		{
			IList<Vector2> patrolPositions = _aircraftProvider.FindBomberPatrolPoints(cruisingAltitudeInM);
            return ProcessPatrolPoints(patrolPositions, OnFirstPatrolPointReached);
		}

		private void OnFirstPatrolPointReached()
		{
			_isAtCruisingHeight = true;
			SwitchMovementControllers(_bomberMovementControler);
		}

		protected override void OnFixedUpdate()
		{
			base.OnFixedUpdate();

            if (_isAtCruisingHeight && !IsInKamikazeMode)
			{
				TryBombTarget();
			}
		}

		private void TryBombTarget()
		{
			Assert.IsNotNull(Target);

			if (_haveDroppedBombOnRun)
			{
				if (IsReadyToTurnAround(transform.position, Target.GameObject.transform.position, EffectiveMaxVelocityInMPerS, _bomberMovementControler.TargetVelocity.x))
				{
					Logging.Log(Tags.AIRCRAFT, "TryBombTarget():  About to turn around");

					TurnAround();
					_haveDroppedBombOnRun = false;
				}
			}
			else if (IsDirectionCorrect(rigidBody.velocity.x, _bomberMovementControler.TargetVelocity.x)
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
			Vector2 newTargetVelocity = new Vector2(EffectiveMaxVelocityInMPerS, 0);
			if (rigidBody.velocity.x > 0)
			{
				newTargetVelocity *= -1;
			}
			_bomberMovementControler.TargetVelocity = newTargetVelocity;
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

		protected override void OnBoostChanged()
        {
            SetTargetVelocity();
        }

        private void SetTargetVelocity()
        {
			if (_target != null)
			{
				_bomberMovementControler.TargetVelocity = FindTargetVelocity(_target.Position);
			}
        }

        private Vector2 FindTargetVelocity(Vector2 targetPosition)
        {
			float xVelocity = EffectiveMaxVelocityInMPerS;
            if (targetPosition.x < transform.position.x)
			{
				xVelocity *= -1;
			}
			return new Vector2(xVelocity, 0);
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
