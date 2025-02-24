using BattleCruisers.Buildables.Boost;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders
{
    public interface IPvPUnitBuildRatelBoostProviders
    {
        ObservableCollection<IBoostProvider> AircraftProviders { get; }
        ObservableCollection<IBoostProvider> ShipProviders { get; }
    }
}
