using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Boost.GlobalProviders
{
    public class UnitBuildRatelBoostProviders : IUnitBuildRatelBoostProviders
    {
        public IObservableCollection<IBoostProvider> AircraftProviders { get; private set; }
        public IObservableCollection<IBoostProvider> ShipProviders { get; private set; }

        public UnitBuildRatelBoostProviders()
        {
            AircraftProviders = new ObservableCollection<IBoostProvider>();
            ShipProviders = new ObservableCollection<IBoostProvider>();
        }
    }
}
