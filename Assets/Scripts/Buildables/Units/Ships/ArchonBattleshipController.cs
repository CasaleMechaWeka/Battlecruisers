using BattleCruisers.Buildables.Pools;
using BattleCruisers.Effects;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Ships
{
    public class ArchonBattleshipController : ShipController
    {
        private IBroadcastingAnimation _unfurlAnimation;
        private AudioSourceGroup _unfurlAudioGroup;

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

            Helper.AssertIsNotNull(bones, bellowAudioSource, crankAudioSource, chainAudioSource, dieselAudioSource);

            _unfurlAnimation = bones.GetComponent<IBroadcastingAnimation>();
            Assert.IsNotNull(_unfurlAnimation);
            _unfurlAnimation.AnimationDone += _unfurlAnimation_AnimationDone;

            TargetProxy[] colliderTargetProxies = GetComponentsInChildren<TargetProxy>(includeInactive: true);
            foreach (TargetProxy targetProxy in colliderTargetProxies)
            {
                targetProxy.Initialise(this);
            }
        }

        public override void Initialise(UIManager uiManager)
        {
            base.Initialise(uiManager);

            _unfurlAudioGroup
                = new AudioSourceGroup(
                    FactoryProvider.SettingsManager,
                    new AudioSourceBC(bellowAudioSource),
                    new AudioSourceBC(crankAudioSource),
                    new AudioSourceBC(chainAudioSource),
                    new AudioSourceBC(dieselAudioSource));
        }

        public override void Activate(BuildableActivationArgs activationArgs)
        {
            //Debug.Log("Done 4 head");
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

        protected override void Deactivate()
        {
            base.Deactivate();
            bones.SetActive(false);
        }
    }
}
