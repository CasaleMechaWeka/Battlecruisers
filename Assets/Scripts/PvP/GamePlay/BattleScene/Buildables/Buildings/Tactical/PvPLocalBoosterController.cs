using BattleCruisers.Buildables.Buildings.Tactical.Shields;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using Unity.Netcode;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical
{
    public class PvPLocalBoosterController : PvPTacticalBuilding
    {
        private IPvPBoostProvider _boostProvider;
        private ParticleSystem _boosterGlow;

        protected override PvPPrioritisedSoundKey ConstructionCompletedSoundKey => PvPPrioritisedSoundKeys.PvPCompleted.PvPBuildings.Booster;
        public override bool IsBoostable => true;

        public float boostMultiplier;

        public override void Initialise(IPvPFactoryProvider factoryProvider)
        {
            base.Initialise(factoryProvider);

            _boostProvider = _factoryProvider.BoostFactory.CreateBoostProvider(boostMultiplier);

            _boosterGlow = transform.FindNamedComponent<ParticleSystem>("LocalBoosterMasterGlow");
            _boosterGlow.gameObject.SetActive(false);
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            // Logging.Log(Tags.LOCAL_BOOSTER, $"About to boost {_parentSlot.NeighbouringSlots.Count} slots :D");

            foreach (IPvPSlot slot in _parentSlot.NeighbouringSlots)
            {
                slot.BoostProviders.Add(_boostProvider);
            }

            _boosterGlow.gameObject.SetActive(true);

            OnEnableBoosterGlowClientRpc(true);
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();

            foreach (IPvPSlot slot in _parentSlot.NeighbouringSlots)
            {
                slot.BoostProviders.Remove(_boostProvider);
            }

            _boosterGlow.gameObject.SetActive(false);
        }

        // Sava added code


        public override void Initialise(IPvPFactoryProvider factoryProvider, IPvPUIManager uiManager)
        {
            base.Initialise(factoryProvider, uiManager);
            _boosterGlow = transform.FindNamedComponent<ParticleSystem>("LocalBoosterMasterGlow");
            _boosterGlow.gameObject.SetActive(false);
        }

        // BuildProgress 
        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();


        // Visibility
        protected override void OnValueChangedIsEnableRenderes(bool isEnabled)
        {
            if (IsClient)
                base.OnValueChangedIsEnableRenderes(isEnabled);
            if (IsServer)
                OnValueChangedIsEnabledRendersClientRpc(isEnabled);
        }

        // Healthbar offset
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

        protected override void ShareIsDroneConsumerFocusableValueWithClient(bool isFocusable)
        {
            OnShareIsDroneConsumerFocusableValueWithClientRpc(isFocusable);
        }

        protected override void CallRpc_ToggleDroneConsumerFocusCommandExecute()
        {
            base.CallRpc_ToggleDroneConsumerFocusCommandExecute();
            if (IsClient)
                OnToggleDroneConsumerFocusCommandExecuteServerRpc();
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

        protected override void CallRpc_ProgressControllerVisible(bool isEnabled)
        {
            OnProgressControllerVisibleClientRpc(isEnabled);
        }

        protected override void OnBuildableStateValueChanged(PvPBuildableState state)
        {
            OnBuildableStateValueChangedClientRpc(state);
        }

        protected override void CallRpc_ClickedRepairButton()
        {
            PvP_RepairableButtonClickedServerRpc();
        }
        protected override void CallRpc_SyncFaction(PvPFaction faction)
        {
            OnSyncFationClientRpc(faction);
        }


        protected override void OnDestroyedEvent()
        {
            if (IsClient)
                base.OnDestroyedEvent();
            if (IsServer)
                OnDestroyedEventClientRpc();
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

        public override void OnNetworkSpawn()
        {
            if (IsServer)
                pvp_Health.Value = maxHealth;
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
        private void OnSetHealthbarOffsetClientRpc(Vector2 offset)
        {
            HealthBar.Offset = offset;
        }

        [ClientRpc]
        private void OnValueChangedIsEnabledRendersClientRpc(bool isEnabled)
        {
            OnValueChangedIsEnableRenderes(isEnabled);
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

        [ClientRpc]
        private void OnEnableBoosterGlowClientRpc(bool enabled)
        {
            _boosterGlow.gameObject.SetActive(enabled);
        }

        [ClientRpc]
        private void OnDestroyedEventClientRpc()
        {
            OnDestroyedEvent();
        }


    }
}
