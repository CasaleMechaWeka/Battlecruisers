using BattleCruisers.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets;

public class PvPUltraCIWS : PvPAntiAirTurret
{
    // Start is called before the first frame update
    protected override void AddBuildRateBoostProviders(
        IPvPGlobalBoostProviders globalBoostProviders,
        IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
    {
        base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
        buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.UltrasProviders);
    }
}
