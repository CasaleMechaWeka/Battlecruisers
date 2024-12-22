using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders
{
    public interface IPvPUnitBuildRatelBoostProviders
    {
        ObservableCollection<IPvPBoostProvider> AircraftProviders { get; }
        ObservableCollection<IPvPBoostProvider> ShipProviders { get; }
    }
}
