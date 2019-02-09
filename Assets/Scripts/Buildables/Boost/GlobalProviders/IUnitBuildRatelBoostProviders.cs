using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Boost.GlobalProviders
{
    public interface IUnitBuildRatelBoostProviders
    {
        IObservableCollection<IBoostProvider> AircraftProviders { get; }
        IObservableCollection<IBoostProvider> ShipProviders { get; }
    }
}
