using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Units;
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
    public class PvPAirFactory : PvPFactory
    {
        public LayerMask aircraftLayerMask;

        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.AirFactory;
        public override UnitCategory UnitCategory => UnitCategory.Aircraft;
        public override LayerMask UnitLayerMask => aircraftLayerMask;

        // sava added
        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();






        // Build Completed
        



        // Death Sound
        protected override void CallRpc_PlayDeathSound()
        {
            if (IsServer)
                OnPlayDeathSoundClientRpc();
            else
                base.CallRpc_PlayDeathSound();
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
            GlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
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
            base.OnNetworkSpawn();
            if (IsServer)
                pvp_Health.Value = maxHealth;
        }

        protected override IPvPUnitSpawnPositionFinder CreateSpawnPositionFinder()
        {
            return new PvPAirFactorySpawnPositionFinder(this);
        }

        // ----------------------------------------









        [ClientRpc]
        private void OnPlayDeathSoundClientRpc()
        {
            if (!IsHost)
                CallRpc_PlayDeathSound();
        }

        [ClientRpc]
        private void OnProgressControllerVisibleClientRpc(bool isEnabled)
        {
            if (!IsHost)
                _buildableProgress.gameObject.SetActive(isEnabled);
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