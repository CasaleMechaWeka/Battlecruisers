using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Pools;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;
using static BattleCruisers.Effects.Smoke.StaticSmokeStats;

namespace BattleCruisers.Buildables.Units
{
    public abstract class Unit : Buildable<BuildableActivationArgs>, IUnit
    {
        private IAudioSource _coreEngineAudioSource;
        private VolumeAwareAudioSource _engineAudioSource;

        [Header("Other")]
        public UnitCategory category;
		public Rigidbody2D rigidBody;

        #region Properties
        public IDroneConsumerProvider DroneConsumerProvider	{ set { _droneConsumerProvider = value;	} }
		public UnitCategory Category => category;
		public override Vector2 Velocity => rigidBody.velocity;
        public virtual bool IsUltra => false;

		public float maxVelocityInMPerS;
        public float MaxVelocityInMPerS => maxVelocityInMPerS;

        public int variantIndex { get; set; }

        private Direction _facingDirection;
		public Direction FacingDirection
		{
			get { return _facingDirection; }
            set
			{
				_facingDirection = value;
				OnDirectionChange();
			}
		}
        #endregion Properties

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            //Assert.IsTrue(maxVelocityInMPerS > 0);

            AudioSource engineAudioSource = transform.FindNamedComponent<AudioSource>("EngineAudioSource");
            Assert.IsNotNull(engineAudioSource);
            Assert.IsNotNull(engineAudioSource.clip);
            _coreEngineAudioSource = new AudioSourceBC(engineAudioSource);

            Name = _commonStrings.GetString($"Buildables/Units/{stringKeyName}Name");
            Description = _commonStrings.GetString($"Buildables/Units/{stringKeyName}Description");
            variantIndex = -1;
        }
        public void OverwriteComparableItem(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public override void Initialise(IUIManager uiManager, IFactoryProvider factoryProvider)
        {
            base.Initialise(uiManager, factoryProvider);

            _engineAudioSource = new EffectVolumeAudioSource(_coreEngineAudioSource, factoryProvider.SettingsManager, 2);
        }

        public override void Activate(BuildableActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

			FacingDirection = ParentCruiser.Direction;

            HealthBar.IsVisible = true;

            // Disable gravity
            rigidBody.bodyType = RigidbodyType2D.Kinematic;
            rigidBody.gravityScale = 0;

            HealthBar.variantIcon.enabled = false;
            if (ParentCruiser.IsPlayerCruiser)
            {
                // Set Variant to Player
                ApplyVariantToPlayer(this);
            }
            else
            {
                // Set Variant to AI

            }
        }

        private async void ApplyVariantToPlayer(IUnit unit)
        {
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            VariantPrefab variant = await applicationModel.DataProvider.GameModel.PlayerLoadout.GetSelectedUnitVariant(_factoryProvider.PrefabFactory, unit);
            if (variant != null)
            {
                // apply icon, name and description
                HealthBar.variantIcon.sprite = variant.variantSprite;
                HealthBar.variantIcon.enabled = true;
                int index = await applicationModel.DataProvider.GameModel.PlayerLoadout.GetSelectedUnitVariantIndex(_factoryProvider.PrefabFactory, unit);
                variantIndex = index;
                Name = _commonStrings.GetString(applicationModel.DataProvider.GameModel.Variants[index].VariantNameStringKeyBase);
                Description = _commonStrings.GetString(applicationModel.DataProvider.GameModel.Variants[index].VariantDescriptionStringKeyBase);

                // apply max health, num of drone required, build time
                ApplyVariantStats(variant.statVariant);
            }
            else
            {
                HealthBar.variantIcon.enabled = false;
                variantIndex = -1;
            }
        }

        public void ApplyVariantStats(StatVariant statVariant)
        {
            maxHealth += statVariant.max_health;
            numOfDronesRequired += statVariant.drone_num;
            buildTimeInS += statVariant.build_time;

            _healthTracker.OverrideHealth(maxHealth);
            _healthTracker.OverrideMaxHealth(maxHealth);
            _buildTimeInDroneSeconds = numOfDronesRequired * buildTimeInS;
            HealthGainPerDroneS = maxHealth / _buildTimeInDroneSeconds;

            HealthBar.OverrideHealth(this);
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();
            _coreEngineAudioSource.Play(isSpatial: true, loop: true);
        }

        void FixedUpdate()
		{
            if (!IsDestroyed)
            {
    			OnFixedUpdate();
            }
		}

		protected virtual void OnFixedUpdate() { }

		protected override void OnSingleClick()
		{
			_uiManager.ShowUnitDetails(this);
		}

		protected virtual void OnDirectionChange()
		{
			int yRotation = FindYRotation(FacingDirection);
			Quaternion rotation = gameObject.transform.rotation;
			rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, yRotation, rotation.eulerAngles.z);
			gameObject.transform.rotation = rotation;
		}

		private int FindYRotation(Direction facingDirection)
		{
			switch (facingDirection)
			{
				case Direction.Right:
					// Sprites by default are facing right, so DO NOT mirror
					return 0;
				case Direction.Left:
					// Sprites by default are facing right, so DO mirror
					return 180;
				default:
					throw new ArgumentException();
			}
		}

		protected override bool CanRepairCommandExecute()
		{
			// Cannot repair units :)
			return false;
		}

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
            _coreEngineAudioSource.Stop();
        }

        protected override void InternalDestroy()
        {
            Logging.Log(Tags.BUILDABLE, this);

            if (ShouldShowDeathEffects())
            {
                ShowDeathEffects();
            }
            else
            {
                base.InternalDestroy();
            }
        }

        protected virtual bool ShouldShowDeathEffects()
        {
            return BuildableState == BuildableState.Completed;
        }

        protected abstract void ShowDeathEffects();

        void IRemovable.RemoveFromScene()
        {
            Logging.Log(Tags.BUILDABLE, this);
            base.InternalDestroy();
        }

        public void AddBuildRateBoostProviders(ObservableCollection<IBoostProvider> boostProviders)
        {
            _buildRateBoostableGroup.AddBoostProvidersList(boostProviders);
        }
    }
}
