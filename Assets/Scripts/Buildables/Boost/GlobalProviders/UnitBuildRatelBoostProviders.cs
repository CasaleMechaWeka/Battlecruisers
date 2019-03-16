using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Boost.GlobalProviders
{
    public class UnitBuildRatelBoostProviders : IUnitBuildRatelBoostProviders
    {
        public ObservableCollection<IBoostProvider> AircraftProviders { get; private set; }
        public ObservableCollection<IBoostProvider> ShipProviders { get; private set; }

        public UnitBuildRatelBoostProviders()
        {
            AircraftProviders = new ObservableCollection<IBoostProvider>();
            ShipProviders = new ObservableCollection<IBoostProvider>();
        }
    }
}
