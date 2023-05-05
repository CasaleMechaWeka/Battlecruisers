using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft
{
    public class PvPBomberController : PvPAircraftController, IPvPTargetConsumer
    {
        private PvPBombSpawner _bombSpawner;
        private IPvPProjectileStats _bombStats;
        private IPvPTargetProcessor _targetProcessor;
        private IPvPBomberMovementController _bomberMovementControler;
        private bool _haveDroppedBombOnRun = false;
        private bool _isAtCruisingHeight = false;

        private const float TURN_AROUND_DISTANCE_MULTIPLIER = 1.5f;
        private const float AVERAGE_FIRE_RATE_PER_S = 0.2f;

        #region Properties
        private IPvPTarget _target;
        public IPvPTarget Target
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

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            _bombSpawner = gameObject.GetComponentInChildren<PvPBombSpawner>();
            Assert.IsNotNull(_bombSpawner);

            _bombStats = GetComponent<PvPProjectileStats>();
            Assert.IsNotNull(_bombStats);

            float damagePerS = _bombStats.Damage * AVERAGE_FIRE_RATE_PER_S;
            IList<PvPTargetType> attackCapabilities = new List<PvPTargetType>()
            {
                PvPTargetType.Cruiser,
                PvPTargetType.Buildings
            };
            AddDamageStats(new PvPDamageCapability(damagePerS, attackCapabilities));
        }

        public override void Initialise(IPvPUIManager uiManager, IPvPFactoryProvider factoryProvider)
        {
            base.Initialise(uiManager, factoryProvider);
            _bomberMovementControler = _movementControllerFactory.CreateBomberMovementController(rigidBody, maxVelocityProvider: this);
        }

        public override void Activate(PvPBuildableActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

            _haveDroppedBombOnRun = false;
            _isAtCruisingHeight = false;

            PvPFaction enemyFaction = PvPHelper.GetOppositeFaction(Faction);
            IPvPTargetFilter targetFilter = _targetFactories.FilterFactory.CreateTargetFilter(enemyFaction, AttackCapabilities);
            int burstSize = 1;
            IPvPProjectileSpawnerArgs spawnerArgs = new PvPProjectileSpawnerArgs(this, _bombStats, burstSize, _factoryProvider, _cruiserSpecificFactories, EnemyCruiser);

            _bombSpawner.InitialiseAsync(spawnerArgs, targetFilter);
        }

        protected async override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            Assert.IsTrue(cruisingAltitudeInM > transform.position.y);

            _targetProcessor = _cruiserSpecificFactories.Targets.ProcessorFactory.BomberTargetProcessor;
            _targetProcessor.AddTargetConsumer(this);

            _spriteChooser = await _factoryProvider.SpriteChooserFactory.CreateBomberSpriteChooserAsync(this);
        }

        protected override IList<IPvPPatrolPoint> GetPatrolPoints()
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
                    // Logging.Log(Tags.AIRCRAFT, "About to turn around");

                    TurnAround();
                    _haveDroppedBombOnRun = false;
                }
            }
            else if (IsDirectionCorrect(rigidBody.velocity.x, _bomberMovementControler.TargetVelocity.x)
                && IsOnTarget(transform.position, Target.Position, rigidBody.velocity.x))
            {
                // Logging.Log(Tags.AIRCRAFT, "About to drop bomb");

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

            // Logging.Verbose(Tags.AIRCRAFT, $"IsReadyToTurnAround():  planePosition.x: {planePosition.x}  xTurnAroundPosition: {xTurnAroundPosition}");

            return
                ((targetXVelocity > 0 && planePosition.x >= xTurnAroundPosition)
                || (targetXVelocity < 0 && planePosition.x <= xTurnAroundPosition));
        }

        private void TurnAround()
        {
            // Logging.Log(Tags.AIRCRAFT, $"Position: {Position}");

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
            // Logging.Verbose(Tags.AIRCRAFT, $"targetPosition: {targetPosition}  planePosition: {planePosition}  planeXVelocityInMPerS: {planeXVelocityInMPerS}");

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
