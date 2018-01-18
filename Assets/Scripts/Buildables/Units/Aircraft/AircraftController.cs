using System;
using System.Collections.Generic;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Data.Static;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public abstract class AircraftController : Unit, IVelocityProvider
	{
        private KamikazeController _kamikazeController;
		private SpriteRenderer _spriteRenderer;
        private IBoostable _velocityBoostable;
        protected ISpriteChooser _spriteChooser;

        public float cruisingAltitudeInM;

        protected IMovementController ActiveMovementController { get; private set; }
        protected IMovementController DummyMovementController { get; private set; }
        protected IMovementController PatrollingMovementController { get; private set; }

        protected bool IsInKamikazeMode { get { return _kamikazeController.isActiveAndEnabled; } }
        public override TargetType TargetType { get { return TargetType.Aircraft; } }
		public override Vector2 Velocity { get { return ActiveMovementController.Velocity; } }
        protected virtual float MaxPatrollingVelocity { get { return EffectiveMaxVelocityInMPerS; } }
        protected float EffectiveMaxVelocityInMPerS { get { return _velocityBoostable.BoostMultiplier * maxVelocityInMPerS; } }
        public float VelocityInMPerS { get { return EffectiveMaxVelocityInMPerS; } }
        protected virtual float PositionEqualityMarginInM { get { return 0.5f; } }
        protected override ISoundKey DeathSoundKey { get { return SoundKeys.Deaths.Aircraft; } }

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            _kamikazeController = GetComponentInChildren<KamikazeController>(includeInactive: true);
            Assert.IsNotNull(_kamikazeController);
            Assert.IsFalse(IsInKamikazeMode);

            _spriteRenderer = GetComponentInChildren<SpriteRenderer>(includeInactive: true);
            Assert.IsNotNull(_spriteRenderer);
        }

		protected override void OnInitialised()
		{
			base.OnInitialised();

            _velocityBoostable = _factoryProvider.BoostFactory.CreateBoostable();
            _boostableGroup.AddBoostable(_velocityBoostable);
            _boostableGroup.AddBoostProvidersList(_factoryProvider.BoostProvidersManager.AircraftBoostProviders);
            _boostableGroup.BoostChanged += _boostableGroup_BoostChanged;

			DummyMovementController = _movementControllerFactory.CreateDummyMovementController();
			PatrollingMovementController 
                = _movementControllerFactory.CreatePatrollingMovementController(
                    rigidBody, 
                    maxVelocityProvider: this,
                    patrolPoints: GetPatrolPoints(),
                    positionEqualityMarginInM: PositionEqualityMarginInM);

			SwitchMovementControllers(DummyMovementController);

            _spriteChooser = _factoryProvider.SpriteChooserFactory.CreateDummySpriteChooser(_spriteRenderer.sprite);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

            SwitchMovementControllers(PatrollingMovementController);
		}

		protected abstract IList<IPatrolPoint> GetPatrolPoints();

		private void _movementController_DirectionChanged(object sender, XDirectionChangeEventArgs e)
		{
			FacingDirection = e.NewDirection;
		}

		protected override void OnFixedUpdate()
		{
			base.OnFixedUpdate();

			Assert.IsNotNull(ActiveMovementController, "OnInitialised() should always be called before OnFixedUpdate()");
			ActiveMovementController.AdjustVelocity();

            _spriteRenderer.sprite = _spriteChooser.ChooseSprite(Velocity).Sprite;
		}

		protected void SwitchMovementControllers(IMovementController newMovementController)
		{
            Logging.Log(Tags.AIRCRAFT, "SwitchMovementControllers: " + ActiveMovementController + " => " + newMovementController);

            if (ActiveMovementController != null)
            {
                newMovementController.Velocity = ActiveMovementController.Velocity;
                ActiveMovementController.DirectionChanged -= _movementController_DirectionChanged;
			}

			ActiveMovementController = newMovementController;
			ActiveMovementController.DirectionChanged += _movementController_DirectionChanged;
            ActiveMovementController.Activate();
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

            ITargetProvider cruiserTarget = _targetsFactory.CreateStaticTargetProvider(kamikazeTarget);
            SwitchMovementControllers(_movementControllerFactory.CreateHomingMovementController(rigidBody, this, cruiserTarget));

            UpdateFaction(kamikazeTarget);

            OnKamikaze();
        }

        private void UpdateFaction(ITarget kamikazeTarget)
        {
			Faction = Helper.GetOppositeFaction(kamikazeTarget.Faction);

			// Make our collider be lost and refound by all target detectors.
			// Means target detectors that we are already in range of can 
			// re-evaluate whether we are a target, as our faction has just changed.
			gameObject.SetActive(false);
			gameObject.SetActive(true);

            // Restart engine sound, which gets paused when we set the game
            // object to inactive.
            PlayEngineSound();

			_kamikazeController.Initialise(this, _factoryProvider, kamikazeTarget);
			_kamikazeController.gameObject.SetActive(true);
        }

        protected virtual void OnKamikaze() { }

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
            base.OnDestroyed();
            _boostableGroup.BoostChanged -= _boostableGroup_BoostChanged;
        }
    }
}
