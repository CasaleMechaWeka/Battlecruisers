using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.UI.Sound;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.Netcode;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories
{
    public class PvPNavalFactory : PvPFactory
    {
        public LayerMask unitsLayerMask;

        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.NavalFactory;
        public override UnitCategory UnitCategory => UnitCategory.Naval;
        public override LayerMask UnitLayerMask => unitsLayerMask;


        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();

        protected override void AddBuildRateBoostProviders(
            GlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.NavalFactoryProviders);
        }

        protected override IPvPUnitSpawnPositionFinder CreateSpawnPositionFinder()
        {
            return new PvPNavalFactorySpawnPositionFinder(this);
        }




        // Toggle Drone
        protected override void CallRpc_ToggleDroneConsumerFocusCommandExecute()
        {
            base.CallRpc_ToggleDroneConsumerFocusCommandExecute();
            if (!IsHost)
                OnToggleDroneConsumerFocusCommandExecuteServerRpc();
        }

        // Build Completed
        protected override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                //    ParentCruiser.DroneManager.NumOfDrones += numOfDronesProvided;
                base.OnBuildableCompleted();
                OnBuildableCompletedClientRpc();
            }
            else
                OnBuildableCompleted_PvPClient();
        }

        // Placement Sound
        protected override void PlayPlacementSound()
        {
            base.PlayPlacementSound();
            if (IsServer)
                PlayPlacementSoundClientRpc();
        }

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
                OnPlayDeathSoundClientRpc();
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

        // SyncFaction
        protected override void CallRpc_SyncFaction(Faction faction)
        {
            OnSyncFationClientRpc(faction);
        }

        // StartBuildingUnit

        protected override void OnStartBuildingUnit(UnitCategory category, string prefabName, int variantIndex)
        {
            OnStartBuildingUnitServerRpc(category, prefabName, variantIndex);
        }

        // PauseBuildingUnit
        protected override void OnPauseBuildingUnit()
        {
            if (IsServer)
                base.OnPauseBuildingUnit();
            else
                OnPauseBuildingUnitServerRpc();
        }

        // ResumeBuildingUnit
        protected override void OnResumeBuildingUnit()
        {
            if (IsServer)
                base.OnResumeBuildingUnit();
            else
                OnResumeBuildingUnitServerRpc();
        }

        // NewUnitChosen
        protected override void OnNewUnitChosen()
        {
            if (IsServer)
            {
                OnNewUnitChosenClientRpc();
                base.OnNewUnitChosen();
            }
            else
                base.OnNewUnitChosen();
        }

        protected override void OnIsUnitPausedValueChanged(bool isPaused)
        {
            if (IsServer)
                OnIsUnitPausedValueChangedClientRpc(isPaused);
            else
                base.OnIsUnitPausedValueChanged(isPaused);
        }

        protected override void OnUnit_BuildingStarted(ulong objectId)
        {
            if (!IsHost && IsOwner)
                base.OnUnit_BuildingStarted(objectId);
            if (IsServer)
                OnUnit_BuildingStartedClientRpc(objectId);
        }

        protected override void OnUnit_CompletedBuildable(ulong objectId)
        {
            if (IsServer)
                OnUnit_CompletedBuildableClientRpc(objectId);
            else
                base.OnUnit_CompletedBuildable(objectId);
        }

        protected override void OnUnitUnderConstruction_Destroyed()
        {
            if (IsServer)
                OnUnitUnderConstruction_DestroyedClientRpc();
            else
                base.OnUnitUnderConstruction_Destroyed();
        }


        protected override void OnDestroyedEvent()
        {
            if (IsServer)
                OnDestroyedEventClientRpc();
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
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
        }

        // Rpcs





        [ServerRpc]
        private void OnToggleDroneConsumerFocusCommandExecuteServerRpc()
        {
            CallRpc_ToggleDroneConsumerFocusCommandExecute();
        }
        [ClientRpc]
        private void OnBuildableCompletedClientRpc()
        {
            if (!IsHost)
                OnBuildableCompleted();
        }

        [ClientRpc]
        private void PlayPlacementSoundClientRpc()
        {
            if (!IsHost)
                base.PlayPlacementSound();
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
            if (!IsHost)
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
        private void OnSyncFationClientRpc(Faction faction)
        {
            if (!IsHost)
                Faction = faction;
        }

        [ServerRpc(RequireOwnership = true)]
        private void OnStartBuildingUnitServerRpc(UnitCategory category, string prefabName, int variantIndex)
        {
            PvPUnitKey _unitKey = new PvPUnitKey(category, prefabName);
            UnitWrapper = PvPPrefabFactory.GetUnitWrapperPrefab(_unitKey);
            VariantIndex = variantIndex;
        }

        [ServerRpc(RequireOwnership = true)]
        private void OnPauseBuildingUnitServerRpc()
        {
            OnPauseBuildingUnit();
        }
        [ServerRpc(RequireOwnership = true)]
        private void OnResumeBuildingUnitServerRpc()
        {
            OnResumeBuildingUnit();
        }
        [ClientRpc]
        private void OnUnit_BuildingStartedClientRpc(ulong objectId)
        {
            if (!IsHost)
                OnUnit_BuildingStarted(objectId);
        }

        [ClientRpc]
        private void OnUnit_CompletedBuildableClientRpc(ulong objectId)
        {
            if (!IsHost)
                OnUnit_CompletedBuildable(objectId);
        }

        [ClientRpc]
        private void OnUnitUnderConstruction_DestroyedClientRpc()
        {
            if (!IsHost)
                OnUnitUnderConstruction_Destroyed();
        }

        [ClientRpc]
        private void OnIsUnitPausedValueChangedClientRpc(bool isPaused)
        {
            if (!IsHost)
                OnIsUnitPausedValueChanged(isPaused);
        }

        [ClientRpc]
        private void OnNewUnitChosenClientRpc()
        {
            if (!IsHost)
                OnNewUnitChosen();
        }

        [ClientRpc]
        private void OnDestroyedEventClientRpc()
        {
            if (!IsHost)
                OnDestroyedEvent();
        }
    }
}