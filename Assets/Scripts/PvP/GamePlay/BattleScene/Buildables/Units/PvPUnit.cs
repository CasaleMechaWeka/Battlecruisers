using BattleCruisers.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using Unity.Netcode;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Data.Static;
using BattleCruisers.Data;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.UI.Sound.AudioSources;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units
{
    public abstract class PvPUnit : PvPBuildable<PvPBuildableActivationArgs>, IPvPUnit
    {
        private IAudioSource _coreEngineAudioSource;
        private VolumeAwareAudioSource _engineAudioSource;

        [Header("Other")]
        public UnitCategory category;
        public Rigidbody2D rigidBody;

        #region Properties
        public IDroneConsumerProvider DroneConsumerProvider { set { _droneConsumerProvider = value; } }
        public UnitCategory Category => category;
        public override Vector2 Velocity => rigidBody.velocity;
        public virtual bool IsUltra => false;

        public float maxVelocityInMPerS;
        public float MaxVelocityInMPerS => maxVelocityInMPerS;

        public virtual float YSpawnOffset => 0.0f; // Default value for non-ship units

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

        private NetworkVariable<int> pvp_variantIndex = new NetworkVariable<int>();
        private bool isAppliedVariant = false;
        public int variantIndex
        {
            get { return pvp_variantIndex.Value; }
            set
            {
                if (IsHost)
                    pvp_variantIndex.Value = value;
            }
        }

        #endregion Properties

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            //Assert.IsTrue(maxVelocityInMPerS > 0);

            AudioSource engineAudioSource = transform.FindNamedComponent<AudioSource>("EngineAudioSource");
            Assert.IsNotNull(engineAudioSource);
            Assert.IsNotNull(engineAudioSource.clip);
            _coreEngineAudioSource = new AudioSourceBC(engineAudioSource);

            Name = LocTableCache.CommonTable.GetString($"Buildables/Units/{stringKeyName}Name");
            Description = LocTableCache.CommonTable.GetString($"Buildables/Units/{stringKeyName}Description");
        }

        private async void ApplyVariantIconOnClient(int oldVal, int newVal)
        {
            if (newVal != -1)
            {
                VariantPrefab variant = await _factoryProvider.PrefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(newVal));
                HealthBar.variantIcon.sprite = variant.variantSprite;
                HealthBar.variantIcon.color = new Color(HealthBar.variantIcon.color.r, HealthBar.variantIcon.color.g, HealthBar.variantIcon.color.b, 1f);
                HealthBar.variantIcon.enabled = true;
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (!IsHost)
                pvp_variantIndex.OnValueChanged += ApplyVariantIconOnClient;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (!IsHost)
                pvp_variantIndex.OnValueChanged -= ApplyVariantIconOnClient;
        }

        public override void Initialise(IPvPFactoryProvider factoryProvider)
        {
            base.Initialise(factoryProvider);

            // _engineAudioSource = new EffectVolumeAudioSource(_coreEngineAudioSource, factoryProvider.SettingsManager, 2);
        }

        public override void Initialise(IPvPFactoryProvider factoryProvider, IPvPUIManager uiManager)
        {
            base.Initialise(factoryProvider, uiManager);
            _engineAudioSource = new EffectVolumeAudioSource(_coreEngineAudioSource, factoryProvider.SettingsManager, 2);
        }

        public override void Activate(PvPBuildableActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

            FacingDirection = ParentCruiser.Direction;

            HealthBar.IsVisible = true;

            // Disable gravity
            rigidBody.bodyType = RigidbodyType2D.Kinematic;
            rigidBody.gravityScale = 0;

            variantIndex = activationArgs.VariantIndex;
            HealthBar.variantIcon.color = new Color(HealthBar.variantIcon.color.r, HealthBar.variantIcon.color.g, HealthBar.variantIcon.color.b, 0f);
            HealthBar.variantIcon.enabled = false;

            if (!isAppliedVariant)
            {
                ApplyVariantPvP(this, variantIndex);
                isAppliedVariant = true;
            }
            else
            {
                if (variantIndex != -1)
                {
                    HealthBar.variantIcon.color = new Color(HealthBar.variantIcon.color.r, HealthBar.variantIcon.color.g, HealthBar.variantIcon.color.b, 1f);
                    HealthBar.variantIcon.enabled = true;
                }
            }
        }

        private async void ApplyVariantPvP(IPvPUnit unit, int varint_index)
        {
            DataProvider dataProvider = ApplicationModelProvider.ApplicationModel.DataProvider;
            if (varint_index != -1)
            {
                VariantPrefab variant = await _factoryProvider.PrefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(varint_index));
                if (variant != null)
                {
                    HealthBar.variantIcon.sprite = variant.variantSprite;
                    HealthBar.variantIcon.enabled = true;
                    HealthBar.variantIcon.color = new Color(HealthBar.variantIcon.color.r, HealthBar.variantIcon.color.g, HealthBar.variantIcon.color.b, 1f);
                    Name = LocTableCache.CommonTable.GetString(dataProvider.StaticData.Variants[varint_index].VariantNameStringKeyBase);
                    Description = LocTableCache.CommonTable.GetString(dataProvider.StaticData.Variants[varint_index].VariantDescriptionStringKeyBase);
                    ApplyVariantStats(variant.statVariant);
                }
                else
                {
                    HealthBar.variantIcon.sprite = null;
                    HealthBar.variantIcon.color = new Color(HealthBar.variantIcon.color.r, HealthBar.variantIcon.color.g, HealthBar.variantIcon.color.b, 0f);
                    HealthBar.variantIcon.enabled = false;
                }
            }
        }

        public void ApplyVariantStats(StatVariant statVariant)
        {
            maxHealth *= statVariant.max_health;
            pvp_Health.Value = maxHealth;
            numOfDronesRequired += statVariant.drone_num;
            buildTimeInS += statVariant.build_time;

            _healthTracker.OverrideHealth(maxHealth);
            _healthTracker.OverrideMaxHealth(maxHealth);
            _buildTimeInDroneSeconds = numOfDronesRequired * buildTimeInS;
            HealthGainPerDroneS = maxHealth / _buildTimeInDroneSeconds;

            HealthBar.OverrideHealth(this);
        }

        public override void Activate_PvPClient()
        {
            // Disable gravity
            rigidBody.bodyType = RigidbodyType2D.Kinematic;
            rigidBody.gravityScale = 0;
            base.Activate_PvPClient();
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();
            _smokeInitialiser.Initialise(this, ShowSmokeWhenDestroyed);
        }
        protected override void OnBuildableCompleted_PvPClient()
        {
            _coreEngineAudioSource.Play(isSpatial: true, loop: true);
            base.OnBuildableCompleted_PvPClient();
            _smokeInitialiser.Initialise(this, ShowSmokeWhenDestroyed);
        }


        protected virtual void FixedUpdate()
        {
            if (!IsHost)
                return;
            if (!IsDestroyed)
            {
                //if (!isUpdating)
                //{
                //    isUpdating = true;
                //    StartCoroutine(iOnFixedUpdte());
                //}
                OnFixedUpdate();
            }
        }
        IEnumerator iOnFixedUpdte()
        {
            yield return null;
            OnFixedUpdate();
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
            //    _coreEngineAudioSource.Stop();
        }

        protected override void OnDestroyedEvent()
        {
            base.OnDestroyedEvent();
            if (IsClient)
                _coreEngineAudioSource.Stop();
            if (ShouldShowDeathEffects())
            {
                ShowDeathEffects();
            }
        }

        protected override void InternalDestroy()
        {
            // Logging.Log(Tags.BUILDABLE, this);

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
            return BuildableState == PvPBuildableState.Completed;
        }

        protected abstract void ShowDeathEffects();

        public void RemoveFromScene()
        {
            // Logging.Log(Tags.BUILDABLE, this);
            base.InternalDestroy();
        }

        public void AddBuildRateBoostProviders(ObservableCollection<IBoostProvider> boostProviders)
        {
            _buildRateBoostableGroup.AddBoostProvidersList(boostProviders);
        }
    }
}
