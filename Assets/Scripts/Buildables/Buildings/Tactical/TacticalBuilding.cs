using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Utils.DataStrctures;
using System.Collections.Generic;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
    // Excludes SpySatelliteLauncherController, because that inerits from SatelliteLauncherController :)
    public abstract class TacticalBuilding : Building
    {
        protected override void AddBuildRateBoostProviders(
            IGlobalBoostProviders globalBoostProviders, 
            IList<IObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_factoryProvider.GlobalBoostProviders.TacticalsBuildRateBoostProviders);
        }
    }
}