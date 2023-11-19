using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories
{
    public class PvPAirFactory : PvPFactory
    {
        public LayerMask aircraftLayerMask;

        protected override PvPPrioritisedSoundKey ConstructionCompletedSoundKey => PvPPrioritisedSoundKeys.PvPCompleted.PvPBuildings.AirFactory;
        public override PvPUnitCategory UnitCategory => PvPUnitCategory.Aircraft;
        public override LayerMask UnitLayerMask => aircraftLayerMask;


        // sava added
        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();

        // Visibility 
        protected override void OnValueChangedIsEnableRenderes(bool isEnabled)
        {
            if (IsServer)
                OnValueChangedIsEnabledRendersClientRpc(isEnabled);
            else
                base.OnValueChangedIsEnableRenderes(isEnabled);
        }

        // Headbar offset
        protected override void CallRpc_SetHealthbarOffset(Vector2 offset)
        {
            OnSetHealthbarOffsetClientRpc(offset);
        }


        // set Position of PvPBuildable
        protected override void CallRpc_SetPosition(Vector3 pos)
        {
            OnSetPositionClientRpc(pos);
        }

        // Set Rotation of PvPBuildable
        protected override void CallRpc_SetRotation(Quaternion rotation)
        {
            OnSetRotationClientRpc(rotation);
        }

        // Drone Focusing
        protected override void ShareIsDroneConsumerFocusableValueWithClient(bool isFocusable)
        {
            OnShareIsDroneConsumerFocusableValueWithClientRpc(isFocusable);
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
        protected override void CallRpc_SyncFaction(PvPFaction faction)
        {
            OnSyncFationClientRpc(faction);
        }

        // StartBuildingUnit

        protected override void OnStartBuildingUnit(PvPUnitCategory category, string prefabName, int variantIndex)
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
            if (IsServer)
                OnUnit_BuildingStartedClientRpc(objectId);
            else
                base.OnUnit_BuildingStarted(objectId);
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



        // ----------------------------------------
        protected override void AddBuildRateBoostProviders(
            IPvPGlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IPvPBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.AirFactoryProviders);
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
            if (IsServer)
                pvp_Health.Value = maxHealth;
        }
        protected override IPvPUnitSpawnPositionFinder CreateSpawnPositionFinder()
        {
            return _factoryProvider.SpawnDeciderFactory.CreateAircraftSpawnPositionFinder(this);
        }

        // ----------------------------------------

        [ClientRpc]
        private void OnValueChangedIsEnabledRendersClientRpc(bool isEnabled)
        {
            if (!IsHost)
                OnValueChangedIsEnableRenderes(isEnabled);
        }

        [ClientRpc]
        private void OnSetHealthbarOffsetClientRpc(Vector2 offset)
        {
            if (!IsHost)
                HealthBar.Offset = offset;
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
        private void OnShareIsDroneConsumerFocusableValueWithClientRpc(bool isFocusable)
        {
            if (!IsHost)
                IsDroneConsumerFocusable_PvPClient = isFocusable;
        }

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
            IPvPDroneConsumer repairDroneConsumer = ParentCruiser.RepairManager.GetDroneConsumer(this);
            ParentCruiser.DroneFocuser.ToggleDroneConsumerFocus(repairDroneConsumer, isTriggeredByPlayer: true);
        }

        [ClientRpc]
        private void OnSyncFationClientRpc(PvPFaction faction)
        {
            if (!IsHost)
                Faction = faction;
        }

        [ServerRpc(RequireOwnership = true)]
        private void OnStartBuildingUnitServerRpc(PvPUnitCategory category, string prefabName, int variantIndex)
        {
            PvPUnitKey _unitKey = new PvPUnitKey(category, prefabName);
            UnitWrapper = PvPBattleSceneGodServer.Instance.prefabFactory.GetUnitWrapperPrefab(_unitKey);
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