using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public class DroneStation : Building
    {
        public int numOfDronesProvided;

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
            ParentCruiser.DroneManager.NumOfDrones += numOfDronesProvided;

            base.OnBuildableCompleted();
        }

        protected override void OnDestroyed()
        {
            if (BuildableState == BuildableState.Completed)
            {
                ParentCruiser.DroneManager.NumOfDrones -= numOfDronesProvided;
            }

            base.OnDestroyed();
        }
    }
}
