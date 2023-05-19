using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.AudioSources;
// using BattleCruisers.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using System;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units
{
    public abstract class PvPUnit : PvPBuildable<PvPBuildableActivationArgs>, IPvPUnit
    {
        private IPvPAudioSource _coreEngineAudioSource;
        private PvPVolumeAwareAudioSource _engineAudioSource;

        [Header("Other")]
        public PvPUnitCategory category;
        public Rigidbody2D rigidBody;

        #region Properties
        public IPvPDroneConsumerProvider DroneConsumerProvider { set { _droneConsumerProvider = value; } }
        public PvPUnitCategory Category => category;
        public override Vector2 Velocity => rigidBody.velocity;
        public virtual bool IsUltra => false;

        public float maxVelocityInMPerS;
        public float MaxVelocityInMPerS => maxVelocityInMPerS;

        private PvPDirection _facingDirection;
        public PvPDirection FacingDirection
        {
            get { return _facingDirection; }
            set
            {
                _facingDirection = value;
                OnDirectionChange();
            }
        }
        #endregion Properties

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            //Assert.IsTrue(maxVelocityInMPerS > 0);

            AudioSource engineAudioSource = transform.FindNamedComponent<AudioSource>("EngineAudioSource");
            Assert.IsNotNull(engineAudioSource);
            Assert.IsNotNull(engineAudioSource.clip);
            _coreEngineAudioSource = new PvPAudioSourceBC(engineAudioSource);

            Name = _commonStrings.GetString($"Buildables/Units/{stringKeyName}Name");
            Description = _commonStrings.GetString($"Buildables/Units/{stringKeyName}Description");
        }

        public override void Initialise(IPvPFactoryProvider factoryProvider)
        {
            base.Initialise(factoryProvider);

            // _engineAudioSource = new PvPEffectVolumeAudioSource(_coreEngineAudioSource, factoryProvider.SettingsManager, 2);
        }

        public override void Activate(PvPBuildableActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

            FacingDirection = ParentCruiser.Direction;

            HealthBar.IsVisible = true;

            // Disable gravity
            rigidBody.bodyType = RigidbodyType2D.Kinematic;
            rigidBody.gravityScale = 0;
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
            // _uiManager.ShowUnitDetails(this);
        }

        protected virtual void OnDirectionChange()
        {
            int yRotation = FindYRotation(FacingDirection);
            Quaternion rotation = gameObject.transform.rotation;
            rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, yRotation, rotation.eulerAngles.z);
            gameObject.transform.rotation = rotation;
        }

        private int FindYRotation(PvPDirection facingDirection)
        {
            switch (facingDirection)
            {
                case PvPDirection.Right:
                    // Sprites by default are facing right, so DO NOT mirror
                    return 0;
                case PvPDirection.Left:
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

        void IPvPRemovable.RemoveFromScene()
        {
            // Logging.Log(Tags.BUILDABLE, this);
            base.InternalDestroy();
        }

        public void AddBuildRateBoostProviders(ObservableCollection<IPvPBoostProvider> boostProviders)
        {
            _buildRateBoostableGroup.AddBoostProvidersList(boostProviders);
        }
    }
}
