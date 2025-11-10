using BattleCruisers.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Utils;
using Unity.Netcode;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft
{
    public class PvPBomberController : PvPAircraftController, ITargetConsumer
    {
        private PvPBombSpawner _bombSpawner;
        private ProjectileStats _bombStats;
        private ITargetProcessor _targetProcessor;
        private IBomberMovementController _bomberMovementControler;
        private bool _haveDroppedBombOnRun = false;
        private bool _isAtCruisingHeight = false;

        private const float TURN_AROUND_DISTANCE_MULTIPLIER = 1.5f;
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

        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();
        #endregion Properties

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            _bombSpawner = gameObject.GetComponentInChildren<PvPBombSpawner>();
            Assert.IsNotNull(_bombSpawner);

            _bombStats = GetComponent<ProjectileStats>();
            Assert.IsNotNull(_bombStats);

            float damagePerS = _bombStats.Damage * AVERAGE_FIRE_RATE_PER_S;
            IList<TargetType> attackCapabilities = new List<TargetType>()
            {
                TargetType.Cruiser,
                TargetType.Buildings
            };
            AddDamageStats(new PvPDamageCapability(damagePerS, attackCapabilities));
        }

        public override void Initialise()
        {
            base.Initialise();
            _bomberMovementControler = new BomberMovementController(rigidBody, maxVelocityProvider: this);
        }

        public override void Initialise(PvPUIManager uiManager)
        {
            base.Initialise(uiManager);
            _bomberMovementControler = new BomberMovementController(rigidBody, maxVelocityProvider: this);
        }

        public override void Activate(PvPBuildableActivationArgs activationArgs)
        {
            OnActivatePvPClientRpc(activationArgs.ParentCruiser.Position, activationArgs.EnemyCruiser.Position, activationArgs.ParentCruiser.Direction, isAtCruiserHeight: false);
            base.Activate(activationArgs);
            _haveDroppedBombOnRun = false;
            _isAtCruisingHeight = false;
            Faction enemyFaction = PvPHelper.GetOppositeFaction(Faction);
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(enemyFaction, AttackCapabilities);
            int burstSize = 1;
            // apply variant stats
            ApplyVariantStats();
            IPvPProjectileSpawnerArgs spawnerArgs = new PvPProjectileSpawnerArgs(this, _bombStats, burstSize, _cruiserSpecificFactories, EnemyCruiser);
            _ = _bombSpawner.InitialiseAsync(spawnerArgs, targetFilter);
        }

        public override void Activate_PvPClient()
        {
            base.Activate_PvPClient();
        }

        private async void ApplyVariantStats()
        {
            if (variantIndex != -1)
            {
                VariantPrefab variant = await PvPPrefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(variantIndex));
                GetComponent<ProjectileStats>().ApplyVariantStats(variant.statVariant);
            }
            _bombStats = GetComponent<ProjectileStats>();
            Assert.IsNotNull(_bombStats);
            float damagePerS = _bombStats.Damage * AVERAGE_FIRE_RATE_PER_S;
            IList<TargetType> attackCapabilities = new List<TargetType>()
            {
                TargetType.Cruiser,
                TargetType.Buildings
            };
            AddDamageStats(new PvPDamageCapability(damagePerS, attackCapabilities));
        }

        protected async override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();
                Assert.IsTrue(cruisingAltitudeInM > transform.position.y);
                _targetProcessor = _cruiserSpecificFactories.Targets.ProcessorFactory.BomberTargetProcessor;
                _targetProcessor.AddTargetConsumer(this);
                IList<Sprite> aircraftSprites = await SpriteProvider.GetAircraftSpritesAsync(PrefabKeyName.Unit_Bomber);
                _spriteChooser = new PvPSpriteChooser(aircraftSprites, this);
                OnBuildableCompletedClientRpc();
            }
            else
            {
                OnBuildableCompleted_PvPClient();
                IList<Sprite> aircraftSprites = await SpriteProvider.GetAircraftSpritesAsync(PrefabKeyName.Unit_Bomber);
                _spriteChooser = new PvPSpriteChooser(aircraftSprites, this);
            }
        }

        protected override IList<IPatrolPoint> GetPatrolPoints()
        {
            IList<Vector2> patrolPositions = _aircraftProvider.BomberPatrolPoints(cruisingAltitudeInM);
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
                && Target != null/* && IsServer*/)
            {
                TryBombTarget();
            }
        }

        private void LateUpdate()
        {
            if (IsServer)
            {
                if (PvP_BuildProgress.Value != BuildProgress)
                    PvP_BuildProgress.Value = BuildProgress;
            }
            else
            {
                BuildProgress = PvP_BuildProgress.Value;
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
                if (IsHost)
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




        protected override void OnBuildableProgressEvent()
        {
            if (IsServer)
                OnBuildableProgressEventClientRpc();
            else
                base.OnBuildableProgressEvent();
        }
        protected override void OnCompletedBuildableEvent()
        {
            if (IsServer)
                OnCompletedBuildableEventClientRpc();
            else
                base.OnCompletedBuildableEvent();
        }
        protected override void OnDestroyedEvent()
        {
            if (IsServer)
                OnDestroyedEventClientRpc();
            else
                base.OnDestroyedEvent();
        }

        //-------------------------------------- RPCs -------------------------------------------------//

        [ClientRpc]
        private void OnActivatePvPClientRpc(Vector3 ParentCruiserPosition, Vector3 EnemyCruiserPosition, Direction facingDirection, bool isAtCruiserHeight)
        {
            if (!IsHost)
            {
                _aircraftProvider = new AircraftProvider(ParentCruiserPosition, EnemyCruiserPosition);
                FacingDirection = facingDirection;
                _isAtCruisingHeight = isAtCruiserHeight;
                Activate_PvPClient();
            }
        }

        [ClientRpc]
        private void OnBuildableProgressEventClientRpc()
        {
            if (!IsHost)
                OnBuildableProgressEvent();
        }


        [ClientRpc]
        private void OnCompletedBuildableEventClientRpc()
        {
            if (!IsHost)
                OnCompletedBuildableEvent();
        }

        [ClientRpc]
        private void OnDestroyedEventClientRpc()
        {
            if (!IsHost)
                OnDestroyedEvent();
        }



    }
}
