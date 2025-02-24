using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.AudioSources;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using BattleCruisers.UI.Sound;
using BattleCruisers.Data.Static;

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
        public override Vector2 DroneAreaPosition => FacingDirection == Direction.Right ? Position + droneAreaPositionAdjustment : Position - droneAreaPositionAdjustment;
        public override bool KeepDistanceFromEnemyCruiser => false;

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
        }

        public override void Initialise(IPvPFactoryProvider factoryProvider, IPvPUIManager uiManager)
        {
            base.Initialise(factoryProvider, uiManager);
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
            // Delay normal setup (movement, turrets) until the unfurl animation has completed
        }


        SpriteRenderer[] renders;
        private void Awake()
        {

        }
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

        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Ultra;

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
            OnSetVisibleBoneClientRpc(false);
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
            else
            {
                BuildProgress = PvP_BuildProgress.Value;
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
                pvp_Health.Value = maxHealth;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
        }
        // Visibility 
        protected override void OnValueChangedIsEnableRenderes(bool isEnabled)
        {
            if (IsServer)
                OnValueChangedIsEnabledRendersClientRpc(isEnabled);
            else
                base.OnValueChangedIsEnableRenderes(isEnabled);
        }

        // ProgressController Visible
        protected override void CallRpc_ProgressControllerVisible(bool isEnabled)
        {
            if (IsServer)
            {
                OnProgressControllerVisibleClientRpc(isEnabled);
                base.CallRpc_ProgressControllerVisible(isEnabled);
            }
            else
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
            if (IsServer)
                OnBuildableProgressEventClientRpc();
            else
                base.OnBuildableProgressEvent();
        }

        protected override void OnCompletedBuildableEvent()
        {
            if (IsServer)
                OnCompletedBuildableEventClientRpc();
            else
                base.OnCompletedBuildableEvent();
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

        protected override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();
                OnBuildableCompletedClientRpc();
            }
            else
                OnBuildableCompleted_PvPClient();
        }


        //-------------------------------------- RPCs -------------------------------------------------//

        [ClientRpc]
        private void OnValueChangedIsEnabledRendersClientRpc(bool isEnabled)
        {
            if (!IsHost)
                OnValueChangedIsEnableRenderes(isEnabled);
        }

        [ClientRpc]
        private void OnProgressControllerVisibleClientRpc(bool isEnabled)
        {
            _buildableProgress.gameObject.SetActive(isEnabled);
            if (!IsHost)
                CallRpc_ProgressControllerVisible(isEnabled);
        }

        [ClientRpc]
        private void OnSetPositionClientRpc(Vector3 pos)
        {
            if (!IsHost)
                Position = pos;
        }

        [ClientRpc]
        private void OnSetRotationClientRpc(Quaternion rotation)
        {
            if (!IsHost)
                Rotation = rotation;
        }

        [ClientRpc]
        private void OnActivatePvPClientRpc()
        {
            if (!IsHost)
                Activate_PvPClient();
        }

        [ClientRpc]
        private void OnBuildableProgressEventClientRpc()
        {
            if (!IsHost)
                OnBuildableProgressEvent();
        }

        [ClientRpc]
        private void OnCompletedBuildableEventClientRpc()
        {
            if (!IsHost)
                OnCompletedBuildableEvent();
        }

        [ClientRpc]
        private void OnDestroyedEventClientRpc()
        {
            if (!IsHost)
                OnDestroyedEvent();
        }

        [ClientRpc]
        private void OnBuildableCompletedClientRpc()
        {
            if (!IsHost)
                OnBuildableCompleted();
            laser.ApplyVariantStats(this);
        }

        [ClientRpc]
        private void OnBuildableStateValueChangedClientRpc(PvPBuildableState state)
        {
            if (!IsHost)
                BuildableState = state;
        }

        [ClientRpc]
        private void OnSetVisibleBoneClientRpc(bool isVisible)
        {
            if (!IsHost)
                SetVisibleBones(isVisible);
        }

    }
}
