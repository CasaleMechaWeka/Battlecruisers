using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Buildables.Pools;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Static;
using BattleCruisers.Effects;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class HuntressUnit : ShipController
    {
        private IBroadcastingAnimation _unfurlAnimation;
        private AudioSourceGroup _unfurlAudioGroup;
        public AudioSource[] audioSources;
        public BarrelWrapper minigun, _samSite;
        public ProjectileStats minigunStats;
        public GameObject bones;
        //public AudioSource bellowAudioSource, crankAudioSource, chainAudioSource, dieselAudioSource;

        public override bool IsUltra => true;
        public override Vector2 Size => base.Size * 2;

        public override Vector2 DroneAreaSize => base.Size;
        public override TargetType TargetType => TargetType.Cruiser;
        public Vector2 droneAreaPositionAdjustment;
        public override Vector2 DroneAreaPosition => FacingDirection == Direction.Right ? Position + droneAreaPositionAdjustment : Position - droneAreaPositionAdjustment;
        public Animator bonesAnimator;
        private float animationSpeed = 1.0f;
        
        public event EventHandler RearingStarted;

        private IDamageApplier _areaDamageApplier;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            //Helper.AssertIsNotNull(bones, laser, bellowAudioSource, crankAudioSource, chainAudioSource, dieselAudioSource);

            _unfurlAnimation = bones.GetComponent<IBroadcastingAnimation>();

            Assert.IsNotNull(_unfurlAnimation);
            _unfurlAnimation.AnimationDone += _unfurlAnimation_AnimationDone;
            _unfurlAnimation.AnimationStarted += _unfurlAnimation_AnimationStarted;

            TargetProxy[] colliderTargetProxies = GetComponentsInChildren<TargetProxy>(includeInactive: true);
            foreach (TargetProxy targetProxy in colliderTargetProxies)
            {
                targetProxy.Initialise(this);
            }
            
        }

        public override void Initialise(IUIManager uiManager, IFactoryProvider factoryProvider)
        {
            base.Initialise(uiManager, factoryProvider);
            AudioSourceBC[] sources = new AudioSourceBC[audioSources.Length];
            for (int i = 0; i < sources.Length; i++)
            {
                sources[i] = new AudioSourceBC(audioSources[i]);
            }
            _unfurlAudioGroup
                = new AudioSourceGroup(
                    factoryProvider.SettingsManager,
                    sources);
        }

        public override void Activate(ICruiser parentCruiser, ICruiser enemyCruiser, ICruiserSpecificFactories cruiserSpecificFactories)
        {
            base.Activate(parentCruiser, enemyCruiser, cruiserSpecificFactories);
        }

        protected override void OnShipCompleted()
        {
            // Show bones, starting unfurl animation
            //bones.SetActive(true);
            // Delay normal setup (movement, turrets) until the unfurl animation has completed
        }

        private void CompleteShip()
        {
            base.OnShipCompleted();
        }

        private void _unfurlAnimation_AnimationDone(object sender, EventArgs e)
        {
            base.OnShipCompleted();
            //Debug.Log("wow!");
        }

        private void _unfurlAnimation_AnimationStarted(object sender, EventArgs e)
        {
            Vector2 collisionPoint = new Vector2(0,0);
            IDamageStats damageStats = new DamageStats(2000, 25);
            ITargetFilter targetFilter = new DummyTargetFilter(isMatchResult: true);

            _areaDamageApplier = new AreaOfEffectDamageApplier(damageStats, targetFilter);
                _areaDamageApplier
                    .ApplyDamage(
                    target: null,
                    collisionPoint: collisionPoint,
                    damageSource: null);

            RearingStarted?.Invoke(this, EventArgs.Empty);
        }

        protected override void AddBuildRateBoostProviders(
            IGlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.UltrasProviders);
        }

        public override float OptimalArmamentRangeInM
        {
            get
            {
                return minigun.RangeInM;
            }
        }

        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Ultra;

        protected override IList<IBarrelWrapper> GetTurrets()
        {
            return new List<IBarrelWrapper>()
            { 
                minigun, _samSite
            };
        }

        protected override void InitialiseTurrets()
        {
            minigun.Initialise(this, _factoryProvider, _cruiserSpecificFactories, SoundKeys.Firing.AttackBoat);
            _samSite.Initialise(this, _factoryProvider, _cruiserSpecificFactories, SoundKeys.Firing.Missile);
        }

        protected override List<SpriteRenderer> GetNonTurretRenderers()
        {
            List<SpriteRenderer> renderers = base.GetNonTurretRenderers();

            Transform pistonsParent = transform.FindNamedComponent<Transform>("HuntressBones");
            SpriteRenderer[] boneRenderers = pistonsParent.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);

            foreach (SpriteRenderer renderer in boneRenderers)
            {
                // Only add enabled renderers, which excludes guide sprites
                if (renderer.enabled)
                {
                    renderers.Add(renderer);
                }
            }

            return renderers;
        }

        protected override void Deactivate()
        {
            base.Deactivate();
            bones.SetActive(false);
        }
        
        protected override void OnTakeDamage()
        {
            SpeedUpAnimation();
        }

        public void SpeedUpAnimation()
        {
            animationSpeed += 1.0f;
            animationSpeed = Math.Max(animationSpeed, 8f);
            bonesAnimator.SetFloat("SpeedMultiplier", animationSpeed);
        }
    }
}
