using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
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
    }
}
