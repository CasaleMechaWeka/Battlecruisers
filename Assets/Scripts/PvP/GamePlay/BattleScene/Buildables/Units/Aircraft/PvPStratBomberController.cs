using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Utils.Localisation;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using BattleCruisers.Utils;
using BattleCruisers.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft
{
    public class PvPStratBomberController : PvPAircraftController, ITargetConsumer
    {
        private PvPBombSpawner _bombSpawner;
        private IPvPProjectileStats _bombStats;
        private IPvPTargetProcessor _targetProcessor;
        private IPvPBomberMovementController _bomberMovementControler;
        private bool _haveDroppedBombOnRun = false;
        private bool _isAtCruisingHeight = false;
        public List<Sprite> allSprites = new List<Sprite>();

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

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            _bombSpawner = gameObject.GetComponentInChildren<PvPBombSpawner>();
            Assert.IsNotNull(_bombSpawner);

            _bombStats = GetComponent<PvPProjectileStats>();
            Assert.IsNotNull(_bombStats);

            float damagePerS = _bombStats.Damage * AVERAGE_FIRE_RATE_PER_S;
            IList<TargetType> attackCapabilities = new List<TargetType>()
            {
                TargetType.Cruiser,
                TargetType.Buildings
            };
            AddDamageStats(new PvPDamageCapability(damagePerS, attackCapabilities));
        }

        public override void Initialise( /* IPvPUIManager uiManager, */ IPvPFactoryProvider factoryProvider)
        {
            base.Initialise(/* uiManager,*/ factoryProvider);
            _bomberMovementControler = _movementControllerFactory.CreateBomberMovementController(rigidBody, maxVelocityProvider: this);
        }

        public override void Initialise(IPvPFactoryProvider factoryProvider, IPvPUIManager uiManager)
        {
            base.Initialise(factoryProvider, uiManager);
            _bomberMovementControler = _movementControllerFactory.CreateBomberMovementController(rigidBody, maxVelocityProvider: this);
        }

        public override void Activate(PvPBuildableActivationArgs activationArgs)
        {
            OnActivatePvPClientRpc(activationArgs.ParentCruiser.Position, activationArgs.EnemyCruiser.Position, activationArgs.ParentCruiser.Direction, isAtCruiserHeight: false);
            base.Activate(activationArgs);
            _haveDroppedBombOnRun = false;
            _isAtCruisingHeight = false;
            Faction enemyFaction = PvPHelper.GetOppositeFaction(Faction);
            ITargetFilter targetFilter = _targetFactories.FilterFactory.CreateTargetFilter(enemyFaction, AttackCapabilities);
            int burstSize = 1;
            // apply variant stats
            ApplyVariantStats();
            IPvPProjectileSpawnerArgs spawnerArgs = new PvPProjectileSpawnerArgs(this, _bombStats, burstSize, _factoryProvider, _cruiserSpecificFactories, EnemyCruiser);
            _bombSpawner.InitialiseAsync(spawnerArgs, targetFilter);
        }

        public override void Activate_PvPClient()
        {
            base.Activate_PvPClient();
        }

        private async void ApplyVariantStats()
        {
            if (variantIndex != -1)
            {
                VariantPrefab variant = await PvPBattleSceneGodClient.Instance.factoryProvider.PrefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(variantIndex));
                GetComponent<PvPProjectileStats>().ApplyVariantStats(variant.statVariant);
            }
            _bombStats = GetComponent<PvPProjectileStats>();
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
                List<ISpriteWrapper> allSpriteWrappers = new List<ISpriteWrapper>();
                foreach (Sprite sprite in allSprites)
                {
                    allSpriteWrappers.Add(new SpriteWrapper(sprite));
                }
                //create Sprite Chooser
                _spriteChooser = new PvPSpriteChooser(new PvPAssignerFactory(), allSpriteWrappers, this);
                OnBuildableCompletedClientRpc();

            }
            else
            {
                OnBuildableCompleted_PvPClient();
                List<ISpriteWrapper> allSpriteWrappers = new List<ISpriteWrapper>();
                foreach (Sprite sprite in allSprites)
                {
                    allSpriteWrappers.Add(new SpriteWrapper(sprite));
                }
                //create Sprite Chooser
                _spriteChooser = new PvPSpriteChooser(new PvPAssignerFactory(), allSpriteWrappers, this);
            }
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

        // BuildableStatus
        protected override void OnBuildableStateValueChanged(PvPBuildableState state)
        {
            OnBuildableStateValueChangedClientRpc(state);
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
            return Mathf.Sqrt(2 * verticalDistanceInM / (_bombStats.GravityScale * PvPConstants.GRAVITY));
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

        //------------------------------------ methods for sync, written by Sava ------------------------------//

        // Visibility 
        protected override void OnValueChangedIsEnableRenderes(bool isEnabled)
        {
            if (IsServer)
                OnValueChangedIsEnabledRendersClientRpc(isEnabled);
            else
                base.OnValueChangedIsEnableRenderes(isEnabled);
        }

        // ProgressController Visible
        protected override void CallRpc_ProgressControllerVisible(bool isEnabled)
        {
            if (IsServer)
            {
                OnProgressControllerVisibleClientRpc(isEnabled);
                base.CallRpc_ProgressControllerVisible(isEnabled);
            }
            else
                base.CallRpc_ProgressControllerVisible(isEnabled);
        }

        // set Position of PvPBuildable
        protected override void CallRpc_SetPosition(Vector3 pos)
        {
            OnSetPositionClientRpc(pos);
        }

        // Set Rotation of PvPBuildable
        protected override void CallRpc_SetRotation(Quaternion rotation)
        {
            OnSetRotationClientRpc(rotation);
        }
        private void ActiveTrail()
        {
            _aircraftTrailObj.SetActive(true);
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
        private void OnSetTargetClientRpc(ulong objectId)
        {
            if (!IsHost)
            {
                if (objectId == ulong.MaxValue)
                {
                    _target = null;
                }
                else
                {
                    NetworkObject obj = PvPBattleSceneGodClient.Instance.GetNetworkObject(objectId);
                    if (obj != null)
                    {
                        ITarget target = obj.gameObject.GetComponent<PvPBuildableWrapper<IPvPBuilding>>()?.Buildable?.Parse<ITarget>();
                        if (target == null)
                        {
                            target = obj.gameObject.GetComponent<PvPBuildableWrapper<IPvPUnit>>()?.Buildable?.Parse<ITarget>();
                        }
                        _target = target;
                    }
                }
            }
        }
        [ClientRpc]
        private void OnValueChangedIsEnabledRendersClientRpc(bool isEnabled)
        {
            if (!IsHost)
                OnValueChangedIsEnableRenderes(isEnabled);
        }

        [ClientRpc]
        private void OnProgressControllerVisibleClientRpc(bool isEnabled)
        {
            _buildableProgress.gameObject.SetActive(isEnabled);
            if (!IsHost)
            {
                if (!isEnabled)
                {
                    Invoke("ActiveTrail", 0.5f);
                }
            }
        }

        [ClientRpc]
        private void OnSetPositionClientRpc(Vector3 pos)
        {
            if (!IsHost)
                Position = pos;
        }

        [ClientRpc]
        private void OnSetRotationClientRpc(Quaternion rotation)
        {
            if (!IsHost)
                Rotation = rotation;
        }

        [ClientRpc]
        private void OnActivatePvPClientRpc(Vector3 ParentCruiserPosition, Vector3 EnemyCruiserPosition, PvPDirection facingDirection, bool isAtCruiserHeight)
        {
            if (!IsHost)
            {
                _aircraftProvider = new PvPAircraftProvider(ParentCruiserPosition, EnemyCruiserPosition, RandomGenerator.Instance);
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

        [ClientRpc]
        private void OnBuildableCompletedClientRpc()
        {
            if (!IsHost)
                OnBuildableCompleted();
        }

        [ClientRpc]
        protected void OnBuildableStateValueChangedClientRpc(PvPBuildableState state)
        {
            if (!IsHost)
                BuildableState = state;
        }
    }
}
