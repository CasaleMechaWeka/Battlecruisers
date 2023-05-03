using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.DamageAppliers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.AudioSources;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships
{
    public class PvPHuntressUnit : PvPShipController
    {
        private IPvPBroadcastingAnimation _unfurlAnimation;
        private PvPAudioSourceGroup _unfurlAudioGroup;
        public AudioSource[] audioSources;
        public PvPBarrelWrapper minigun, _samSite;
        public PvPProjectileStats minigunStats;
        public GameObject bones;
        //public AudioSource bellowAudioSource, crankAudioSource, chainAudioSource, dieselAudioSource;

        public override bool IsUltra => true;
        public override Vector2 Size => base.Size * 2;

        public override Vector2 DroneAreaSize => base.Size;
        public override PvPTargetType TargetType => PvPTargetType.Cruiser;
        public Vector2 droneAreaPositionAdjustment;
        public override Vector2 DroneAreaPosition => FacingDirection == PvPDirection.Right ? Position + droneAreaPositionAdjustment : Position - droneAreaPositionAdjustment;
        public Animator bonesAnimator;
        private float animationSpeed = 1.0f;

        public event EventHandler RearingStarted;

        private IPvPDamageApplier _areaDamageApplier;

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            //Helper.AssertIsNotNull(bones, laser, bellowAudioSource, crankAudioSource, chainAudioSource, dieselAudioSource);

            _unfurlAnimation = bones.GetComponent<IPvPBroadcastingAnimation>();

            Assert.IsNotNull(_unfurlAnimation);
            _unfurlAnimation.AnimationDone += _unfurlAnimation_AnimationDone;
            _unfurlAnimation.AnimationStarted += _unfurlAnimation_AnimationStarted;

            PvPTargetProxy[] colliderTargetProxies = GetComponentsInChildren<PvPTargetProxy>(includeInactive: true);
            foreach (PvPTargetProxy targetProxy in colliderTargetProxies)
            {
                targetProxy.Initialise(this);
            }

        }

        public override void Initialise(IPvPUIManager uiManager, IPvPFactoryProvider factoryProvider)
        {
            base.Initialise(uiManager, factoryProvider);
            PvPAudioSourceBC[] sources = new PvPAudioSourceBC[audioSources.Length];
            for (int i = 0; i < sources.Length; i++)
            {
                sources[i] = new PvPAudioSourceBC(audioSources[i]);
            }
            _unfurlAudioGroup
                = new PvPAudioSourceGroup(
                    factoryProvider.SettingsManager,
                    sources);
        }

        public override void Activate(IPvPCruiser parentCruiser, IPvPCruiser enemyCruiser, IPvPCruiserSpecificFactories cruiserSpecificFactories)
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
            Vector2 collisionPoint = new Vector2(0, 0);
            IPvPDamageStats damageStats = new PvPDamageStats(2000, 25);
            IPvPTargetFilter targetFilter = new PvPDummyTargetFilter(isMatchResult: true);

            _areaDamageApplier = new PvPAreaOfEffectDamageApplier(damageStats, targetFilter);
            _areaDamageApplier
                .ApplyDamage(
                target: null,
                collisionPoint: collisionPoint,
                damageSource: null);

            RearingStarted?.Invoke(this, EventArgs.Empty);
        }

        protected override void AddBuildRateBoostProviders(
            IPvPGlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IPvPBoostProvider>> buildRateBoostProvidersList)
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

        protected override PvPPrioritisedSoundKey ConstructionCompletedSoundKey => PvPPrioritisedSoundKeys.PvPCompleted.Ultra;

        protected override IList<IPvPBarrelWrapper> GetTurrets()
        {
            return new List<IPvPBarrelWrapper>()
            {
                minigun, _samSite
            };
        }

        protected override void InitialiseTurrets()
        {
            minigun.Initialise(this, _factoryProvider, _cruiserSpecificFactories, PvPSoundKeys.PvPFiring.AttackBoat);
            _samSite.Initialise(this, _factoryProvider, _cruiserSpecificFactories, PvPSoundKeys.PvPFiring.Missile);
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
