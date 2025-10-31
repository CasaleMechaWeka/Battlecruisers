using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
    public class SpySatelliteLauncherController : SatelliteLauncherController, IBuilding
    {
        protected override Vector3 SpawnPositionAdjustment => new Vector3(0, 0.17f, 0);
        public override TargetValue TargetValue => TargetValue.Medium;

        protected override void AddBuildRateBoostProviders(
            GlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.TacticalsProviders);
        }
    }
}