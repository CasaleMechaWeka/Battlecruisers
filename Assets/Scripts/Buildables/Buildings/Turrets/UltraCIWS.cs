using System.Collections.Generic;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Buildings.Turrets;
public class UltraCIWS : AntiAirTurret
{
    // Start is called before the first frame update
    protected override void AddBuildRateBoostProviders(
        GlobalBoostProviders globalBoostProviders,
        IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
    {
        base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
        buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.UltrasProviders);
    }
}
