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

        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();

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
    }
}
