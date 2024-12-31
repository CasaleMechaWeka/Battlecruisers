using BattleCruisers.Buildables;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical.Shields;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships
{
    public class PvPFlakTurtleController : PvPShipController
    {
        private IPvPBarrelWrapper _flakTurret;
        private PvPSectorShieldController _shieldController;
        public float armamentRange;

        private bool isCompleted = false;
        private Animator animator;

        public override float OptimalArmamentRangeInM => armamentRange;
        public bool keepDistanceFromEnemyCruiser;
        public override bool KeepDistanceFromEnemyCruiser => keepDistanceFromEnemyCruiser;
        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            _shieldController = GetComponentInChildren<PvPSectorShieldController>(includeInactive: true);
            animator = GetComponent<Animator>();
            Assert.IsNotNull(_shieldController, "Cannot find PvPSectorShieldController component");
            Assert.IsNotNull(animator, "Animator component could not be found.");
            isCompleted = false;
            animator.enabled = false; // Ensure the animator is disabled by default
            _shieldController.StaticInitialise(commonStrings);
        }

        public override void Activate(PvPBuildableActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

            if (_shieldController != null)
            {
                _shieldController.Initialise(Faction /*,  _factoryProvider.Sound.SoundPlayer */, null, TargetType.Ships);
                _shieldController.gameObject.SetActive(false);
                OnEnableShieldClientRpc(false);
                _localBoosterBoostableGroup.AddBoostable(_shieldController.Stats);
                _localBoosterBoostableGroup.AddBoostProvidersList(_cruiserSpecificFactories.GlobalBoostProviders.ShieldRechargeRateBoostProviders);
            }
        }

        protected override IList<IPvPBarrelWrapper> GetTurrets()
        {
            IList<IPvPBarrelWrapper> turrets = new List<IPvPBarrelWrapper>();

            _flakTurret = transform.FindNamedComponent<IPvPBarrelWrapper>("FlakTurret");
            Assert.IsNotNull(_flakTurret);
            turrets.Add(_flakTurret);

            return turrets;
        }

        protected override void InitialiseTurrets()
        {
            _flakTurret.Initialise(this, _factoryProvider, _cruiserSpecificFactories, SoundKeys.Firing.AntiAir);
        }
        private void PlayAnimation()
        {
            if (!animator.enabled)
            {
                animator.enabled = true;
            }
        }
        private bool ShouldPlayAnimation()
        {
            return isCompleted && !IsMoving;
        }

        //------------------------------------ methods for sync, written by Sava ------------------------------//

        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();

        protected override void OnShipCompleted()
        {
            if (IsServer)
                base.OnShipCompleted();
        }
        private void LateUpdate()
        {
            if (IsServer)
            {
                if (PvP_BuildProgress.Value != BuildProgress)
                    PvP_BuildProgress.Value = BuildProgress;

                if (ShouldPlayAnimation())
                    PlayAnimation();
                EnableAnimatorClientRpc();
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
                OnDestroyedEventClientRpc();
            else
                base.OnDestroyedEvent();
        }

        protected override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();
                _shieldController.gameObject.SetActive(true);
                _shieldController.ActivateShield();
                OnEnableShieldClientRpc(true);
                OnBuildableCompletedClientRpc();
                isCompleted = true;
            }
            else
            {
                OnBuildableCompleted_PvPClient();
            }

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
        private void OnEnableShieldClientRpc(bool enabled)
        {
            if (!IsHost)
                _shieldController.gameObject.SetActive(enabled);
        }

        [ClientRpc]
        private void OnBuildableCompletedClientRpc()
        {
            if (!IsHost)
                OnBuildableCompleted();
            _flakTurret.ApplyVariantStats(this);
        }

        [ClientRpc]
        private void OnBuildableStateValueChangedClientRpc(PvPBuildableState state)
        {
            if (!IsHost)
                BuildableState = state;
        }

        [ClientRpc]
        private void EnableAnimatorClientRpc()
        {
            animator.enabled = true;
        }
    }
}
