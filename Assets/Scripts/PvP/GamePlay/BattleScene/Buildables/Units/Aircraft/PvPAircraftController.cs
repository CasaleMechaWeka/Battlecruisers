using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Seabed;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft
{
    public abstract class PvPAircraftController : PvPUnit, IPvPVelocityProvider, IPvPPatrollingVelocityProvider, IPvPSeabedImpactable
    {
        private PvPKamikazeController _kamikazeController;
        private Collider2D _collider;
        private SpriteRenderer _spriteRenderer;
        private IPvPBoostable _velocityBoostable;
        private float _fuzziedMaxVelocityInMPerS;
        private TrailRenderer _aircraftTrail;
        private bool _onSeabed;

        protected IPvPSpriteChooser _spriteChooser;

        public float cruisingAltitudeInM;
        public float seabedParkTimeInS = 10;

        private const float MAX_VELOCITY_FUZZING_PROPORTION = 0.1f;
        private const float ON_DEATH_GRAVITY_SCALE = 0.4f;
        private const float SEABED_SAFE_POSITION_Y = -40;

        protected bool IsInKamikazeMode => _kamikazeController.isActiveAndEnabled;
        public override PvPTargetType TargetType => PvPTargetType.Aircraft;
        public override Vector2 Velocity => ActiveMovementController.Velocity;
        protected virtual float MaxPatrollingVelocity => EffectiveMaxVelocityInMPerS;
        protected float EffectiveMaxVelocityInMPerS => _velocityBoostable.BoostMultiplier * _fuzziedMaxVelocityInMPerS;
        public float PatrollingVelocityInMPerS => MaxPatrollingVelocity;
        public float VelocityInMPerS => EffectiveMaxVelocityInMPerS;
        protected virtual float PositionEqualityMarginInM => 0.5f;
        protected override bool ShowSmokeWhenDestroyed => true;

        protected IPvPMovementController DummyMovementController { get; private set; }
        protected IPvPMovementController PatrollingMovementController { get; private set; }

        private IPvPMovementController _activeMovementController;
        protected IPvPMovementController ActiveMovementController
        {
            get { return _activeMovementController; }
            set
            {
                // Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}  {ActiveMovementController}  => {value}");
                Assert.IsNotNull(value);

                if (ReferenceEquals(ActiveMovementController, value))
                {
                    // Already have the desired movement controller
                    return;
                }

                if (ActiveMovementController != null)
                {
                    value.Velocity = ActiveMovementController.Velocity;
                    ActiveMovementController.DirectionChanged -= _movementController_DirectionChanged;
                }

                _activeMovementController = value;

                ActiveMovementController.DirectionChanged += _movementController_DirectionChanged;
                ActiveMovementController.Activate();
            }
        }

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            _kamikazeController = GetComponentInChildren<PvPKamikazeController>(includeInactive: true);
            Assert.IsNotNull(_kamikazeController);
            Assert.IsFalse(IsInKamikazeMode);

            _collider = GetComponent<Collider2D>();
            Assert.IsNotNull(_collider);

            _spriteRenderer = GetComponentInChildren<SpriteRenderer>(includeInactive: true);
            Assert.IsNotNull(_spriteRenderer);

            _aircraftTrail = transform.FindNamedComponent<TrailRenderer>("AircraftTrail");
        }

        public override void Initialise( /* IPvPUIManager uiManager, */IPvPFactoryProvider factoryProvider)
        {
            base.Initialise( /* uiManager, */ factoryProvider);

            _velocityBoostable = _factoryProvider.BoostFactory.CreateBoostable();
            _fuzziedMaxVelocityInMPerS = PvPRandomGenerator.Instance.Randomise(maxVelocityInMPerS, MAX_VELOCITY_FUZZING_PROPORTION, PvPChangeDirection.Both);
            DummyMovementController = _movementControllerFactory.CreateDummyMovementController();
        }

        public override void Activate(PvPBuildableActivationArgs activationArgs)
        {
            // Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}");

            // Needs to happen before we are moved to a new position and have our game object enabled, otherwise get trail from last death position.
            _aircraftTrail.Clear();

            base.Activate(activationArgs);

            _localBoosterBoostableGroup.AddBoostable(_velocityBoostable);
            _localBoosterBoostableGroup.AddBoostProvidersList(_cruiserSpecificFactories.GlobalBoostProviders.AircraftBoostProviders);
            _localBoosterBoostableGroup.BoostChanged += _boostableGroup_BoostChanged;

            PatrollingMovementController
                = _movementControllerFactory.CreatePatrollingMovementController(
                    rigidBody,
                    maxVelocityProvider: _movementControllerFactory.CreatePatrollingVelocityProvider(this),
                    patrolPoints: GetPatrolPoints(),
                    positionEqualityMarginInM: PositionEqualityMarginInM);

            ActiveMovementController = DummyMovementController;
            ActiveMovementController.Velocity = Vector2.zero;

            _spriteChooser = _factoryProvider.SpriteChooserFactory.CreateDummySpriteChooser(_spriteRenderer.sprite);
            _onSeabed = false;

            _kamikazeController.gameObject.SetActive(false);
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();
            ActiveMovementController = PatrollingMovementController;
        }

        protected override void AddBuildRateBoostProviders(
            IPvPGlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IPvPBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(globalBoostProviders.UnitBuildRate.AircraftProviders);
        }

        protected abstract IList<IPvPPatrolPoint> GetPatrolPoints();

        private void _movementController_DirectionChanged(object sender, PvPXDirectionChangeEventArgs e)
        {
            FacingDirection = e.NewDirection;
        }

        protected override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            // Logging.Verbose(Tags.AIRCRAFT, $"{GetInstanceID()}  Adjusting velocity");

            Assert.IsNotNull(ActiveMovementController, "OnInitialised() should always be called before OnFixedUpdate()");
            ActiveMovementController.AdjustVelocity();
            //compare sprite number choses to sprite name
            _spriteRenderer.sprite = _spriteChooser.ChooseSprite(Velocity).Sprite;
        }

        public void Kamikaze(IPvPTarget kamikazeTarget)
        {
            Assert.AreEqual(PvPUnitCategory.Aircraft, Category, "Only aircraft should kamikaze");
            Assert.AreEqual(PvPBuildableState.Completed, BuildableState, "Only completed aircraft should kamikaze.");

            if (IsInKamikazeMode)
            {
                // Already in kamikaze mode, no need to do anything again :)
                return;
            }

            IPvPTargetProvider cruiserTarget = _cruiserSpecificFactories.Targets.ProviderFactory.CreateStaticTargetProvider(kamikazeTarget);
            ActiveMovementController = _movementControllerFactory.CreateHomingMovementController(rigidBody, this, cruiserTarget);

            UpdateFaction(kamikazeTarget);

            OnKamikaze();
        }

        private void UpdateFaction(IPvPTarget kamikazeTarget)
        {
            Faction = PvPHelper.GetOppositeFaction(kamikazeTarget.Faction);

            // Make our collider be lost and refound by all target detectors.
            // Means target detectors that we are already in range of can 
            // re-evaluate whether we are a target, as our faction has just changed.
            _collider.enabled = false;
            _collider.enabled = true;

            _kamikazeController.Initialise(this, _factoryProvider, kamikazeTarget);
            _kamikazeController.gameObject.SetActive(true);
        }

        protected IList<IPvPPatrolPoint> ProcessPatrolPoints(IList<Vector2> patrolPositions, Action onFirstPatrolPointReached)
        {
            IList<IPvPPatrolPoint> patrolPoints = new List<IPvPPatrolPoint>(patrolPositions.Count);

            patrolPoints.Add(new PvPPatrolPoint(patrolPositions[0], removeOnceReached: false, actionOnReached: onFirstPatrolPointReached));

            for (int i = 1; i < patrolPositions.Count; ++i)
            {
                patrolPoints.Add(new PvPPatrolPoint(patrolPositions[i]));
            }

            return patrolPoints;
        }

        private void _boostableGroup_BoostChanged(object sender, EventArgs e)
        {
            OnBoostChanged();
        }

        protected virtual void OnBoostChanged() { }

        protected override void OnDestroyed()
        {
            // Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}");

            base.OnDestroyed();

            _localBoosterBoostableGroup.BoostChanged -= _boostableGroup_BoostChanged;

            if (BuildableState == PvPBuildableState.Completed
                && !IsInKamikazeMode)
            {
                CleanUp();
            }
        }

        protected override bool ShouldShowDeathEffects()
        {
            return
                base.ShouldShowDeathEffects()
                && !IsInKamikazeMode;
        }

        protected override void ShowDeathEffects()
        {
            // Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}");
            HealthBar.IsVisible = false;

            // Make gravity take effect
            rigidBody.bodyType = RigidbodyType2D.Dynamic;
            rigidBody.gravityScale = ON_DEATH_GRAVITY_SCALE;

            // Pass on current velocity
            rigidBody.AddForce(Velocity, ForceMode2D.Impulse);

            // Make aircraft spin a bit for coolness
            rigidBody.AddTorque(0.5f, ForceMode2D.Impulse);
        }

        private void OnKamikaze()
        {
            CleanUp();
        }

        protected virtual void CleanUp() { }

        /// <summary>
        /// Stop movement, wait until smoke trail has dissipated before removing
        /// game object from scene.  Otherwise smoke trail insta-disappears :P
        /// </summary>
        public void OnHitSeabed()
        {
            // Logging.Log(Tags.AIRCRAFT, this);

            if (_onSeabed)
            {
                // Logging.Warn(Tags.AIRCRAFT, $"{GetInstanceID()}  Should not be called when already on seabed :/");
                return;
            }

            // Freeze unit
            rigidBody.bodyType = RigidbodyType2D.Kinematic;
            rigidBody.velocity = new Vector2(0, 0);

            // Move unit below seabed collider, so it does not recollide in subsequent frames.
            Vector3 currentPosition = rigidBody.position;
            rigidBody.position = new Vector3(currentPosition.x, SEABED_SAFE_POSITION_Y, currentPosition.z);

            _factoryProvider.DeferrerProvider.Deferrer.Defer(((IPvPRemovable)this).RemoveFromScene, seabedParkTimeInS);
        }
    }
}
