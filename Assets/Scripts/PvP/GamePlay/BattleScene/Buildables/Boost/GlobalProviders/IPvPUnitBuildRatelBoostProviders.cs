using System.Collections.ObjectModel;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders
{
    public interface IPvPUnitBuildRatelBoostProviders
    {
        ObservableCollection<IPvPBoostProvider> AircraftProviders { get; }
        ObservableCollection<IPvPBoostProvider> ShipProviders { get; }
    }
}
