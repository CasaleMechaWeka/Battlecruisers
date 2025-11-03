using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using Unity.Netcode;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical
{
    public class PvPLocalBoosterController : PvPTacticalBuilding
    {
        private IBoostProvider _boostProvider;
        private ParticleSystem _boosterGlow;

        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.Booster;
        public override bool IsBoostable => true;

        public float boostMultiplier;

        public override void Initialise()
        {
            base.Initialise();

            _boostProvider = new BoostProvider(boostMultiplier);

            _boosterGlow = transform.FindNamedComponent<ParticleSystem>("LocalBoosterMasterGlow");
            _boosterGlow.gameObject.SetActive(false);
        }

        protected override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();

                // Logging.Log(Tags.LOCAL_BOOSTER, $"About to boost {_parentSlot.NeighbouringSlots.Count} slots :D");

                foreach (PvPSlot slot in _parentSlot.NeighbouringSlots)
                {
                    slot.BoostProviders.Add(_boostProvider);
                }

                _boosterGlow.gameObject.SetActive(true);

                OnEnableBoosterGlowClientRpc(true);
                OnBuildableCompletedClientRpc();
            }
            else
            {
                OnBuildableCompleted_PvPClient();
            }

        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();

            foreach (PvPSlot slot in _parentSlot.NeighbouringSlots)
            {
                slot.BoostProviders.Remove(_boostProvider);
            }

            _boosterGlow.gameObject.SetActive(false);
        }

        // Sava added code


        public override void Initialise(PvPUIManager uiManager)
        {
            base.Initialise(uiManager);
            _boosterGlow = transform.FindNamedComponent<ParticleSystem>("LocalBoosterMasterGlow");
            _boosterGlow.gameObject.SetActive(false);
        }

        // BuildProgress 
        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();









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
            else
                OnDestroyMeServerRpc();
        }

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

        protected override void PlayBuildableConstructionCompletedSound()
        {
            if (IsServer)
                PlayBuildableConstructionCompletedSoundClientRpc();
            else
                base.PlayBuildableConstructionCompletedSound();
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
        protected override void CallRpc_SyncFaction(Faction faction)
        {
            OnSyncFationClientRpc(faction);
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

        [ClientRpc]
        private void OnEnableBoosterGlowClientRpc(bool enabled)
        {
            if (!IsHost)
                _boosterGlow.gameObject.SetActive(enabled);
        }

        [ClientRpc]
        private void OnDestroyedEventClientRpc()
        {
            if (!IsHost)
                OnDestroyedEvent();
        }
    }
}
