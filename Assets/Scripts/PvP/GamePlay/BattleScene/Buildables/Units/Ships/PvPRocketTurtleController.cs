using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical.Shields;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships
{
    public class PvPRocketTurtleController : PvPShipController
    {
        private IPvPBarrelWrapper _missileLauncher;
        private PvPSectorShieldController _shieldController;
        public float armamentRange;

        private bool isCompleted = false;
        private Animator animator;

        public override float OptimalArmamentRangeInM => armamentRange;
        public bool keepDistanceFromEnemyCruiser;
        public override bool KeepDistanceFromEnemyCruiser => keepDistanceFromEnemyCruiser;
        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            _shieldController = GetComponentInChildren<PvPSectorShieldController>(includeInactive: true);
            animator = GetComponent<Animator>();
            Assert.IsNotNull(_shieldController, "Cannot find PvPSectorShieldController component");
            Assert.IsNotNull(animator, "Animator component could not be found.");
            isCompleted = false;
            animator.enabled = false; // Ensure the animator is disabled by default
            _shieldController.StaticInitialise();
        }

        public override void Activate(PvPBuildableActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

            if (_shieldController != null)
            {
                _shieldController.Initialise(Faction, TargetType.Ships);
                _shieldController.gameObject.SetActive(false);
                OnEnableShieldClientRpc(false);
                _localBoosterBoostableGroup.AddBoostable(_shieldController.Stats);
                _localBoosterBoostableGroup.AddBoostProvidersList(_cruiserSpecificFactories.GlobalBoostProviders.ShieldRechargeRateBoostProviders);
            }
        }

        protected override IList<IPvPBarrelWrapper> GetTurrets()
        {
            IList<IPvPBarrelWrapper> turrets = new List<IPvPBarrelWrapper>();

            _missileLauncher = transform.FindNamedComponent<IPvPBarrelWrapper>("MissileLauncher");
            Assert.IsNotNull(_missileLauncher);
            turrets.Add(_missileLauncher);

            return turrets;
        }

        protected override void InitialiseTurrets()
        {
            _missileLauncher.Initialise(this, _cruiserSpecificFactories);
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

        protected override void OnShipCompleted()
        {
            if (IsServer)
                base.OnShipCompleted();
        }
        /*
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
        */

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
            _missileLauncher.ApplyVariantStats(this);
        }

        [ClientRpc]
        private void EnableAnimatorClientRpc()
        {
            animator.enabled = true;
        }
    }
}
