using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Boost.GlobalProviders
{
    public interface IUnitBuildRatelBoostProviders
    {
        ObservableCollection<IBoostProvider> AircraftProviders { get; }
        ObservableCollection<IBoostProvider> ShipProviders { get; }
    }
}
