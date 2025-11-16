using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories
{
    public class PvPDroneStation : PvPBuilding
    {
        public int numOfDronesProvided;
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.DroneStation;
        public override TargetValue TargetValue => TargetValue.Medium;

        protected override void AddBuildRateBoostProviders(
            GlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.DroneBuildingsProviders);
        }

        protected override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                ParentCruiser.DroneManager.NumOfDrones += numOfDronesProvided;
                base.OnBuildableCompleted();
                OnBuildableCompletedClientRpc();
            }
            else
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
    }
}
