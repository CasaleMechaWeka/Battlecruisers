using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Sound;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.Netcode;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Offensive
{
    public class PvPDeathstarLauncherController : PvPSatelliteLauncherController
    {
        protected override Vector3 SpawnPositionAdjustment => new Vector3(0.003f, 0.21f, 0);
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Ultra;
        public override TargetValue TargetValue => TargetValue.High;

        protected override void AddBuildRateBoostProviders(
            GlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.UltrasProviders);
        }

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            // Need satellite to be initialised to be able to access damage capabilities.
            satellitePrefab.StaticInitialise();

            foreach (IDamageCapability damageCapability in satellitePrefab.Buildable.DamageCapabilities)
            {
                AddDamageStats(damageCapability);
            }
        }

        // sava added
        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();










        // Death Sound
        protected override void CallRpc_PlayDeathSound()
        {
            if (IsServer)
            {
                OnPlayDeathSoundClientRpc();
                base.CallRpc_PlayDeathSound();
            }
            else
                base.CallRpc_PlayDeathSound();
        }

        // BuildableConstructionCompletedSound
        protected override void PlayBuildableConstructionCompletedSound()
        {
            if (IsServer)
                PlayBuildableConstructionCompletedSoundClientRpc();
            else
                base.PlayBuildableConstructionCompletedSound();
        }
        // ProgressController Visible
        protected override void CallRpc_ProgressControllerVisible(bool isEnabled)
        {
            OnProgressControllerVisibleClientRpc(isEnabled);
        }

        // BuildableStatus
        protected override void OnBuildableStateValueChanged(PvPBuildableState state)
        {
            OnBuildableStateValueChangedClientRpc(state);
        }



        protected override void OnDestroyedEvent()
        {
            if (IsServer)
            {
                OnDestroyedEventClientRpc();
                base.OnDestroyedEvent();
            }
            else
                base.OnDestroyedEvent();
        }


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

        // ----------------------------------------






        [ClientRpc]
        private void PlayPlacementSoundClientRpc()
        {
            if (!IsHost)
                base.PlayPlacementSound();
        }

        [ClientRpc]
        private void OnPlayDeathSoundClientRpc()
        {
            if (!IsHost)
                CallRpc_PlayDeathSound();
        }
        [ClientRpc]
        private void PlayBuildableConstructionCompletedSoundClientRpc()
        {
            if (!IsHost)
                PlayBuildableConstructionCompletedSound();
        }

        [ClientRpc]
        private void OnProgressControllerVisibleClientRpc(bool isEnabled)
        {
            _buildableProgress.gameObject.SetActive(isEnabled);
        }

        [ClientRpc]
        protected void OnBuildableStateValueChangedClientRpc(PvPBuildableState state)
        {
            if (!IsHost)
                BuildableState = state;
        }



        [ServerRpc(RequireOwnership = true)]
        private void OnStartBuildingUnitServerRpc(UnitCategory category, string prefabName)
        {
            PvPUnitKey _unitKey = new PvPUnitKey(category, prefabName);
            //    UnitWrapper = PvPBattleSceneGodServer.Instance.prefabFactory.GetUnitWrapperPrefab(_unitKey);
        }

        [ClientRpc]
        private void OnDestroyedEventClientRpc()
        {
            if (!IsHost)
                OnDestroyedEvent();
        }
    }
}
