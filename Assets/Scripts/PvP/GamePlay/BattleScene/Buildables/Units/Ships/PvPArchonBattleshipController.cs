using BattleCruisers.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using BattleCruisers.Effects;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships
{
    public class PvPArchonBattleshipController : PvPShipController
    {
        private IBroadcastingAnimation _unfurlAnimation;

        public GameObject bones;
        public AudioSource bellowAudioSource, crankAudioSource, chainAudioSource, dieselAudioSource;

        public override bool IsUltra => true;
        public override Vector2 Size => base.Size * 2;

        public override Vector2 DroneAreaSize => base.Size;

        public Vector2 droneAreaPositionAdjustment;
        public override Vector2 DroneAreaPosition => FacingDirection == Direction.Right ? Position + droneAreaPositionAdjustment : Position - droneAreaPositionAdjustment;

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            PvPHelper.AssertIsNotNull(bones, bellowAudioSource, crankAudioSource, chainAudioSource, dieselAudioSource);

            _unfurlAnimation = bones.GetComponent<IBroadcastingAnimation>();
            Assert.IsNotNull(_unfurlAnimation);
            _unfurlAnimation.AnimationDone += _unfurlAnimation_AnimationDone;

            PvPTargetProxy[] colliderTargetProxies = GetComponentsInChildren<PvPTargetProxy>(includeInactive: true);
            foreach (PvPTargetProxy targetProxy in colliderTargetProxies)
            {
                targetProxy.Initialise(this);
            }
        }

        public override void Initialise()
        {
            base.Initialise();
        }

        public override void Initialise(PvPUIManager uiManager)
        {
            base.Initialise(uiManager);
        }

        public override void Activate(PvPBuildableActivationArgs activationArgs)
        {
            //Debug.Log("Done 4 head");
            bones.SetActive(false);
            base.Activate(activationArgs);
            bones.SetActive(true);
        }

        protected override void OnShipCompleted()
        {
            SetVisibleBones(true);
            if (IsHost)
                OnSetVisibleBoneClientRpc(true);
            base.OnShipCompleted();
        }


        SpriteRenderer[] renders;

        private void Start()
        {
            renders = bones.GetComponentsInChildren<SpriteRenderer>();
            SetVisibleBones(false);
        }

        private void SetVisibleBones(bool isVisible)
        {
            bones.GetComponent<Animator>().enabled = isVisible;
            foreach (SpriteRenderer render in renders)
            {
                render.enabled = isVisible;
            }
        }

        private void _unfurlAnimation_AnimationDone(object sender, EventArgs e)
        {
            if (IsServer)
                base.OnShipCompleted();
        }

        protected override void Deactivate()
        {
            base.Deactivate();
            // bones.SetActive(false);
            SetVisibleBones(false);
            OnSetVisibleBoneClientRpc(false);
        }

        protected override void OnDestroyedEvent()
        {
            if (IsServer)
            {
                OnDestroyedEventClientRpc();
            }
            else
            {
                // bones.SetActive(false);
                SetVisibleBones(false);
                base.OnDestroyedEvent();
            }
        }

        //-------------------------------------- RPCs -------------------------------------------------//


        [ClientRpc]
        private void OnSetVisibleBoneClientRpc(bool isVisible)
        {
            if (!IsHost)
                SetVisibleBones(isVisible);
        }
    }
}
