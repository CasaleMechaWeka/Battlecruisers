using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using BattleCruisers.UI.Sound;
using BattleCruisers.Data.Static;
using BattleCruisers.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical.Shields
{
    public class PvPGrapheneBarrierController : PvPTacticalBuilding
    {
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.Shields;
        public override TargetValue TargetValue => TargetValue.Low;
        public override bool IsBoostable => true;
        private Animator animator;

        private PvPGrapheneSectorShieldController _shieldController;

        protected override void AddBuildRateBoostProviders(
            GlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.ShieldsProviders);
        }

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            _shieldController = GetComponentInChildren<PvPGrapheneSectorShieldController>(includeInactive: true);
            Assert.IsNotNull(_shieldController);
            _shieldController.StaticInitialise();

            animator = GetComponent<Animator>();
            Assert.IsNotNull(animator, "Animator component could not be found.");
            animator.enabled = false; // Ensure the animator is disabled by default

            _shieldController.onShieldDepleted.AddListener(OnShieldDepleted);
            _shieldController.onShieldDamaged.AddListener(OnShieldDamaged);

            HealthChanged += OnHealthChanged;
        }


        public override void Activate(PvPBuildingActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

            _shieldController.Initialise(Faction);
            _shieldController.gameObject.SetActive(false);
            OnEnableShieldClientRpc(false);
            _localBoosterBoostableGroup.AddBoostable(_shieldController.Stats);
        }

        protected override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();
                // Start deploy animation
                animator.enabled = true;

                _shieldController.gameObject.SetActive(true);
                OnEnableShieldClientRpc(true);
                EnableAnimatorClientRpc(); // Notify the client to enable the animator
                OnBuildableCompletedClientRpc();
            }
            else
            {
                OnBuildableCompleted_PvPClient();
            }
        }


        private void OnShieldDepleted()
        {
            base.Destroy();
        }

        private void OnShieldDamaged()
        {
            float newHealth = maxHealth / _shieldController.maxHealth * _shieldController.Health;

            TakeDamage(Health - newHealth, _shieldController.LastDamagedSource ?? EnemyCruiser);
        }

        private void OnHealthChanged(object sender, EventArgs e)
        {
            Debug.Log("OnHealthChanged");
            _shieldController.SetShieldHealth(_shieldController.maxHealth / maxHealth * Health);
        }

        [ClientRpc]
        private void OnBuildableCompletedClientRpc()
        {
            if (!IsHost)
                OnBuildableCompleted();
            _shieldController.ApplyVariantStats(this);
        }

        [ClientRpc]
        private void OnEnableShieldClientRpc(bool enabled)
        {
            if (!IsHost)
                _shieldController.gameObject.SetActive(enabled);
        }

        [ClientRpc]
        private void EnableAnimatorClientRpc()
        {
            animator.enabled = true;
        }
    }
}
