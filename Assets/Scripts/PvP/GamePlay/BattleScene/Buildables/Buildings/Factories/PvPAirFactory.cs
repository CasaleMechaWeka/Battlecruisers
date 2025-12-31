using BattleCruisers.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using Unity.Netcode;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories
{
    public class PvPAirFactory : PvPFactory
    {
        public LayerMask aircraftLayerMask;

        public override UnitCategory UnitCategory => UnitCategory.Aircraft;
        public override LayerMask UnitLayerMask => aircraftLayerMask;

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

        protected override IPvPUnitSpawnPositionFinder CreateSpawnPositionFinder()
        {
            return new PvPAirFactorySpawnPositionFinder(this);
        }

        // ----------------------------------------
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
    }
}