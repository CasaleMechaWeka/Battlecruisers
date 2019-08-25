using BattleCruisers.Buildables.ActivationArgs;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units
{
    public abstract class Unit : Buildable<BuildableActivationArgs>, IUnit
    {
        private IAudioClipWrapper _engineAudioClip;

		public UnitCategory category;

		public float maxVelocityInMPerS;
        public float MaxVelocityInMPerS => maxVelocityInMPerS;

		public Rigidbody2D rigidBody;

		#region Properties
		public IDroneConsumerProvider DroneConsumerProvider	{ set { _droneConsumerProvider = value;	} }
		public UnitCategory Category => category;
		public override Vector2 Velocity => rigidBody.velocity;
        public virtual bool IsUltra => false;

		private Direction _facingDirection;
		public Direction FacingDirection
		{
			get { return _facingDirection; }
            protected set
			{
				_facingDirection = value;
				OnDirectionChange();
			}
		}

        protected override bool IsDroneConsumerFocusable => false;

        protected abstract ISoundKey EngineSoundKey { get; }
        protected virtual float OnDeathGravityScale => 1;
		#endregion Properties

		public override void Initialise(IUIManager uiManager, IFactoryProvider factoryProvider)
		{
            base.Initialise(uiManager, factoryProvider);
			
            Assert.IsTrue(maxVelocityInMPerS > 0);
            _engineAudioClip = _factoryProvider.Sound.SoundFetcher.GetSound(EngineSoundKey);
        }

        public override void Activate(BuildableActivationArgs activationArgs)
        {
            base.Activate(activationArgs);
			FacingDirection = ParentCruiser.Direction;
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();
            PlayEngineSound();
        }

        protected void PlayEngineSound()
        {
            _audioSource.AudioClip = _engineAudioClip;
            _audioSource.Play(isSpatial: true, loop: true);
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
            _audioSource.Stop();
        }

        protected override void InternalDestroy()
        {
            if (BuildableState == BuildableState.Completed)
            {
                OnDeathWhileCompleted();
            }
            else
            {
                base.InternalDestroy();
            }
        }

        protected virtual void OnDeathWhileCompleted()
        {
            // FELIX  Deactivate instead (need to activate in Activate() )
            Destroy(_healthBar.gameObject);

            // Make gravity take effect
            rigidBody.bodyType = RigidbodyType2D.Dynamic;
            rigidBody.gravityScale = OnDeathGravityScale;
        }

        void IRemovable.RemoveFromScene()
        {
            base.InternalDestroy();
        }

        public void AddBuildRateBoostProviders(ObservableCollection<IBoostProvider> boostProviders)
        {
            _buildRateBoostableGroup.AddBoostProvidersList(boostProviders);
        }
    }
}
