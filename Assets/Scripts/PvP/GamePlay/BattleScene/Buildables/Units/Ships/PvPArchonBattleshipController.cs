using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects;
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
using Unity.Netcode;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships
{
    public class PvPArchonBattleshipController : PvPShipController
    {
        private IPvPBroadcastingAnimation _unfurlAnimation;
        private PvPAudioSourceGroup _unfurlAudioGroup;

        public PvPBarrelWrapper laser;
        public GameObject bones;
        public AudioSource bellowAudioSource, crankAudioSource, chainAudioSource, dieselAudioSource;

        public override bool IsUltra => true;
        public override Vector2 Size => base.Size * 2;

        public override Vector2 DroneAreaSize => base.Size;

        public Vector2 droneAreaPositionAdjustment;
        public override Vector2 DroneAreaPosition => FacingDirection == PvPDirection.Right ? Position + droneAreaPositionAdjustment : Position - droneAreaPositionAdjustment;

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            PvPHelper.AssertIsNotNull(bones, laser, bellowAudioSource, crankAudioSource, chainAudioSource, dieselAudioSource);

            _unfurlAnimation = bones.GetComponent<IPvPBroadcastingAnimation>();
            Assert.IsNotNull(_unfurlAnimation);
            _unfurlAnimation.AnimationDone += _unfurlAnimation_AnimationDone;

            PvPTargetProxy[] colliderTargetProxies = GetComponentsInChildren<PvPTargetProxy>(includeInactive: true);
            foreach (PvPTargetProxy targetProxy in colliderTargetProxies)
            {
                targetProxy.Initialise(this);
            }
        }

        public override void Initialise(/* IPvPUIManager uiManager,*/ IPvPFactoryProvider factoryProvider)
        {
            base.Initialise(/* uiManager,*/ factoryProvider);

            // _unfurlAudioGroup
            //     = new PvPAudioSourceGroup(
            //         factoryProvider.SettingsManager,
            //         new PvPAudioSourceBC(bellowAudioSource),
            //         new PvPAudioSourceBC(crankAudioSource),
            //         new PvPAudioSourceBC(chainAudioSource),
            //         new PvPAudioSourceBC(dieselAudioSource));
        }

        public override void Initialise(IPvPFactoryProvider factoryProvider, IPvPUIManager uiManager)
        {
            base.Initialise(factoryProvider, uiManager);
        }

        public override void Activate(PvPBuildableActivationArgs activationArgs)
        {
            //Debug.Log("Done 4 head");
            base.Activate(activationArgs);

        }

        protected override void OnShipCompleted()
        {
            // Show bones, starting unfurl animation
            //    bones.SetActive(true);

            SetVisibleBones(true);
            // Delay normal setup (movement, turrets) until the unfurl animation has completed
        }


        SpriteRenderer[] renders;
        private void Awake()
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
                return laser.RangeInM;
            }
        }

        protected override PvPPrioritisedSoundKey ConstructionCompletedSoundKey => PvPPrioritisedSoundKeys.PvPCompleted.Ultra;

        protected override IList<IPvPBarrelWrapper> GetTurrets()
        {
            return new List<IPvPBarrelWrapper>()
            {
                laser
            };
        }

        protected override void InitialiseTurrets()
        {
            laser.Initialise(this, _factoryProvider, _cruiserSpecificFactories);
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
            // bones.SetActive(false);
            SetVisibleBones(false);
        }

        //------------------------------------ methods for sync, written by Sava ------------------------------//

        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();

        private void LateUpdate()
        {
            if (IsServer)
            {
                if (PvP_BuildProgress.Value != BuildProgress)
                    PvP_BuildProgress.Value = BuildProgress;
            }
            if (IsClient)
            {
                BuildProgress = PvP_BuildProgress.Value;
            }
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
                pvp_Health.Value = maxHealth;
        }

        // Visibility 
        protected override void OnValueChangedIsEnableRenderes(bool isEnabled)
        {
            if (IsClient)
                base.OnValueChangedIsEnableRenderes(isEnabled);
            if (IsServer)
                OnValueChangedIsEnabledRendersClientRpc(isEnabled);
        }

        // ProgressController Visible
        protected override void CallRpc_ProgressControllerVisible(bool isEnabled)
        {
            if (IsServer)
                OnProgressControllerVisibleClientRpc(isEnabled);
            if (IsClient)
                base.CallRpc_ProgressControllerVisible(isEnabled);
        }


        // set Position of PvPBuildable
        protected override void CallRpc_SetPosition(Vector3 pos)
        {
            //  OnSetPositionClientRpc(pos);
        }

        // Set Rotation of PvPBuildable
        protected override void CallRpc_SetRotation(Quaternion rotation)
        {
            OnSetRotationClientRpc(rotation);
        }

        // BuildableStatus
        protected override void OnBuildableStateValueChanged(PvPBuildableState state)
        {
            OnBuildableStateValueChangedClientRpc(state);
        }

        protected override void OnBuildableProgressEvent()
        {
            if (IsClient)
                base.OnBuildableProgressEvent();
            if (IsServer)
                OnBuildableProgressEventClientRpc();
        }

        protected override void OnCompletedBuildableEvent()
        {
            if (IsClient)
                base.OnCompletedBuildableEvent();
            if (IsServer)
                OnCompletedBuildableEventClientRpc();
        }

        protected override void OnDestroyedEvent()
        {
            if (IsClient)
            {
                // bones.SetActive(false);
                SetVisibleBones(false);
                base.OnDestroyedEvent();
            }

            if (IsServer)
            {
                OnDestroyedEventClientRpc();
            }

        }

        protected override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();
                OnBuildableCompletedClientRpc();
            }
            if (IsClient)
                OnBuildableCompleted_PvPClient();
        }


        //-------------------------------------- RPCs -------------------------------------------------//

        [ClientRpc]
        private void OnValueChangedIsEnabledRendersClientRpc(bool isEnabled)
        {
            OnValueChangedIsEnableRenderes(isEnabled);
        }

        [ClientRpc]
        private void OnProgressControllerVisibleClientRpc(bool isEnabled)
        {
            _buildableProgress.gameObject.SetActive(isEnabled);
            CallRpc_ProgressControllerVisible(isEnabled);
        }

        [ClientRpc]
        private void OnSetPositionClientRpc(Vector3 pos)
        {
            Position = pos;
        }

        [ClientRpc]
        private void OnSetRotationClientRpc(Quaternion rotation)
        {
            Rotation = rotation;
        }

        [ClientRpc]
        private void OnActivatePvPClientRpc()
        {
            Activate_PvPClient();
        }

        [ClientRpc]
        private void OnBuildableProgressEventClientRpc()
        {
            OnBuildableProgressEvent();
        }

        [ClientRpc]
        private void OnCompletedBuildableEventClientRpc()
        {
            OnCompletedBuildableEvent();
        }

        [ClientRpc]
        private void OnDestroyedEventClientRpc()
        {
            OnDestroyedEvent();
        }

        [ClientRpc]
        private void OnBuildableCompletedClientRpc()
        {
            OnBuildableCompleted();
        }

        [ClientRpc]
        private void OnBuildableStateValueChangedClientRpc(PvPBuildableState state)
        {
            BuildableState = state;
        }

    }
}
