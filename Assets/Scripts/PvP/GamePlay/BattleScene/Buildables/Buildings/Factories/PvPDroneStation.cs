using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.Netcode;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories
{
    public class PvPDroneStation : PvPBuilding
    {

        public int numOfDronesProvided;

        public NetworkVariable<Quaternion> PvP_Rotation = new NetworkVariable<Quaternion>();
        public NetworkVariable<Vector2> PvP_Position = new NetworkVariable<Vector2>();
        public NetworkVariable<Vector2> PvP_Offset = new NetworkVariable<Vector2>();
        public NetworkVariable<bool> PvP_IsEnabledRenderers = new NetworkVariable<bool>();
        public NetworkVariable<bool> PvP_IsEnabledBuildableProgressController = new NetworkVariable<bool>();
        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();
        public NetworkVariable<PvPBuildableState> PvP_BuildableState = new NetworkVariable<PvPBuildableState>();

        protected override PvPPrioritisedSoundKey ConstructionCompletedSoundKey => PvPPrioritisedSoundKeys.PvPCompleted.PvPBuildings.DroneStation;
        public override PvPTargetValue TargetValue => PvPTargetValue.Medium;

        protected override void AddBuildRateBoostProviders(
            IPvPGlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IPvPBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.DroneBuildingsProviders);
        }

        protected override void ShareIsDroneConsumerFocusableValueWithClient(bool isFocusable)
        {
            //    base.ShareIsDroneConsumerFocusableValueWithClient(isFocusable);
            OnShareIsDroneConsumerFocusableValueWithClientRpc(isFocusable);
        }
        protected override void CallRpc_ToggleDroneConsumerFocusCommandExecute()
        {
            base.CallRpc_ToggleDroneConsumerFocusCommandExecute();
            if (IsClient)
                OnToggleDroneConsumerFocusCommandExecuteServerRpc();
        }
        protected override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                ParentCruiser.DroneManager.NumOfDrones += numOfDronesProvided;        
                base.OnBuildableCompleted();
                OnBuildableCompletedClientRpc();
            //    Invoke("OnBuildableCompletedClientRpc", 0.1f);
            }
            if (IsClient)
                OnBuildableCompleted_PvPClient();
        }



        protected override void OnDestroyed()
        {
            if (BuildableState == PvPBuildableState.Completed)
            {
                ParentCruiser.DroneManager.NumOfDrones -= numOfDronesProvided;
            }

            base.OnDestroyed();
        }

        protected override void PlayPlacementSound()
        {
            base.PlayPlacementSound();

            if (IsServer)
                PlayPlacementSoundClientRpc();
        }


        protected override void DestroyMe()
        {
            if (IsServer)
                base.DestroyMe();
            if (IsClient)
                OnDestroyMeServerRpc();
        }

        protected override void CallRpc_PlayDeathSound()
        {
            if (IsClient)
                base.CallRpc_PlayDeathSound();
            if (IsServer)
                OnPlayDeathSoundClientRpc();
        }

        protected override void PlayBuildableConstructionCompletedSound()
        {
            if (IsClient)
                base.PlayBuildableConstructionCompletedSound();
            if (IsServer)
                PlayBuildableConstructionCompletedSoundClientRpc();
        }


        private void LateUpdate()
        {

            if (IsServer)
            {
                if (PvP_Position.Value != Position)
                    PvP_Position.Value = Position;
                if (PvP_Rotation.Value != Rotation)
                    PvP_Rotation.Value = Rotation;
                if (HealthBar is not null && PvP_Offset.Value != HealthBar.Offset)
                    PvP_Offset.Value = HealthBar.Offset;

                if (PvP_IsEnabledRenderers.Value != isEnabledRenderers)
                    PvP_IsEnabledRenderers.Value = isEnabledRenderers;
                if (PvP_IsEnabledBuildableProgressController.Value != _buildableProgress.gameObject.activeSelf)
                    PvP_IsEnabledBuildableProgressController.Value = _buildableProgress.gameObject.activeSelf;

                if (PvP_BuildProgress.Value != BuildProgress)
                    PvP_BuildProgress.Value = BuildProgress;
                if (PvP_BuildableState.Value != BuildableState)
                    PvP_BuildableState.Value = BuildableState;
            }
            if (IsClient)
            {
                Position = PvP_Position.Value;
                Rotation = PvP_Rotation.Value;
                if (HealthBar is not null)
                    HealthBar.Offset = PvP_Offset.Value;
                isEnabledRenderers = PvP_IsEnabledRenderers.Value;
                _buildableProgress.gameObject.SetActive(PvP_IsEnabledBuildableProgressController.Value);
                BuildProgress = PvP_BuildProgress.Value;
                BuildableState = PvP_BuildableState.Value;
            }
        }

        private void Start()
        {
            if (IsClient && IsOwner)
                Faction = PvPFaction.Blues;
            if (IsClient && !IsOwner)
                Faction = PvPFaction.Reds;
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
        private void OnBuildableCompletedClientRpc()
        {
            OnBuildableCompleted();
        }

    }
}
