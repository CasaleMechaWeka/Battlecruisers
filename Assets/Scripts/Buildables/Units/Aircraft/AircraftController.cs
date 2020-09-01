using BattleCruisers.Buildables.Pools;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.BattleScene.Seabed;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public abstract class AircraftController : Unit, IVelocityProvider, IPatrollingVelocityProvider, ISeabedImpactable
    {
        private KamikazeController _kamikazeController;
        private Collider2D _collider;
		private SpriteRenderer _spriteRenderer;
        private IBoostable _velocityBoostable;
        private float _fuzziedMaxVelocityInMPerS;
        private TrailRenderer _aircraftTrail;
        private bool _onSeabed;

        protected ISpriteChooser _spriteChooser;

        public float cruisingAltitudeInM;
        public float seabedParkTimeInS = 10;

        private const float MAX_VELOCITY_FUZZING_PROPORTION = 0.1f;
        private const float ON_DEATH_GRAVITY_SCALE = 0.4f;
        private const float SEABED_SAFE_POSITION_Y = -40;

        protected bool IsInKamikazeMode => _kamikazeController.isActiveAndEnabled;
        public override TargetType TargetType => TargetType.Aircraft;
		public override Vector2 Velocity => ActiveMovementController.Velocity;
        protected virtual float MaxPatrollingVelocity => EffectiveMaxVelocityInMPerS;
        protected float EffectiveMaxVelocityInMPerS => _velocityBoostable.BoostMultiplier * _fuzziedMaxVelocityInMPerS;
		public float PatrollingVelocityInMPerS => MaxPatrollingVelocity;
        public float VelocityInMPerS => EffectiveMaxVelocityInMPerS;
        protected virtual float PositionEqualityMarginInM => 0.5f;
        protected override bool ShowSmokeWhenDestroyed => true;

        protected IMovementController DummyMovementController { get; private set; }
        protected IMovementController PatrollingMovementController { get; private set; }

        private IMovementController _activeMovementController;
        protected IMovementController ActiveMovementController
        {
            get { return _activeMovementController; }
            set
            {
                Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}  {ActiveMovementController}  => {value}");
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

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            _kamikazeController = GetComponentInChildren<KamikazeController>(includeInactive: true);
            Assert.IsNotNull(_kamikazeController);
            Assert.IsFalse(IsInKamikazeMode);

            _collider = GetComponent<Collider2D>();
            Assert.IsNotNull(_collider);

            _spriteRenderer = GetComponentInChildren<SpriteRenderer>(includeInactive: true);
            Assert.IsNotNull(_spriteRenderer);

            _aircraftTrail = transform.FindNamedComponent<TrailRenderer>("AircraftTrail");
        }

        public override void Initialise(IUIManager uiManager, IFactoryProvider factoryProvider)
        {
            base.Initialise(uiManager, factoryProvider);

            _velocityBoostable = _factoryProvider.BoostFactory.CreateBoostable();
            _fuzziedMaxVelocityInMPerS = RandomGenerator.Instance.Randomise(maxVelocityInMPerS, MAX_VELOCITY_FUZZING_PROPORTION, ChangeDirection.Both);
            DummyMovementController = _movementControllerFactory.CreateDummyMovementController();
        }

        public override void Activate(UnitActivationArgs activationArgs)
        {
            Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}");

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
            IGlobalBoostProviders globalBoostProviders, 
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(globalBoostProviders.UnitBuildRate.AircraftProviders);
        }

        protected abstract IList<IPatrolPoint> GetPatrolPoints();

		private void _movementController_DirectionChanged(object sender, XDirectionChangeEventArgs e)
		{
			FacingDirection = e.NewDirection;
		}

		protected override void OnFixedUpdate()
		{
			base.OnFixedUpdate();

            Logging.Verbose(Tags.AIRCRAFT, $"{GetInstanceID()}  Adjusting velocity");


            Assert.IsNotNull(ActiveMovementController, "OnInitialised() should always be called before OnFixedUpdate()");
			ActiveMovementController.AdjustVelocity();

            _spriteRenderer.sprite = _spriteChooser.ChooseSprite(Velocity).Sprite;
		}

        public void Kamikaze(ITarget kamikazeTarget)
        {
			Assert.AreEqual(UnitCategory.Aircraft, Category, "Only aircraft should kamikaze");
            Assert.AreEqual(BuildableState.Completed, BuildableState, "Only completed aircraft should kamikaze.");

            if (IsInKamikazeMode)
            {
                // Already in kamikaze mode, no need to do anything again :)
                return;
            }

            ITargetProvider cruiserTarget = _cruiserSpecificFactories.Targets.ProviderFactory.CreateStaticTargetProvider(kamikazeTarget);
            ActiveMovementController = _movementControllerFactory.CreateHomingMovementController(rigidBody, this, cruiserTarget);

            UpdateFaction(kamikazeTarget);

            OnKamikaze();
        }

        private void UpdateFaction(ITarget kamikazeTarget)
        {
			Faction = Helper.GetOppositeFaction(kamikazeTarget.Faction);

            // Make our collider be lost and refound by all target detectors.
            // Means target detectors that we are already in range of can 
            // re-evaluate whether we are a target, as our faction has just changed.
            _collider.enabled = false;
            _collider.enabled = true;

            _kamikazeController.Initialise(this, _factoryProvider, kamikazeTarget);
			_kamikazeController.gameObject.SetActive(true);
        }

        protected IList<IPatrolPoint> ProcessPatrolPoints(IList<Vector2> patrolPositions, Action onFirstPatrolPointReached)
        {
			IList<IPatrolPoint> patrolPoints = new List<IPatrolPoint>(patrolPositions.Count);

			patrolPoints.Add(new PatrolPoint(patrolPositions[0], removeOnceReached: false, actionOnReached: onFirstPatrolPointReached));

            for (int i = 1; i < patrolPositions.Count; ++i)
            {
                patrolPoints.Add(new PatrolPoint(patrolPositions[i]));
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
            Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}");

            base.OnDestroyed();

            _localBoosterBoostableGroup.BoostChanged -= _boostableGroup_BoostChanged;

            if (BuildableState == BuildableState.Completed
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
            Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}");
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
            Logging.Log(Tags.AIRCRAFT, this);

            if (_onSeabed)
            {
                Logging.Warn(Tags.AIRCRAFT, $"{GetInstanceID()}  Should not be called when already on seabed :/");
                return;
            }

            // Freeze unit
            rigidBody.bodyType = RigidbodyType2D.Kinematic;
            rigidBody.velocity = new Vector2(0, 0);

            // Move unit below seabed collider, so it does not recollide in subsequent frames.
            Vector3 currentPosition = rigidBody.transform.position;
            rigidBody.transform.position = new Vector3(currentPosition.x, SEABED_SAFE_POSITION_Y, currentPosition.z);

            _factoryProvider.DeferrerProvider.Deferrer.Defer(((IRemovable)this).RemoveFromScene, seabedParkTimeInS);
        }
    }
}
