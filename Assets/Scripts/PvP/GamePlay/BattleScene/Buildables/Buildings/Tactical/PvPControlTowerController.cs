using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using Unity.Netcode;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.UI.Sound;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical
{
    public class PvPControlTowerController : PvPTacticalBuilding
    {
        private IBoostProvider _boostProvider;

        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.ControlTower;

        protected override void AddBuildRateBoostProviders(
            GlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.MastStructureProviders);
        }


        public float boostMultiplier;

        public override void Initialise()
        {
            base.Initialise();
            _boostProvider = new BoostProvider(boostMultiplier);
        }

        protected override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();
                _cruiserSpecificFactories.GlobalBoostProviders.AircraftBoostProviders.Add(_boostProvider);
                OnBuildableCompletedClientRpc();
            }
            else
                OnBuildableCompleted_PvPClient();
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
            _cruiserSpecificFactories.GlobalBoostProviders.AircraftBoostProviders.Remove(_boostProvider);
        }

        // sava added code
        // BuildProgress 
        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();













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
        private void OnPlayDeathSoundClientRpc()
        {
            if (!IsHost)
                CallRpc_PlayDeathSound();
        }


        [ClientRpc]
        private void OnDestroyedEventClientRpc()
        {
            if (!IsHost)
                OnDestroyedEvent();
        }
    }
}
