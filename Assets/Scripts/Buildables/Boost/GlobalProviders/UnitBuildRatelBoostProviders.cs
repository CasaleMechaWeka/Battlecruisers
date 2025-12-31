using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Boost.GlobalProviders
{
    public class UnitBuildRatelBoostProviders
    {
        public ObservableCollection<IBoostProvider> AircraftProviders { get; }
        public ObservableCollection<IBoostProvider> ShipProviders { get; }

        public UnitBuildRatelBoostProviders()
        {
            AircraftProviders = new ObservableCollection<IBoostProvider>();
            ShipProviders = new ObservableCollection<IBoostProvider>();
        }
    }
}
