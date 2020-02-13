using BattleCruisers.Buildables.Pools;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Data.Static;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using System.Collections.Generic;
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
		private bool _haveDroppedBombOnRun = false;
        private bool _isAtCruisingHeight = false;

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

                if (_target == null)
                {
                    ActiveMovementController = PatrollingMovementController;
                }
                else if (_isAtCruisingHeight)
                {
                    SwitchToBomberMovement();
                }
			}
		}

        protected override ISoundKey EngineSoundKey => SoundKeys.Engines.Bomber;

        protected override Vector2 MaskHighlightableSize
        {
            get
            {
                return
                    new Vector2(
                        base.MaskHighlightableSize.x * 2,
                        base.MaskHighlightableSize.y * 8);
            }
        }
        #endregion Properties

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
		{
            base.StaticInitialise(parent, healthBar);

			_bombSpawner = gameObject.GetComponentInChildren<BombSpawner>();
			Assert.IsNotNull(_bombSpawner);

            _bombStats = GetComponent<ProjectileStats>();
            Assert.IsNotNull(_bombStats);

            float damagePerS = _bombStats.Damage * AVERAGE_FIRE_RATE_PER_S;
            IList<TargetType> attackCapabilities = new List<TargetType>()
            {
                TargetType.Cruiser,
                TargetType.Buildings
            };
            AddDamageStats(new DamageCapability(damagePerS, attackCapabilities));
		}

        public override void Initialise(IUIManager uiManager, IFactoryProvider factoryProvider)
        {
            base.Initialise(uiManager, factoryProvider);
            _bomberMovementControler = _movementControllerFactory.CreateBomberMovementController(rigidBody, maxVelocityProvider: this);
        }

        public override void Activate(BuildableActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

            _haveDroppedBombOnRun = false;
            _isAtCruisingHeight = false;

            Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            ITargetFilter targetFilter = _targetFactories.FilterFactory.CreateTargetFilter(enemyFaction, AttackCapabilities);
            int burstSize = 1;
            IProjectileSpawnerArgs spawnerArgs = new ProjectileSpawnerArgs(this, _bombStats, burstSize, _factoryProvider);

            _bombSpawner.Initialise(spawnerArgs, targetFilter);
		}
		
		protected async override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			Assert.IsTrue(cruisingAltitudeInM > transform.position.y);

			_targetProcessor = _cruiserSpecificFactories.Targets.ProcessorFactory.BomberTargetProcessor;
			_targetProcessor.AddTargetConsumer(this);

            _spriteChooser = await _factoryProvider.SpriteChooserFactory.CreateBomberSpriteChooserAsync(this);
		}

		protected override IList<IPatrolPoint> GetPatrolPoints()
		{
            IList<Vector2> patrolPositions = _aircraftProvider.FindBomberPatrolPoints(cruisingAltitudeInM);
            return ProcessPatrolPoints(patrolPositions, OnFirstPatrolPointReached);
		}

		private void OnFirstPatrolPointReached()
        {
            _isAtCruisingHeight = true;
            SwitchToBomberMovement();
        }

        private void SwitchToBomberMovement()
        {
            SetTargetVelocity();
            ActiveMovementController = _bomberMovementControler;
        }

        protected override void OnFixedUpdate()
		{
			base.OnFixedUpdate();

            if (_isAtCruisingHeight 
                && !IsInKamikazeMode
                && Target != null)
			{
				TryBombTarget();
			}
		}

		private void TryBombTarget()
		{
			if (_haveDroppedBombOnRun)
			{
				if (IsReadyToTurnAround(transform.position, Target.Position, EffectiveMaxVelocityInMPerS, _bomberMovementControler.TargetVelocity.x))
				{
					Logging.Log(Tags.AIRCRAFT, "About to turn around");

					TurnAround();
					_haveDroppedBombOnRun = false;
				}
			}
			else if (IsDirectionCorrect(rigidBody.velocity.x, _bomberMovementControler.TargetVelocity.x)
				&& IsOnTarget(transform.position, Target.Position, rigidBody.velocity.x))
			{
				Logging.Log(Tags.AIRCRAFT, "About to drop bomb");

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

            Logging.Verbose(Tags.AIRCRAFT, $"IsReadyToTurnAround():  planePosition.x: {planePosition.x}  xTurnAroundPosition: {xTurnAroundPosition}");

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
            Logging.Verbose(Tags.AIRCRAFT, $"targetPosition: {targetPosition}  planePosition: {planePosition}  planeXVelocityInMPerS: {planeXVelocityInMPerS}");

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
		/// The x distance before the target where the bomb needs to be dropped
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
			return Mathf.Sqrt(2 * verticalDistanceInM / (_bombStats.GravityScale * Constants.GRAVITY));
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

        protected override void CleanUp()
        {
            base.CleanUp();

			_targetProcessor.RemoveTargetConsumer(this);
			_targetProcessor = null;
        }
    }
}
