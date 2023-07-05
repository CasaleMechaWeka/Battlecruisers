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

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories
{
    public class PvPNavalFactory : PvPFactory
    {
        public LayerMask unitsLayerMask;

        protected override PvPPrioritisedSoundKey ConstructionCompletedSoundKey => PvPPrioritisedSoundKeys.PvPCompleted.PvPBuildings.NavalFactory;
        public override PvPUnitCategory UnitCategory => PvPUnitCategory.Naval;
        public override LayerMask UnitLayerMask => unitsLayerMask;


        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();

        protected override void AddBuildRateBoostProviders(
            IPvPGlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IPvPBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.NavalFactoryProviders);
        }

        protected override IPvPUnitSpawnPositionFinder CreateSpawnPositionFinder()
        {
            return _factoryProvider.SpawnDeciderFactory.CreateNavalSpawnPositionFinder(this);
        }


        // Visibility 
        protected override void OnValueChangedIsEnableRenderes(bool isEnabled)
        {
            if (IsClient)
                base.OnValueChangedIsEnableRenderes(isEnabled);
            if (IsServer)
                OnValueChangedIsEnabledRendersClientRpc(isEnabled);
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
            if (IsClient)
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
            if (IsClient)
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
            if (IsClient)
                OnDestroyMeServerRpc();
        }

        // Death Sound
        protected override void CallRpc_PlayDeathSound()
        {
            if (IsClient)
                base.CallRpc_PlayDeathSound();
            if (IsServer)
                OnPlayDeathSoundClientRpc();
        }

        // BuildableConstructionCompletedSound
        protected override void PlayBuildableConstructionCompletedSound()
        {
            if (IsClient)
                base.PlayBuildableConstructionCompletedSound();
            if (IsServer)
                PlayBuildableConstructionCompletedSoundClientRpc();
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

        protected override void OnStartBuildingUnit(PvPUnitCategory category, string prefabName)
        {
            OnStartBuildingUnitServerRpc(category, prefabName);
        }

        // PauseBuildingUnit
        protected override void OnPauseBuildingUnit()
        {
            if (IsClient)
                OnPauseBuildingUnitServerRpc();
            if (IsServer)
                base.OnPauseBuildingUnit();

        }

        // ResumeBuildingUnit
        protected override void OnResumeBuildingUnit()
        {
            if (IsClient)
                OnResumeBuildingUnitServerRpc();
            if (IsServer)
                base.OnResumeBuildingUnit();
        }

        // NewUnitChosen
        protected override void OnNewUnitChosen()
        {
            if (IsClient)
            {
                OnNewUnitChosenServerRpc();
            }
            if (IsServer)
                base.OnNewUnitChosen();

        }

        protected override void OnIsUnitPausedValueChanged(bool isPaused)
        {
            if (IsClient)
                base.OnIsUnitPausedValueChanged(isPaused);
            if (IsServer)
                OnIsUnitPausedValueChangedClientRpc(isPaused);
        }

        protected override void OnUnit_BuildingStarted(ulong objectId)
        {
            if (IsClient)
                base.OnUnit_BuildingStarted(objectId);
            if (IsServer)
                OnUnit_BuildingStartedClientRpc(objectId);

        }

        protected override void OnUnit_CompletedBuildable(ulong objectId)
        {
            if (IsClient)
                base.OnUnit_CompletedBuildable(objectId);
            if (IsServer)
                OnUnit_CompletedBuildableClientRpc(objectId);
        }

        protected override void OnUnitUnderConstruction_Destroyed()
        {
            if (IsClient)
                base.OnUnitUnderConstruction_Destroyed();
            if (IsServer)
                OnUnitUnderConstruction_DestroyedClientRpc();
        }

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


        // Rpcs

        [ClientRpc]
        private void OnValueChangedIsEnabledRendersClientRpc(bool isEnabled)
        {
            OnValueChangedIsEnableRenderes(isEnabled);
        }

        [ClientRpc]
        private void OnSetHealthbarOffsetClientRpc(Vector2 offset)
        {
            HealthBar.Offset = offset;
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
        private void OnShareIsDroneConsumerFocusableValueWithClientRpc(bool isFocusable)
        {
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
            OnBuildableCompleted();
        }

        [ClientRpc]
        private void PlayPlacementSoundClientRpc()
        {
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
            CallRpc_PlayDeathSound();
        }

        [ClientRpc]
        private void PlayBuildableConstructionCompletedSoundClientRpc()
        {
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
            Faction = faction;
        }

        [ServerRpc(RequireOwnership = true)]
        private void OnStartBuildingUnitServerRpc(PvPUnitCategory category, string prefabName)
        {
            PvPUnitKey _unitKey = new PvPUnitKey(category, prefabName);
            UnitWrapper = PvPBattleSceneGodServer.Instance.prefabFactory.GetUnitWrapperPrefab(_unitKey);
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
            OnUnit_BuildingStarted(objectId);
        }

        [ClientRpc]
        private void OnUnit_CompletedBuildableClientRpc(ulong objectId)
        {
            OnUnit_CompletedBuildable(objectId);
        }

        [ClientRpc]
        private void OnUnitUnderConstruction_DestroyedClientRpc()
        {
            OnUnitUnderConstruction_Destroyed();
        }

        [ClientRpc]
        private void OnIsUnitPausedValueChangedClientRpc(bool isPaused)
        {
            OnIsUnitPausedValueChanged(isPaused);
        }

        [ServerRpc(RequireOwnership = true)]
        private void OnNewUnitChosenServerRpc()
        {
            OnNewUnitChosen();
        }
    }
}