using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Sound;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical
{
    public class PvPSpySatelliteLauncherController : PvPSatelliteLauncherController, IPvPBuilding
    {
        protected override Vector3 SpawnPositionAdjustment => new Vector3(0, 0.17f, 0);
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.SpySatellite;
        public override TargetValue TargetValue => TargetValue.Medium;

        protected override void AddBuildRateBoostProviders(
            GlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.TacticalsProviders);
        }

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);
            Assert.IsNotNull(satellitePrefab);
            // Need satellite to be initialised to be able to access damage capabilities.
            satellitePrefab.StaticInitialise();
        }


        // sava added
        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();










        // Destroy me
        protected override void DestroyMe()
        {
            if (IsServer)
                base.DestroyMe();
            else
                OnDestroyMeServerRpc();
        }

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

        // ClickedRepairButton
        protected override void CallRpc_ClickedRepairButton()
        {
            PvP_RepairableButtonClickedServerRpc();
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







        [ClientRpc]
        private void PlayPlacementSoundClientRpc()
        {
            if (!IsHost)
                PlayPlacementSound();
        }

        [ServerRpc(RequireOwnership = true)]
        private void OnDestroyMeServerRpc()
        {
            DestroyMe();
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
        private void PvP_RepairableButtonClickedServerRpc()
        {
            IDroneConsumer repairDroneConsumer = ParentCruiser.RepairManager.GetDroneConsumer(this);
            ParentCruiser.DroneFocuser.ToggleDroneConsumerFocus(repairDroneConsumer, isTriggeredByPlayer: true);
        }



        [ClientRpc]
        private void OnDestroyedEventClientRpc()
        {
            if (!IsHost)
                OnDestroyedEvent();
        }
    }
}