using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
    public class StealthGenerator : TacticalBuilding, IBuilding
    {

        protected override void AddBuildRateBoostProviders(
            GlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.MastStructureProviders);
        }
    }
}
