using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Buildables.Pools;
using BattleCruisers.Data.Static;
using BattleCruisers.Effects;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class ArchonBattleshipController : ShipController
    {
        private IBroadcastingAnimation _unfurlAnimation;
        private AudioSourceGroup _unfurlAudioGroup;

        public BarrelWrapper laser;
        public GameObject bones;
        public AudioSource bellowAudioSource, crankAudioSource, chainAudioSource, dieselAudioSource;

        public override bool IsUltra => true;
        public override Vector2 Size => base.Size * 2;

        public override Vector2 DroneAreaSize => base.Size;

        public Vector2 droneAreaPositionAdjustment;
        public override Vector2 DroneAreaPosition => FacingDirection == Direction.Right ? Position + droneAreaPositionAdjustment : Position - droneAreaPositionAdjustment;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            Helper.AssertIsNotNull(bones, laser, bellowAudioSource, crankAudioSource, chainAudioSource, dieselAudioSource);

            _unfurlAnimation = bones.GetComponent<IBroadcastingAnimation>();
            Assert.IsNotNull(_unfurlAnimation);
            _unfurlAnimation.AnimationDone += _unfurlAnimation_AnimationDone;

            TargetProxy[] colliderTargetProxies = GetComponentsInChildren<TargetProxy>(includeInactive: true);
            foreach (TargetProxy targetProxy in colliderTargetProxies)
            {
                targetProxy.Initialise(this);
            }
        }

        public override void Initialise(IUIManager uiManager, IFactoryProvider factoryProvider)
        {
            base.Initialise(uiManager, factoryProvider);

            _unfurlAudioGroup
                = new AudioSourceGroup(
                    factoryProvider.SettingsManager,
                    new AudioSourceBC(bellowAudioSource),
                    new AudioSourceBC(crankAudioSource),
                    new AudioSourceBC(chainAudioSource),
                    new AudioSourceBC(dieselAudioSource));
        }

        public override void Activate(BuildableActivationArgs activationArgs)
        {
            base.Activate(activationArgs);
        }

        protected override void OnShipCompleted()
        {
            // Show bones, starting unfurl animation
            bones.SetActive(true);

            // Delay normal setup (movement, turrets) until the unfurl animation has completed
        }

        private void _unfurlAnimation_AnimationDone(object sender, EventArgs e)
        {
            base.OnShipCompleted();
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
                return laser.RangeInM;
            }
        }

        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Ultra;

        protected override IList<IBarrelWrapper> GetTurrets()
        {
            return new List<IBarrelWrapper>()
            { 
                laser 
            };
        }

        protected override void InitialiseTurrets()
        {
            Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            laser.Initialise(this, _factoryProvider, _cruiserSpecificFactories, enemyFaction);
        }

        protected override List<SpriteRenderer> GetNonTurretRenderers()
        {
            List<SpriteRenderer> renderers = base.GetNonTurretRenderers();

            Transform pistonsParent = transform.FindNamedComponent<Transform>("UnitBones");
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
    }
}
